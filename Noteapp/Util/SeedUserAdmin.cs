using Microsoft.AspNetCore.Identity;
using NoteApp.Models;

namespace Noteapp.Util
{
    public class SeedAdminUser
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public SeedAdminUser(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task InitializeAsync()
        {
            // Sprawdź, czy administrator już istnieje
            if (await _userManager.FindByEmailAsync("admin@admin.com") == null)
            {
                // Tworzenie roli, jeśli nie istnieje
                if (!await _roleManager.RoleExistsAsync("ROLE_ADMIN"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("ROLE_ADMIN"));
                }

                // Tworzenie administratora
                var adminUser = new User
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                };

                var result = await _userManager.CreateAsync(adminUser, "adminPassword");

                if (result.Succeeded)
                {
                    // Przypisanie roli administratora
                    await _userManager.AddToRoleAsync(adminUser, "ADMIN");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create admin user: {errors}");
                }
            }
        }
    }

}
