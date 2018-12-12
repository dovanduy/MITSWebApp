using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models.GraphQL.Types
{
    public class SpeakerType : ObjectGraphType<Speaker>
    {
        public SpeakerType()
        {
            Field(s => s.Id);
            Field(s => s.FirstName);
            Field(s => s.LastName);
            Field(s => s.Bio);
            Field(s => s.IsPanelist);
            Field(s => s.Title);
            //Field<GuidGraphType>("imageName", resolve: context => context.Source.ImageName);
            //Remember to mutate back to a guid type to store in the database.
            //Field(s => s.ImageName, type: typeof (StringGraphType));
        }
    }
}
