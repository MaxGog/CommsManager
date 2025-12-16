using Microsoft.AspNetCore.Identity;
using CommsManager.Models;

namespace CommsManager.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            string[] roles = { "Admin", "Artist", "Crafter", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string email = "artist@example.com";
            string password = "Artist123!";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    DisplayName = "Тестовый Художник",
                    Bio = "Профессиональный художник и крафтер",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Artist");
                }
            }
        }
    }
}