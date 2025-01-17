using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security.User;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUsername()
    {
        return _httpContextAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Name);
    }
}
