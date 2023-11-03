using CarShop.IdentityServer.Data;
using CarShop.IdentityServer.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

namespace CarShop.IdentityServer
{
    public class SeedData
    {
        public static async void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();
                await CreateRoles(scope);

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                ApplicationUser admin = await userMgr.FindByNameAsync("admin");
                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@email.com",
                        EmailConfirmed = true,
                    };
                    var result = userMgr.CreateAsync(admin, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(admin, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Liza Glebtsova"),
                            new Claim(JwtClaimTypes.GivenName, "Liza"),
                            new Claim(JwtClaimTypes.FamilyName, "Glebtsova"),
                            new Claim(JwtClaimTypes.WebSite, "https://lizaonair.com/"),
                        }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("admin created");
                }
                else
                {
                    Log.Debug("admin already exists");
                }

                ApplicationUser user = await userMgr.FindByNameAsync("user");
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = "user",
                        Email = "user@email.com",
                        EmailConfirmed = true
                    };
                    var result = userMgr.CreateAsync(user, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(user, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Fake User"),
                            new Claim(JwtClaimTypes.GivenName, "User"),
                            new Claim(JwtClaimTypes.FamilyName, "Fake"),
                            new Claim(JwtClaimTypes.WebSite, "http://user.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("user created");
                }
                else
                {
                    Log.Debug("user already exists");
                }
            }
        }

        private static async Task CreateRoles(IServiceScope serviceScope)
        {
            var RoleManager = serviceScope.ServiceProvider
                                    .GetRequiredService<RoleManager<IdentityRole>>();

            bool isAdminRoleExists = await RoleManager.RoleExistsAsync("admin");
            if (!isAdminRoleExists)
            {
                await RoleManager.CreateAsync(new IdentityRole("admin"));
            }

            bool isUserRoleExists = await RoleManager.RoleExistsAsync("user");
            if (!isUserRoleExists)
            {
                await RoleManager.CreateAsync(new IdentityRole("user"));
            }
        }
    }
}