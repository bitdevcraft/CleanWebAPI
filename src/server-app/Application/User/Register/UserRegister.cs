using Domain.Identity;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.User.Register;

public class UserRegister
{
    public class Command : IRequest<ErrorOr<Unit>>
    {
        public UserRegisterDto User { get; set; }
    }

    public class Handler : IRequestHandler<Command, ErrorOr<Unit>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public Handler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ErrorOr<Unit>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == request.User.UserName))
            {
                return Error.Validation(description: "Username Taken");
            }

            var user = new ApplicationUser
            {
                Email = request.User.Email,
                UserName = request.User.UserName,
            };

            var result = await _userManager.CreateAsync(user, request.User.Password);

            if (!result.Succeeded)
                return Error.Validation(description: "Problem registering user");

            return Unit.Value;
        }
    }
}
