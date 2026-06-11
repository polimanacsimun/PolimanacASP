using InventoryManagement.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagement.Data
{
    public static class IdentitySeedData
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] roles =
            {
                "Admin",
                "Manager"
            };

            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            await CreateUserIfMissing(
                userManager,
                email: "admin@inventory.local",
                password: "Admin123!",
                firstName: "Admin",
                lastName: "User",
                oib: "12345678901",
                jmbg: "1234567890123",
                role: "Admin");

            await CreateUserIfMissing(
                userManager,
                email: "manager@inventory.local",
                password: "Manager123!",
                firstName: "Manager",
                lastName: "User",
                oib: "23456789012",
                jmbg: "2345678901234",
                role: "Manager");
        }

        private static async Task CreateUserIfMissing(
            UserManager<AppUser> userManager,
            string email,
            string password,
            string firstName,
            string lastName,
            string oib,
            string jmbg,
            string role)
        {
            var existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                if (!await userManager.IsInRoleAsync(existingUser, role))
                {
                    await userManager.AddToRoleAsync(existingUser, role);
                }

                return;
            }

            var user = new AppUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName,
                OIB = oib,
                JMBG = jmbg
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
            else
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Could not create seed user {email}: {errors}");
            }
        }
    }
}