using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MITSDataLib.Models;

namespace MITSBusinessLib.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<List<IdentityRole>> GetUserRolesByUserIdAsync(string id);
    }
}
