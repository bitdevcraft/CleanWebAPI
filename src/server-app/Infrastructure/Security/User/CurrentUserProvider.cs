using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Throw;

namespace Infrastructure.Security.User;

public class CurrentUserProvider(IHttpContextAccessor _httpContextAccessor) : ICurrentUserProvider
{
    public CurrentUser GetCurrentUser()
    {
        _httpContextAccessor.HttpContext.ThrowIfNull();

        var userName = GetSingleClaimValue(ClaimTypes.Name);
        var id = Guid.Parse(GetSingleClaimValue(ClaimTypes.NameIdentifier));
        var permissions = GetClaimValues("permissions");
        var roles = GetClaimValues(ClaimTypes.Role);
        var email = GetSingleClaimValue(ClaimTypes.Email);

        return new CurrentUser(id, userName, email, permissions, roles);
    }

    private List<string> GetClaimValues(string claimType) =>
        _httpContextAccessor
            .HttpContext!.User.Claims.Where(claim => claim.Type == claimType)
            ?.Select(claim => claim.Value)
            .ToList();

    private string GetSingleClaimValue(string claimType) =>
        _httpContextAccessor
            .HttpContext!.User.Claims.Single(claim => claim.Type == claimType)
            ?.Value;
}
