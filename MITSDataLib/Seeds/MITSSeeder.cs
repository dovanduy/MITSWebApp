using MITSDataLib.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MITSDataLib.Seeds
{
    public class MITSSeeder
    {

        private readonly MITSContext _context;

        public MITSSeeder(MITSContext context) {
            _context = context;
        }

        public void Seed() {
            _context.Database.EnsureCreated();

            if (!_context.Persons.Any())
            {
                //Need to create sample data

            }
        }
    }
}
