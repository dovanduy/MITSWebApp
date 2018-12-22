using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Contexts;
using MITSDataLib.Models;

namespace MITSBusinessLib.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MITSContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(MITSContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<IdentityRole>> GetUserRolesByUserIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);

            return roles.Select(role => new IdentityRole(role)).ToList();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
    }
}
