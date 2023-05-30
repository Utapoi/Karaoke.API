﻿using Karaoke.API.Requests.Auth;
using Karaoke.Application.Auth.Commands.GetToken;
using Karaoke.Application.Auth.Commands.RefreshToken;
using Karaoke.Application.Auth.Commands.RegisterUser;
using Karaoke.Application.Auth.Requests.LoginUser;
using Karaoke.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Karaoke.API.Controllers.Auth;

/// <summary>
///     The controller for authentication.
/// </summary>
public class AuthController : ApiControllerBase
{
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AuthController" /> class.
    /// </summary>
    /// <param name="logger">
    ///     The logger.
    /// </param>
    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Registers a new user.
    /// </summary>
    /// <param name="request">
    ///     The request.
    /// </param>
    /// <returns>
    ///     A <see cref="string" /> containing the UserId or a <see cref="IEnumerable{T}" /> of <see cref="string" />
    ///     containing the errors.
    /// </returns>
    [HttpPost("Register")]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        try
        {
            var m = new RegisterUser.Command
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
                IpAddress = GetOriginFromRequest()
            };

            var response = await Mediator.Send(m);

            if (!response.Succeeded)
            {
                return BadRequest(response.Message);
            }

            return StatusCode(StatusCodes.Status201Created);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Error registering in user: {Username}", request.Username);
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user: {Username} / {Email}", request.Username, request.Email);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    ///     Logs in a user.
    /// </summary>
    /// <param name="request">
    ///     The request.
    /// </param>
    /// <returns>
    ///     A <see cref="string" /> containing the UserId or a <see cref="IEnumerable{T}" /> of <see cref="string" />
    ///     containing the errors.
    /// </returns>
    [HttpPost("Login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        try
        {
            var m = new LoginUser.Request
            {
                Username = request.Username,
                Password = request.Password,
                IpAddress = GetOriginFromRequest()
            };

            var response = await Mediator.Send(m);

            if (!response.Succeeded)
            {
                return BadRequest(response.Message);
            }

            return Ok();
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Error logging in user: {Username}", request.Username);
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging in user: {Username}", request.Username);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    ///     Gets a token for a user.
    /// </summary>
    /// <param name="request">
    ///     The request.
    /// </param>
    /// <returns>
    ///     A <see cref="GetToken.Response" /> containing the token or an error.
    /// </returns>
    [HttpPost("GetToken")]
    [ProducesResponseType(typeof(GetToken.Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTokenAsync([FromBody] GetTokenRequest request)
    {
        try
        {
            var m = new GetToken.Command
            {
                Username = request.Username,
                Password = request.Password,
                IpAddress = GetOriginFromRequest()
            };

            var response = await Mediator.Send(m);

            return Ok(response);
        }
        catch (ForbiddenAccessException ex)
        {
            _logger.LogError(ex, "Error retrieving token");
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Failed to generate token: {Username}", request.Username);
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting token for user: {Username}", request.Username);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    ///     Refreshes a token.
    /// </summary>
    /// <param name="request">
    ///     The request.
    /// </param>
    /// <returns>
    ///     A <see cref="RefreshToken.Response" /> containing the token or an error.
    /// </returns>
    [HttpPost("RefreshToken")]
    [ProducesResponseType(typeof(RefreshToken.Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var m = new RefreshToken.Command
            {
                Token = request.Token,
                RefreshToken = request.RefreshToken,
                IpAddress = GetOriginFromRequest()
            };

            var response = await Mediator.Send(m);

            return Ok(response);
        }
        catch (ForbiddenAccessException ex)
        {
            _logger.LogError(ex, "Error refreshing token");
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Failed to refresh token: {Token}", request.Token);
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}