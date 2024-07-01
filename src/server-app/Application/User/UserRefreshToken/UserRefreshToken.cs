using System.Security.Claims;
using Application.Common.Interfaces;
using Domain.Identity;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.User.UserRefreshToken;

public class UserRefreshToken
{
    public class Command : IRequest<ErrorOr<UserDto>>
    {
        public string RefreshToken { get; set; }
    }

    public class Handler : IRequestHandler<Command, ErrorOr<UserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserAccessor _userAccessor;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public Handler(
            UserManager<ApplicationUser> userManager,
            IUserAccessor userAccessor,
            IJwtTokenGenerator jwtTokenGenerator
        )
        {
            _userAccessor = userAccessor;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userManager = userManager;
        }

        public async Task<ErrorOr<UserDto>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
                
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine(_userAccessor.GetUsername());
            Console.ResetColor();

            var user = await _userManager
                .Users.Include(r => r.RefreshTokens)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            if (user == null)
                return Error.Unauthorized();

            var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == request.RefreshToken);

            if (oldToken != null && !oldToken.IsActive)
                return Error.Unauthorized();

            if (oldToken != null)
                oldToken.Revoked = DateTime.UtcNow;

            return new UserDto
            {
                Username = user.UserName,
                Token = _jwtTokenGenerator.GenerateToken(user)
            };
        }
    }
}
