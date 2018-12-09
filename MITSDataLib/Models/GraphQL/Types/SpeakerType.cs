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
            //Field(s => s.ImageName, type: typeof (GuidGraphType));
        }
    }
}
