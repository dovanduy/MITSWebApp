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
                    new Speaker { }
                };
            }


            #endregion



        }
    }
}
