using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public int? ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }


    }
}
