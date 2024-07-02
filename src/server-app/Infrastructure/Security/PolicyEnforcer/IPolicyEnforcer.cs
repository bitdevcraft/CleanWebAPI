using Application.Common.Security.Request;
using ErrorOr;
using Infrastructure.Security.User;

namespace Infrastructure.Security.PolicyEnforcer;

public interface IPolicyEnforcer
{
    public ErrorOr<Success> Authorize<T>(
        IAuthorizeableRequest<T> request,
        CurrentUser currentUser,
        string policy
    );
}
