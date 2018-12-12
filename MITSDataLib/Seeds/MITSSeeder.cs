using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MITSDataLib.Contexts;
using MITSDataLib.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITSDataLib.Seeds
{
    public class MITSSeeder
    {

        private readonly MITSContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public MITSSeeder(MITSContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            #region User

            var role = await _roleManager.RoleExistsAsync("Admin");
         
            if (!role)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole("Admin"));

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Could not create admin role");
                }
            }

            User user = await _userManager.FindByEmailAsync("enderjs@gmail.com");

            // Seed the Main User

            if (user == null)
            {
                user = new User()
                {
                    LastName = "Silvers",
                    FirstName = "Jason",
                    Email = "enderjs@gmail.com",
                    UserName = "enderjs@gmail.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                       
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create user in Seeding");
                }

                
            }

            await _userManager.AddToRoleAsync(user, "Admin");

            #endregion

            _context.SaveChanges();

            #region Day

            var days = new List<Day>();

            if (!_context.Days.Any())
            {
                days = new List<Day>
                {
                    new Day { AgendaDay = new DateTime(2018, 10, 6, 00, 00, 00) },
                    new Day { AgendaDay = new DateTime(2018, 10, 7, 00, 00, 00) }
                };

                _context.Days.AddRange(days);
                _context.SaveChanges();
            }

            #endregion

            #region Section

            var day1Sections = new List<Section>();
            var day2Sections = new List<Section>();
            var allSections = new List<Section>();
                      

            if (!_context.Sections.Any())
            {
                var day1 = days.First();
                var day2 = days.Last();

                var section1 = new List<Section>
                {
                    new Section {
                        DayId = day1.Id,
                        Name = "Keynote",
                        Description = "Start of the conference",
                        SlideUrl = "http://dropbox.com/slide1",
                        RestrictSlide = true,
                        IsPanel = false,
                        StartDate = new DateTime(2018, 10, 6, 7, 0, 0),
                        EndDate = new DateTime(2018, 10, 6, 8, 0, 0)

                    },
                    new Section {
                        DayId = day1.Id,
                        Name = "Codelabs",
                        Description = "Codelabs building",
                        SlideUrl = "http://dropbox.com/slide2",
                        RestrictSlide = true,
                        IsPanel = false,
                        StartDate = new DateTime(2018, 10, 6, 8, 0, 0),
                        EndDate = new DateTime(2018, 10, 6, 9, 0, 0)

                    },
                    new Section {
                        DayId = day1.Id,
                        Name = "Angular Router",
                        Description = "We will discuss the angular router ",
                        SlideUrl = "http://dropbox.com/slide3",
                        RestrictSlide = true,
                        IsPanel = false,
                        StartDate = new DateTime(2018, 10, 6, 9, 0, 0),
                        EndDate = new DateTime(2018, 10, 6, 10, 0, 0)

                    },
                    new Section {
                        DayId = day1.Id,
                        Name = "Angular Component",
                        Description = "We will discuss angular component",
                        SlideUrl = "http://dropbox.com/slide4",
                        RestrictSlide = true,
                        IsPanel = false,
                        StartDate = new DateTime(2018, 10, 6, 10, 0, 0),
                        EndDate = new DateTime(2018, 10, 6, 11, 0, 0)

                    },
                    new Section {
                        DayId = day1.Id,
                        Name = "Lunch",
                        Description = "Eat a good lunch",
                        SlideUrl = "",
                        RestrictSlide = false,
                        IsPanel = false,
                        StartDate = new DateTime(2018, 10, 6, 11, 0, 0),
                        EndDate = new DateTime(2018, 10, 6, 12, 0, 0)

                    },
                    new Section {
                        DayId = day1.Id,
                        Name = "Angular Pipe",
                        Description = "We will discuss angular pipes",
                        SlideUrl = "http://dropbox.com/slide5",
                        RestrictSlide = true,
                        IsPanel = false,
                        StartDate = new DateTime(2018, 10, 6, 12, 0, 0),
                        EndDate = new DateTime(2018, 10, 6, 13, 0, 0)

                    },
                    new Section {
                        DayId = day1.Id,
                        Name = "Angular AOT",
                        Description = "We will discuss angular AOT",
                        SlideUrl = "http://dropbox.com/slid6",
                        RestrictSlide = true,
                        IsPanel = false,
                        StartDate = new DateTime(2018, 10, 6, 13, 0, 0),
                        EndDate = new DateTime(2018, 10, 6, 14, 0, 0)

                    }
             
                };

                var section2 = new List<Section>
                {
                    new Section {
                        DayId = day2.Id,
                        Name = "Keynote",
                        Description = "Start of the conference",
                        SlideUrl = "http://dropbox.com/slide1",
                        RestrictSlide = true,
                        IsPanel = false,
                        StartDate = new DateTime(2018, 10, 7, 7, 0, 0),
                        EndDate = new DateTime(2018, 10, 7, 8, 0, 0)

                    },
                    new Section {
                        DayId = day2.Id,
                        Name = "Codelabs",
                        Description = "Codelabs building",
                        SlideUrl = "http://dropbox.com/slide2",
                        RestrictSlide = true,
                        IsPanel = false,
                        StartDate = new DateTime(2018, 10, 7, 8, 0, 0),
                        EndDate = new DateTime(2018, 10, 7, 9, 0, 0)

                    },
                    new Section {
                        DayId = day2.Id,
                        Name = "Angular Router",
                        Description = "We will discuss the angular router ",
                        SlideUrl = "http://dropbox.com/slide3",
                        RestrictSlide = true,
                        IsPanel = false,
                        StartDate = new DateTime(2018, 10, 7, 9, 0, 0),
                        EndDate = new DateTime(2018, 10, 7, 10, 0, 0)

                    },
                    new Section {
                        DayId = day2.Id,
                        Name = "Angular Component",
                        Description = "We will discuss angular component",
                        SlideUrl = "http://dropbox.com/slide4",
                        RestrictSlide = true,
                        IsPanel = false,
                        StartDate = new DateTime(2018, 10, 7, 10, 0, 0),
                        EndDate = new DateTime(2018, 10, 7, 11, 0, 0)

                    },
                    new Section {
                        DayId = day2.Id,
                        Name = "Lunch",
                        Description = "Eat a good lunch",
                        SlideUrl = "",
                        RestrictSlide = false,
                        IsPanel = false,
                        StartDate = new DateTime(2018, 10, 7, 11, 0, 0),
                        EndDate = new DateTime(2018, 10, 7, 12, 0, 0)

                    },
                    new Section {
                        DayId = day2.Id,
                        Name = "Angular Pipe",
                        Description = "We will discuss angular pipes",
                        SlideUrl = "http://dropbox.com/slide5",
                        RestrictSlide = true,
                        IsPanel = false,
                        StartDate = new DateTime(2018, 10, 7, 12, 0, 0),
                        EndDate = new DateTime(2018, 10, 7, 13, 0, 0)

                    },
                    new Section {
                        DayId = day2.Id,
                        Name = "Angular AOT",
                        Description = "We will discuss angular AOT",
                        SlideUrl = "http://dropbox.com/slid6",
                        RestrictSlide = true,
                        IsPanel = false,
                        StartDate = new DateTime(2018, 10, 7, 13, 0, 0),
                        EndDate = new DateTime(2018, 10, 7, 14, 0, 0)

                    }

                };
              
                day1Sections.AddRange(section1);
                day2Sections.AddRange(section2);
                allSections.AddRange(day1Sections);
                allSections.AddRange(day2Sections);

                _context.Sections.AddRange(day1Sections);
                _context.Sections.AddRange(day2Sections);
                _context.SaveChanges();
            }

            #endregion

            #region Speaker

            var speakers = new List<Speaker>();

            if (!_context.Speakers.Any())
            {
                var newSpeakers = new List<Speaker>
                {
                    new Speaker
                    {
                        FirstName = "Bob",
                        LastName = "Anderson",
                        Bio = "I have worked on anguler stuff for exactly one week",
                        Title = "VP Boeing",
                        IsPanelist = false
                    },
                    new Speaker
                    {
                        FirstName = "Jane",
                        LastName = "Smith",
                        Bio = "I have worked on anguler stuff for exactly two week",
                        Title = "VP Boeing",
                        IsPanelist = false
                    },
                    new Speaker
                    {
                        FirstName = "Andrew",
                        LastName = "Shroble",
                        Bio = "I have worked on anguler stuff for exactly three week",
                        Title = "VP Boeing",
                        IsPanelist = false
                    },
                    new Speaker
                    {
                        FirstName = "Pete",
                        LastName = "Jacboson",
                        Bio = "I have worked on anguler stuff for exactly Four week",
                        Title = "VP Boeing",
                        IsPanelist = false
                    },
                    new Speaker
                    {
                        FirstName = "John",
                        LastName = "Silvers",
                        Bio = "I have worked on anguler stuff for exactly one week",
                        Title = "VP Boeing",
                        IsPanelist = false
                    },
                    new Speaker
                    {
                        FirstName = "Jennifer",
                        LastName = "Silvers",
                        Bio = "I have worked on anguler stuff for exactly one week",
                        Title = "VP Boeing",
                        IsPanelist = false
                    },
                    new Speaker
                    {
                        FirstName = "Frank",
                        LastName = "Mileto",
                        Bio = "I have worked on anguler stuff for exactly one week",
                        Title = "VP Boeing",
                        IsPanelist = false
                    },
                    new Speaker
                    {
                        FirstName = "Christoper",
                        LastName = "Logsdon",
                        Bio = "I have worked on anguler stuff for exactly one week",
                        Title = "VP Boeing",
                        IsPanelist = false
                    },
                    new Speaker
                    {
                        FirstName = "Richard",
                        LastName = "Chauvin",
                        Bio = "I have worked on anguler stuff for exactly one week",
                        Title = "VP Boeing",
                        IsPanelist = false
                    },
                    new Speaker
                    {
                        FirstName = "Brandy",
                        LastName = "Silvers",
                        Bio = "I have worked on anguler stuff for exactly one week",
                        Title = "VP Boeing",
                        IsPanelist = false
                    },
                    new Speaker
                    {
                        FirstName = "Jason",
                        LastName = "Silvers",
                        Bio = "I have worked on anguler stuff for exactly one week",
                        Title = "VP Boeing",
                        IsPanelist = false
                    },
                    new Speaker
                    {
                        FirstName = "Shane",
                        LastName = "Zondor",
                        Bio = "I have worked on anguler stuff for exactly one week",
                        Title = "VP Boeing",
                        IsPanelist = false
                    },
                    new Speaker
                    {
                        FirstName = "Markiesha",
                        LastName = "Crawford",
                        Bio = "I have worked on anguler stuff for exactly one week",
                        Title = "VP Boeing",
                        IsPanelist = false
                    },
                    new Speaker
                    {
                        FirstName = "Brian",
                        LastName = "Peters",
                        Bio = "I have worked on anguler stuff for exactly one week",
                        Title = "VP Boeing",
                        IsPanelist = false
                    },
                };

                speakers.AddRange(newSpeakers);
                _context.Speakers.AddRange(speakers);
                _context.SaveChanges();
            }


            #endregion

            var sectionSpeakers = new List<SectionSpeaker>();

            #region SpeakerSection
            allSections.ForEach(section =>
            {
                var assignedSpeaker = speakers.First();
                speakers.Remove(assignedSpeaker);
                var newSectionSpeaker = new SectionSpeaker { Section = section, Speaker = assignedSpeaker };
                sectionSpeakers.Add(newSectionSpeaker);

            });

            _context.SectionsSpeakers.AddRange(sectionSpeakers);
            _context.SaveChanges();

            #endregion

            #region Tag

            var tags = new List<Tag>();

            if (!_context.Tags.Any())
            {
                var newTags = new List<Tag>
                {
                    new Tag {Name = "Security" },
                    new Tag {Name = "Angular"},
                    new Tag {Name = "SaaS"},
                    new Tag {Name = "Cloud"},
                    new Tag {Name = "Testing"}

                };

                tags.AddRange(newTags);
                _context.Tags.AddRange(tags);
                _context.SaveChanges();
            }

            #endregion



            #region SectionTag

            if (!_context.SectionsTags.Any())
            {

                var allSectionTags = new List<SectionTag>();

                allSections.ForEach(section =>
                {
                    Random random = new Random();
                    int numOfTags = random.Next(1, 3);

                    if (numOfTags == 1)
                    {
                        var index = random.Next(1, tags.Count);
                        var newSectionTag = new SectionTag { SectionId = section.Id, TagId = tags[index].Id };

                        allSectionTags.Add(newSectionTag);
                    }

                    if (numOfTags == 2)
                    {

                        var index1 = random.Next(1, tags.Count);
                        var index2 = random.Next(1, tags.Count);

                        while (index1 == index2)
                        {
                            index2 = random.Next(1, tags.Count);
                        }


                        var newSectionTag1 = new SectionTag { SectionId = section.Id, TagId = tags[index1].Id };
                        var newSectionTag2 = new SectionTag { SectionId = section.Id, TagId = tags[index2].Id };

                        allSectionTags.Add(newSectionTag1);
                        allSectionTags.Add(newSectionTag2);
                    }



                });

                _context.SectionsTags.AddRange(allSectionTags);
                _context.SaveChanges();
            }



            #endregion


        }
    }

    
}


