using Microsoft.AspNetCore.Identity;
using EkoTrack.Data;
using EkoTrack.Models;

namespace EkoTrack.Data;

public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            
        const string adminRole = "Admin";
        const string adminEmail = "admin@admin.com";
        const string adminPassword = "Password123!"; 

        if (await roleManager.FindByNameAsync(adminRole) == null)
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new AppUser 
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true 
            };
                
            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
    }
}