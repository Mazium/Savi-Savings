using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Savi_Thrift.Domain.Entities;

namespace Savi_Thrift.Common.Utilities
{
    public class Seeder
    {
        public static async Task SeedRolesAndSuperAdmin(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // Seed roles
            if (!await roleManager.RoleExistsAsync("SuperAdmin"))
            {
                var role = new IdentityRole("SuperAdmin");
                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                var role = new IdentityRole("User");
                await roleManager.CreateAsync(role);
            }

            if (userManager.FindByNameAsync("Admin").Result == null)
            {
                var user = new AppUser
                {
                    UserName = "Admin",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    CreatedAt = DateTime.UtcNow
                };

                var result = userManager.CreateAsync(user, "Password@123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "SuperAdmin").Wait();
                }
            }
        }



    }
}
