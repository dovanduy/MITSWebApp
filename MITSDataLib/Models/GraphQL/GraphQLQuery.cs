using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models
{
    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public Newtonsoft.Json.Linq.JObject Variables { get; set; }
    }
}
