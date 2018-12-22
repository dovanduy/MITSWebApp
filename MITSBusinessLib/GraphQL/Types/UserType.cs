using System.Collections.Generic;
using GraphQL.Types;
using Microsoft.AspNetCore.Identity;
using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Models;

namespace MITSBusinessLib.GraphQL.Types
{
    public class UserType : ObjectGraphType<User>
    {
        public UserType(IUserRepository userRepo)
        {
            Field(u => u.Email);
            Field(u => u.FirstName);
            Field(u => u.LastName);
            Field(u => u.UserName);
            Field(u => u.Id);
            Field<ListGraphType<UserRoleType>, List<IdentityRole>>()
                .Name("roles")
                .ResolveAsync(context => userRepo.GetUserRolesByUserIdAsync(context.Source.Id));

        }
    }
}
