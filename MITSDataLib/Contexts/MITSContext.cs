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

        public MITSContext(DbContextOptions<MITSContext> options) : base(options)
        {
            //options.
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SectionSpeaker>().HasKey(key => new { key.SectionId, key.SpeakerId });
            modelBuilder.Entity<SectionSpeaker>()
                .HasOne(ss => ss.Section)
                .WithMany(section => section.SectionSpeakers)
                .HasForeignKey(ss => ss.SectionId);

            modelBuilder.Entity<SectionSpeaker>()
                .HasOne(ss => ss.Speaker)
                .WithMany(speaker => speaker.SpeakerSections)
                .HasForeignKey(ss => ss.SpeakerId);

            modelBuilder.Entity<SectionTag>().HasKey(key => new { key.SectionId, key.TagId });
            modelBuilder.Entity<SectionTag>()
                .HasOne(st => st.Section)
                .WithMany(section => section.SectionTags)
                .HasForeignKey(ss => ss.SectionId);
            modelBuilder.Entity<SectionTag>()
                .HasOne(st => st.Tag)
                .WithMany(tag => tag.TagSections)
                .HasForeignKey(st => st.TagId);

       
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<WildApricotToken> WaTokens { get; set; }
        public DbSet<WildApricotEvent> WaEvents { get; set; }
        public DbSet<WildApricotRegistrationType> WaRegistrations { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<SectionSpeaker> SectionsSpeakers { get; set; }
        public DbSet<SectionTag> SectionsTags { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Day> Days { get; set; }

        
    }
}
