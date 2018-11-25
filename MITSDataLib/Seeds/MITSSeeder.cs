using Microsoft.AspNetCore.Identity;
using MITSDataLib.Contexts;
using MITSDataLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITSDataLib.Seeds
{
    public class MITSSeeder
    {

        private readonly MITSContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public MITSSeeder(MITSContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            var role = await _roleManager.RoleExistsAsync("Admin");

            

            if (!role)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole("Admin"));

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Could not create admin role");
                }
            }

            User user = await _userManager.FindByEmailAsync("enderjs@gmail.com");

            // Seed the Main User

            if (user == null)
            {
                user = new User()
                {
                    LastName = "Silvers",
                    FirstName = "Jason",
                    Email = "enderjs@gmail.com",
                    UserName = "enderjs@gmail.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                       
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create user in Seeding");
                }

                
            }

            await _userManager.AddToRoleAsync(user, "Admin");

            _context.SaveChanges();
        }
    }
}
