using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

public class ApplicationUser : IdentityUser
{
    //
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
