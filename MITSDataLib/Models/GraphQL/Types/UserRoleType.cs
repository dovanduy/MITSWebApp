using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Microsoft.AspNetCore.Identity;

namespace MITSDataLib.Models.GraphQL.Types
{
    public class UserRoleType : ObjectGraphType<IdentityRole>
    {
        public UserRoleType()
        {
            Field(r => r.Name);
        }
    }
}
