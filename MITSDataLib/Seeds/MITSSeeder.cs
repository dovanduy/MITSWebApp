using Microsoft.AspNetCore.Identity;
using MITSDataLib.Contexts;
using MITSDataLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITSDataLib.Seeds
{
    public class MITSSeeder
    {

        private readonly MITSContext _context;

        public MITSSeeder(MITSContext context ) {
            _context = context;

        }

        //public async Task SeedAsync() {
        //    _context.Database.EnsureCreated();

        //    //// Seed the Main User
            
        //    //if (user == null)
        //    //{
                
        //    //}

        //    //_context.SaveChanges();
        //}
    }
}
