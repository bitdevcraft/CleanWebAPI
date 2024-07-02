using Application.Common.Security.Roles;
using Domain.Entities;
using Domain.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(
        ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        _context = context;
        _userManager = userManager;

        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var sysAdminRole = new IdentityRole(Roles.SystemAdministrator);

        if (_roleManager.Roles.All(r => r.Name != sysAdminRole.Name))
        {
            await _roleManager.CreateAsync(sysAdminRole);
        }

        // Default users
        var administrator = new ApplicationUser
        {
            UserName = "sysadmin@localhost",
            Email = "sysadmin@localhost"
        };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(sysAdminRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { sysAdminRole.Name });
            }
        }

        if (!_context.Items.Any())
        {
            _context.Items.Add(new Item { Name = "Retro Handheld", Description = "Console" });

            await _context.SaveChangesAsync();
        }
    }
}
