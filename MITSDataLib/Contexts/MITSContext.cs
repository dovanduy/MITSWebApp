using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MITSDataLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Contexts
{
    public class MITSContext : DbContext
    {

        public MITSContext(DbContextOptions<MITSContext> options): base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }

       
    }
}
