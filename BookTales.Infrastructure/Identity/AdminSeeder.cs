using BookTales.Domain.Entities;
using BookTales.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookTales.Infrastructure.Identity;

public static class AdminSeeder
{
    public static async Task SeedAdminAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ApplicationDbContext context)
    {
        const string adminEmail = "admin@booktales.com";
        const string adminPassword = "Admin@123";

        // Make sure Admin role exists
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new ApplicationRole
            {
                Name = "Admin"
            });
        }

        // Check if admin already exists
        var existingAdmin =
            await userManager.FindByEmailAsync(adminEmail);

        if (existingAdmin != null)
        {
            // Make sure the existing account has Admin role
            if (!await userManager.IsInRoleAsync(existingAdmin, "Admin"))
            {
                await userManager.AddToRoleAsync(
                    existingAdmin,
                    "Admin");
            }

            return;
        }

        // Create domain user
        var domainUser = new User
        {
            FirstName = "BookTales",
            LastName = "Admin",
            Email = adminEmail,
            PhoneNumber = string.Empty
        };

        context.DomainUsers.Add(domainUser);
        await context.SaveChangesAsync();

        // Create Identity user
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            DomainUserId = domainUser.Id
        };

        var result = await userManager.CreateAsync(
            adminUser,
            adminPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(e => e.Description));

            throw new Exception(
                $"Failed to create admin user: {errors}");
        }

        // Assign Admin role
        var roleResult =
            await userManager.AddToRoleAsync(
                adminUser,
                "Admin");

        if (!roleResult.Succeeded)
        {
            var errors = string.Join(
                ", ",
                roleResult.Errors.Select(e => e.Description));

            throw new Exception(
                $"Failed to assign Admin role: {errors}");
        }
    }
}