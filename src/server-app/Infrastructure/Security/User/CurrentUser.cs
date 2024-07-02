namespace Infrastructure.Security.User;

public record CurrentUser(
    Guid Id,
    string UserName,
    string Email,
    IReadOnlyList<string> Permissions,
    IReadOnlyList<string> Roles
);
