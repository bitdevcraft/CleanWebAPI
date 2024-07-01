using System.Security.Claims;
using Domain.Identity;

namespace Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(ApplicationUser user);
    string GenerateRefreshToken();
}
