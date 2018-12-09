using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MITSDataLib.Models;

namespace MITSDataLib.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<List<IdentityRole>> GetUserRolesByUserIdAsync(string id);
    }
}
