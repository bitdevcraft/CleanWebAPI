using API.Controllers.Base;
using Application.User;
using Application.User.Login;
using Application.User.Register;
using Application.User.UserRefreshToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace API.Controllers.Identity;

public class UserController : ApiController
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await Mediator.Send(
            new UserLogin.Command { UserName = loginDto.UserName, Password = loginDto.Password }
        );

        return result.Match(
            userDto =>
            {
                SetRefeshToken(userDto.RefreshToken);
                return Ok(CreateUserResponse(userDto.Username, userDto.Token));
            },
            Problem
        );
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
    {
        var result = await Mediator.Send(new UserRegister.Command { User = userRegisterDto });

        return result.Match(_ => Ok(), Problem);
    }

    [Authorize]
    [HttpPost("refreshToken")]
    public async Task<ActionResult<UserDto>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var result = await Mediator.Send(
            new UserRefreshToken.Command { RefreshToken = refreshToken }
        );

        return result.Match(
            userDto => Ok(CreateUserResponse(userDto.Username, userDto.Token)),
            Problem
        );
    }

    // Private Methods
    private void SetRefeshToken(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }

    private static UserResponse CreateUserResponse(string username, string token)
    {
        return new UserResponse(username, token);
    }
}

public class LoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public record UserResponse(string UserName, string Token);
