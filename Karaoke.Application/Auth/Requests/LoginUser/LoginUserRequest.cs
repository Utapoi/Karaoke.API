﻿using FluentValidation;
using JetBrains.Annotations;
using Karaoke.Application.Interfaces.Auth;
using MediatR;

namespace Karaoke.Application.Auth.Requests.LoginUser;

public static class LoginUser
{
    public sealed class Request : IRequest<LoginUserResponse>
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    [UsedImplicitly]
    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    [UsedImplicitly]
    internal sealed class Handler : IRequestHandler<Request, LoginUserResponse>
    {
        private readonly IAuthService _authService;

        public Handler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginUserResponse> Handle(Request request, CancellationToken cancellationToken)
        {
            var (result, userId) = await _authService.LoginUserAsync(request);

            return new LoginUserResponse
            {
                Result = result,
                UserId = userId
            };
        }
    }
}