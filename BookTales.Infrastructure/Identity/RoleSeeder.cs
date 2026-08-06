using BookTales.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace BookTales.Infrastructure.Identity;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        string[] roles =
        {
            "Admin",
            "User"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = role
                });
            }
        }
    }
}