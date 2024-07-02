using Application.Common.Security.Policies;
using Application.Common.Security.Request;
using Application.Common.Security.Roles;
using ErrorOr;
using Infrastructure.Security.User;

namespace Infrastructure.Security.PolicyEnforcer;

public class PolicyEnforcer : IPolicyEnforcer
{
    public ErrorOr<Success> Authorize<T>(
        IAuthorizeableRequest<T> request,
        CurrentUser currentUser,
        string policy
    )
    {
        return policy switch
        {
            Policy.SystemAdministrator => SystemAdministrator(request, currentUser),
            _ => Error.Unexpected(description: "Unknown policy name"),
        };
    }

    private static ErrorOr<Success> SystemAdministrator<T>(
        IAuthorizeableRequest<T> request,
        CurrentUser currentUser
    ) =>
        currentUser.Roles.Contains(Roles.SystemAdministrator)
            ? Result.Success
            : Error.Unauthorized(description: "Requesting user failed policy requirement");
}
