using Application.Common.Interfaces;
using Domain.Identity;
using Infrastructure.Persistence;
using Infrastructure.Security;
using Infrastructure.Security.PolicyEnforcer;
using Infrastructure.Security.TokenGenerator;
using Infrastructure.Security.TokenValidation;
using Infrastructure.Security.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddPersistence(configuration);
        services.AddAuthenticationService(configuration);
        services.AddIdentityService(configuration);
        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Setup the connection to the Database
        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetService<ApplicationDbContext>()
        );

        services.AddScoped<ApplicationDbContextInitialiser>();

        return services;
    }

    private static IServiceCollection AddAuthenticationService(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddAuthorization();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        services.AddSingleton<IPolicyEnforcer, PolicyEnforcer>();

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services
            .ConfigureOptions<JwtBearerTokenValidationConfiguration>()
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        return services;
    }

    private static IServiceCollection AddIdentityService(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        // .AddSignInManager<SignInManager<ApplicationUser>>();

        services.AddHttpContextAccessor();
        services.AddScoped<IUserAccessor, UserAccessor>();
        return services;
    }
}
