using GraphQL.Types;
using Microsoft.AspNetCore.Identity;

namespace MITSBusinessLib.GraphQL.Types
{
    public class UserRoleType : ObjectGraphType<IdentityRole>
    {
        public UserRoleType()
        {
            Field(r => r.Name);
        }
    }
}
