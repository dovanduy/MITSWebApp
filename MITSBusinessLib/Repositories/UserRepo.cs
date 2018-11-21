using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Contexts;
using MITSDataLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MITSBusinessLib.Repositories
{
    public class UserRepo : IUserRepo
    {

        private readonly MITSContext _context;

        public UserRepo(MITSContext context)
        {
            _context = context;
        }


        public IEnumerable<Person> GetUsers()
        {
            return _context.Persons;
        }
    }
}
