using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MITSDataLib.Models;
using MITSBusinessLib.Repositories;
using MITSBusinessLib.Repositories.Interfaces;

namespace MITSWebServices.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SampleDataController : Controller
    {
        private readonly IUserRepo _userRepo;

        public SampleDataController(IUserRepo userRepo) {
            _userRepo = userRepo;
        }

        
        [HttpGet]
        public IEnumerable<Person> GetUsers()
        {
            return _userRepo.GetUsers();
        }
    }
}
