using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UserRepo> _logger;

        public UserRepo(MITSContext context, ILogger<UserRepo> logger )
        {
            _context = context;
            _logger = logger;
        }

        public void AddEntity(object model)
        {
            _context.Add(model);
        }

        public Person GetUserById(int id)
        {
            return _context.Persons.Where(person => person.Id == id).FirstOrDefault();
        }

        [Authorize]
        public IEnumerable<Person> GetUsers()
        {
            try
            {
                _logger.LogInformation("get All products was called");
                return _context.Persons.ToList();
            }

            catch (Exception ex) {
                _logger.LogError($"Failed to get all users: {ex}");
                return null;
            }
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
