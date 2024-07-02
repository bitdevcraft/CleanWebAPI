using System.Reflection;
using Application.Common.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            options.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
        });

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
