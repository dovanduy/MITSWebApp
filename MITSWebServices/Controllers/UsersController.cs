using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MITSBusinessLib.Repositories;
using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MITSWebServices.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    [Authorize(Policy = "Admin")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : Controller
    {
        private readonly IUserRepo _userRepo;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepo userRepo, ILogger<UsersController> logger)
        {
            _userRepo = userRepo;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_userRepo.GetUsers());
            }

            catch (Exception ex)
            {
                _logger.LogError($"Failed to get products: {ex}");
                return BadRequest("Failed to get products");
            }


        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var user = _userRepo.GetUserById(id);

                if (user != null) return Ok(user);
                else return NotFound();
            }

            catch (Exception ex)
            {
                _logger.LogError($"Failed to get products: {ex}");
                return BadRequest("Failed to get products");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Person person)
        {

            try
            {
                _userRepo.AddEntity(person);


                if (_userRepo.SaveAll())
                {
                    return Created($"/api/orders{person.Id}", person);
                }             
            }

            catch (Exception ex)
            {
                
                _logger.LogError($"Failed to save person: {ex}");
                
            }

            return BadRequest("Failed to save new person");

        }
    }
}
