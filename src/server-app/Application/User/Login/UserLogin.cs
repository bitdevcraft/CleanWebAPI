using System.Net;
using Application.Common.Interfaces;
using Domain.Identity;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User.Login;

public class UserLogin
{
    public class Command : IRequest<ErrorOr<UserDto>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class Handler : IRequestHandler<Command, ErrorOr<UserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public Handler(
            UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator
        )
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ErrorOr<UserDto>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
                return Error.Unauthorized();

            await _userManager.CheckPasswordAsync(user, request.Password);

            var accessToken = _jwtTokenGenerator.GenerateToken(user);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken { Token = refreshToken });

            await _userManager.UpdateAsync(user);

            return new UserDto
            {
                Username = user.UserName,
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
