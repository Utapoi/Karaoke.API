﻿using FluentValidation;
using JetBrains.Annotations;
using Karaoke.Application.Identity.Auth;
using Karaoke.Application.Identity.Tokens;
using MediatR;

namespace Karaoke.Application.Auth.Requests.LoginUser;

public static class LoginUser
{
    public sealed class Request : IRequest<TokenResponse?>
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string IpAddress { get; set; } = string.Empty;
    }

    [UsedImplicitly]
    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.IpAddress).NotEmpty();
        }
    }

    [UsedImplicitly]
    internal sealed class Handler : IRequestHandler<Request, TokenResponse?>
    {
        private readonly IAuthService _authService;

        public Handler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<TokenResponse?> Handle(Request request, CancellationToken cancellationToken)
        {
            return _authService.LoginUserAsync(request, cancellationToken);
        }
    }
}