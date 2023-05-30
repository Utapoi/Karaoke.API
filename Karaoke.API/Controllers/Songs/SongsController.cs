﻿using Karaoke.Application.DTO;
using Karaoke.Application.Songs.Requests.GetSongs;
using Microsoft.AspNetCore.Mvc;

namespace Karaoke.API.Controllers.Songs;

/// <summary>
///     Songs controller.
/// </summary>
public sealed class SongsController : ApiControllerBase
{
    private readonly ILogger<SongsController> _logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SongsController" /> class.
    /// </summary>
    /// <param name="logger">
    ///     The logger.
    /// </param>
    public SongsController(ILogger<SongsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Gets all songs.
    /// </summary>
    /// <returns>
    ///     An <see cref="IEnumerable{T}" /> of <see cref="SongDTO" />.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetSongs.Response), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            var m = new GetSongs.Request();
            var response = await Mediator.Send(m);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting songs.");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}