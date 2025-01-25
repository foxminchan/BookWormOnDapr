using BookWorm.Identity.Data;
using BookWorm.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BookWorm.Identity;

public sealed class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var admin = new IdentityRole("Admin");
        if (roleMgr.Roles.All(r => r.Name != admin.Name))
        {
            roleMgr.CreateAsync(admin);
        }

        var user = new IdentityRole("User");
        if (roleMgr.Roles.All(r => r.Name != user.Name))
        {
            roleMgr.CreateAsync(user);
        }

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var adminUser = userMgr.FindByNameAsync("admin").Result;
        if (adminUser is null)
        {
            adminUser = new()
            {
                UserName = "admin",
                Email = "admin@bookworm.com",
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = true
            };

            var result = userMgr.CreateAsync(adminUser, "P@ssw0rd123").Result;

            if (!result.Succeeded)
            {
                throw new(result.Errors.First().Description);
            }

            result = userMgr
                .AddClaimsAsync(
                    adminUser,
                    [
                        new(JwtClaimTypes.Name, "Admin"),
                        new(JwtClaimTypes.WebSite, "http://bookworm.com"),
                        new(JwtClaimTypes.Locale, "en-US")
                    ]
                )
                .Result;

            if (!result.Succeeded)
            {
                throw new(result.Errors.First().Description);
            }

            result = userMgr.AddToRoleAsync(adminUser, admin.Name!).Result;

            if (!result.Succeeded)
            {
                throw new(result.Errors.First().Description);
            }

            Log.Debug("admin created");
        }
        else
        {
            Log.Debug("admin already exists");
        }

        var customerUser = userMgr.FindByNameAsync("customer").Result;

        if (customerUser is null)
        {
            customerUser = new()
            {
                UserName = "customer",
                Email = "customer@bookworm.com",
                EmailConfirmed = true,
                PhoneNumber = "9876543210",
                PhoneNumberConfirmed = true
            };
            var result = userMgr.CreateAsync(customerUser, "P@ssw0rd123").Result;

            if (!result.Succeeded)
            {
                throw new(result.Errors.First().Description);
            }

            result = userMgr
                .AddClaimsAsync(
                    customerUser,
                    [
                        new(JwtClaimTypes.Name, "Customer"),
                        new(JwtClaimTypes.WebSite, "http://bookworm.com"),
                        new(JwtClaimTypes.Locale, "en-US")
                    ]
                )
                .Result;

            if (!result.Succeeded)
            {
                throw new(result.Errors.First().Description);
            }

            result = userMgr.AddToRoleAsync(customerUser, user.Name!).Result;

            if (!result.Succeeded)
            {
                throw new(result.Errors.First().Description);
            }

            Log.Debug("customer created");
        }
        else
        {
            Log.Debug("customer already exists");
        }
    }
}
