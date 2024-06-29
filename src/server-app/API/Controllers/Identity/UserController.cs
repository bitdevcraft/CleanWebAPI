using API.Controllers.Base;
using Application.User.Login;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Identity;

public class UserController : ApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await Mediator.Send(new UserLogin.Command { Email = loginDto.email, Password = loginDto.password  });
        return result.Match(
                _ => Ok(), 
                Problem
            );
    }
}

public class LoginDto
{
    public string email { get; set; }
    public string password { get; set; }
}
