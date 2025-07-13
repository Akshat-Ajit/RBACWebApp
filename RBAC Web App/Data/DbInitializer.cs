using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RBACWebApp.Models;

namespace RBACWebApp.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Define roles
            string[] roleNames = { "Admin", "Manager", "User" };

            // Create roles if they don't exist
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed default Admin user
            var adminEmail = "admin@rbac.com";
            var adminPassword = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    FullName = "Super Admin",
                    UserName = adminEmail,
                    Email = adminEmail
                };

                var result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    // Add to Admin role
                    await userManager.AddToRoleAsync(user, "Admin");

                    // Confirm email so login works immediately
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    await userManager.ConfirmEmailAsync(user, token);
                }
                else
                {
                    // Optional: log or throw errors if user creation fails
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Admin User Creation Error: {error.Description}");
                    }
                }
            }
        }
    }
}
