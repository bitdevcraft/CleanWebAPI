using Domain.Identity;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User.Login;

public class UserLogin
{
    public class Command : IRequest<ErrorOr<Unit>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Handler : IRequestHandler<Command, ErrorOr<Unit>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public Handler(UserManager<ApplicationUser> userManager) 
        {
            _userManager = userManager;
        }

        public async Task<ErrorOr<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Error.Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            return Unit.Value;            
        }
    }
}
