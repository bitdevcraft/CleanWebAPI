namespace Infrastructure.Security.User;

public interface ICurrentUserProvider
{
    CurrentUser GetCurrentUser();
}
