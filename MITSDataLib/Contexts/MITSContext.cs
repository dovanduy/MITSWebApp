using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MITSDataLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Contexts
{
    public class MITSContext : IdentityDbContext<User>
    {

        public MITSContext(DbContextOptions<MITSContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SectionSpeaker>().HasKey(key => new { key.SectionId, key.SpeakerId });

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<WildApricotEvent> WaEvents { get; set; }
        public DbSet<WildApricotRegistrationType> WaRegistrationTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Day> Days { get; set; }

        
    }
}
