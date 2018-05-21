using ElectronicJournal.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal
{
    public class StartInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "kapitula_ak16@nuwm.edu.ua";
            string password = "!Journal123";
            string adminName = "Дмитро";
            string adminLastName = "Капітула";
            Group adminGroup = null;

            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (await roleManager.FindByNameAsync("GroupLeader") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("GroupLeader"));
            }

            if (await roleManager.FindByNameAsync("Student") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Student"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User {
                    Email = adminEmail,
                    UserName = adminEmail,
                    Name = adminName,
                    LastName = adminLastName,
                    Group = adminGroup
                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
