using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Microsoft.AspNetCore.Identity;
using MITSDataLib.Repositories.Interfaces;

namespace MITSDataLib.Models.GraphQL.Types
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
