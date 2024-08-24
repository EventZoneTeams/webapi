using EventZone.Domain.Entities;
using EventZone.Repositories.Utils;
using Microsoft.AspNetCore.Identity;

namespace EventZone.Repositories
{
    public static class DBInitializer
    {
        public static async Task Initialize(StudentEventForumDbContext context, UserManager<User> userManager)
        {
            Random random = new Random();

            if (!context.EventCategories.Any())
            {
                var eventCategories = new List<EventCategory>()
            {
                              new EventCategory
                              {
                                Title = "Education",
                                ImageUrl = "https://www.timeshighereducation.com/student/sites/default/files/istock-499343530.jpg"
                              },
                              new EventCategory
                              {
                                Title = "Music",
                                ImageUrl = "https://artsreview.b-cdn.net/wp-content/uploads/2022/06/A-crowd-at-a-music-concert.jpg"
                              },
                              new EventCategory
                              {
                                Title = "Sports",
                                ImageUrl = "https://www.coe.int/documents/24916852/0/Supporters3.jpg/63b405d6-be6d-d2ec-bd11-0f03c6ca8130?t=1503560660000"
                              },
                              new EventCategory
                              {
                                Title = "Technology",
                                ImageUrl = "https://blog.bishopmccann.com/hubfs/event%20tech%20main_%20AdobeStock_352613171.jpeg#keepProtocol"
                              },
                              new EventCategory
                              {
                                Title = "Business",
                                ImageUrl = "https://www.loghicconnect.com.au/wp-content/uploads/2020/05/Untitled-design-2023-03-16T112342.403.jpg"
                              },
                              new EventCategory
                              {
                                Title = "Art",
                                ImageUrl = "https://freeyorkk.b-cdn.net/wp-content/uploads/2021/06/AdobeStock_380232446-2400x1233.jpeg"
                              },
                              new EventCategory
                              {
                                Title = "Food & Drink",
                                ImageUrl = "https://goeshow.com/wp-content/uploads/2022/11/Blog-Image-10112022-700x423.jpg"
                              },
                              new EventCategory
                              {
                                Title = "Travel",
                                ImageUrl = "https://au.travelctm.com/wp-content/uploads/2019/10/Double-Image-Incentive-600x438.jpg"
                              },
                              new  EventCategory
                              {
                                Title = "Film",
                                ImageUrl = "https://images.lifestyleasia.com/wp-content/uploads/sites/7/2023/01/23162628/shutterstock_95844517.jpg"
                              },
                              new EventCategory
                              {
                                Title = "Other",
                                ImageUrl = "https://static.ra.co/images/promoter/za-theotherevents.jpg?dateUpdated=1576847763880s"
                              },
                            };

                foreach (var eventCategory in eventCategories)
                {
                    await context.EventCategories.AddAsync(eventCategory);
                }
                await context.SaveChangesAsync();
            }

            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                            {
                                new Role { Name = "Admin", NormalizedName = "ADMIN" },
                                new Role { Name = "Manager", NormalizedName = "MANAGER" },
                                new Role { Name = "Student", NormalizedName = "STUDENT" }
                            };

                foreach (var role in roles)
                {
                    await context.Roles.AddAsync(role);
                }

                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    FullName = "admin",
                    Dob = DateTime.Now,
                    PhoneNumber = "0123456789",
                    Gender = false,
                    University = "FPT"
                };
                //Add roles
                await userManager.CreateAsync(admin, "123456");
                await userManager.AddToRoleAsync(admin, "Admin");

                var manager = new User
                {
                    UserName = "manager",
                    Email = "manager@gmail.com",
                    FullName = "Quan ly",
                    Dob = DateTime.Now,
                    PhoneNumber = "0123456789",
                    Gender = false,
                    University = "FPT"
                };
                //Add roles
                await userManager.CreateAsync(manager, "123456");
                await userManager.AddToRoleAsync(manager, "Manager");

                var student = new User
                {
                    UserName = "student",
                    Email = "student@gmail.com",
                    FullName = "Hoang Tien",
                    Dob = DateTime.Now,
                    PhoneNumber = "0123456789",
                    Gender = false,
                    University = "FPT"
                };
                //Add roles
                await userManager.CreateAsync(student, "123456");
                await userManager.AddToRoleAsync(student, "Student");

                var uydev = new User
                {
                    UserName = "uydev",
                    Email = "lequocuy@gmail.com",
                    FullName = "Lê Quốc Uy",
                    UnsignFullName = "Le Quoc Uy",
                    University = "FPTU HCM",
                    Dob = new DateTime(2003, 7, 11),
                    Gender = true,
                    ImageUrl = "https://scontent.fsgn15-1.fna.fbcdn.net/v/t39.30808-1/430878538_2206677789683723_4464660377243750146_n.jpg?stp=dst-jpg_p200x200&_nc_cat=106&ccb=1-7&_nc_sid=5f2048&_nc_eui2=AeE_Vr1x6BHZ_S__ovdDg7zS5W9udhABzaHlb252EAHNoS38q_urtNeTErRYpa0zqYNo-vOAf49-zjjLBslYOw-p&_nc_ohc=8En2AdNVtaUQ7kNvgEn1g25&_nc_ht=scontent.fsgn15-1.fna&oh=00_AYA_Dyr3Kzs4J5lFKCiaYlu6-KlRK4icdur4m-IrU68PPA&oe=664E1D9B"
                };
                //Add roles
                await userManager.CreateAsync(uydev, "123456");
                await userManager.AddToRoleAsync(uydev, "Student");

                var namthhse172294 = new User
                {
                    UserName = "namthhse172294",
                    Email = "namthhse172294@fpt.edu.vn",
                    FullName = "Trương Hà Hào Nam",
                    UnsignFullName = StringTools.ConvertToUnSign("Trương Hà Hào Nam"),
                    University = "FPTU HCM",
                    Dob = new DateTime(2003, 1, 1), // Replace with the actual date of birth
                    Gender = true, // Assuming true means male
                    ImageUrl = "https://avatar.iran.liara.run/public/boy?username=namthhse172294" // Replace with the actual ImageUrl URL
                };

                await userManager.CreateAsync(namthhse172294, "123456"); // Replace "password" with the actual password
                await userManager.AddToRoleAsync(namthhse172294, "Student");

                var vunse172437 = new User
                {
                    UserName = "vunse172437",
                    Email = "vunse172437@fpt.edu.vn",
                    FullName = "Nguyễn Vũ",
                    University = "FPTU HCM",
                    Dob = new DateTime(2003, 2, 15), // Replace with the actual date of birth
                    Gender = true, // Assuming true means male
                    ImageUrl = "https://avatar.iran.liara.run/public/boy?username=vunse172437" // Replace with the actual ImageUrl URL
                };

                await userManager.CreateAsync(vunse172437, "123456"); // Replace "password" with the actual password
                await userManager.AddToRoleAsync(vunse172437, "Student");

                var huanngse171018 = new User
                {
                    UserName = "huanngse171018",
                    Email = "huanngse171018@fpt.edu.vn",
                    FullName = "Ngô Gia Huấn",
                    UnsignFullName = StringTools.ConvertToUnSign("Ngô Gia Huấn"),
                    University = "FPTU HCM",
                    Dob = new DateTime(2003, 3, 20), // Replace with the actual date of birth
                    Gender = true, // Assuming true means male
                    ImageUrl = "https://avatar.iran.liara.run/public/boy?username=huanngse171018" // Replace with the actual ImageUrl URL
                };

                await userManager.CreateAsync(huanngse171018, "123456"); // Replace "password" with the actual password
                await userManager.AddToRoleAsync(huanngse171018, "Student");

                var tienhmse172436 = new User
                {
                    UserName = "tienhmse172436",
                    Email = "tienhmse172436@fpt.edu.vn",
                    FullName = "Hoàng Minh Tiến Lmao",
                    UnsignFullName = StringTools.ConvertToUnSign("Hoàng Minh Tiến"),
                    University = "FPTU HCM",
                    Dob = new DateTime(2003, 4, 5), // Replace with the actual date of birth
                    Gender = true, // Assuming true means male
                    ImageUrl = "https://avatar.iran.liara.run/public/boy?username=tienhmse172436" // Replace with the actual image URL
                };

                await userManager.CreateAsync(tienhmse172436, "123456"); // Replace "password" with the actual password
                await userManager.AddToRoleAsync(tienhmse172436, "Student");

                await context.SaveChangesAsync();
            }

            // Ensure users exist before seeding dependent data
            var firstUser = context.Users.FirstOrDefault();
            if (firstUser == null) return;

            if (!context.Events.Any())
            {
                var categories = context.EventCategories.ToList();

                if (categories.Any())
                {
                    var events = new List<Event>
                    {
                        new Event
                        {
                            Name = "Tech Conference 2024",
                            Description = "Explore the latest in technology and innovation.",
                            ThumbnailUrl = "https://picsum.photos/500/300/?image=10",
                            EventStartDate = DateTime.Now.AddDays(10),
                            EventEndDate = DateTime.Now.AddDays(11),
                            Latitude = "10.762622",
                            Longitude = "106.660172",
                            LocationDisplay = "Ho Chi Minh City, Vietnam",
                            LocationNote = "XYZ Convention Center",
                            Note = "Please bring your ID for check-in.",
                            UserId = firstUser.Id,
                            EventCategoryId = categories.FirstOrDefault(c => c.Title == "Education")?.Id ?? Guid.Empty,
                            Status = "Upcoming",
                            CreatedAt = DateTime.Now,
                        },
                        new Event
                        {
                            Name = "Acoustic Night",
                            Description = "Enjoy a night of acoustic performances.",
                            ThumbnailUrl = "https://picsum.photos/500/300/?image=20",
                            EventStartDate = DateTime.Now.AddDays(20),
                            EventEndDate = DateTime.Now.AddDays(20),
                            Latitude = "21.028511",
                            Longitude = "105.804817",
                            LocationDisplay = "Hanoi, Vietnam",
                            LocationNote = "Cafe ABC",
                            Note = "Limited seats available, register early.",
                            UserId = firstUser.Id,
                            EventCategoryId = categories.FirstOrDefault(c => c.Title == "Music")?.Id ?? Guid.Empty,
                            Status = "Upcoming",
                            CreatedAt = DateTime.Now,
                        }
                    };

                    await context.Events.AddRangeAsync(events);
                    await context.SaveChangesAsync();
                }
            }

            // Seed EventBoards
            var firstEvent = context.Events.FirstOrDefault();
            if (firstEvent == null || firstUser == null) return;

            if (!context.EventBoards.Any())
            {
                var eventBoards = new List<EventBoard>
                {
                    new EventBoard
                    {
                        EventId = firstEvent.Id,
                        Name = "Tech Board",
                        Description = "Board for managing tech events",
                        Priority = "High",
                        CreatedAt = DateTime.Now,
                        LeaderId = firstUser.Id,
                    },
                    new EventBoard
                    {
                        EventId = firstEvent.Id,
                        Name = "Music Board",
                        Description = "Board for managing music events",
                        Priority = "Medium",
                        CreatedAt = DateTime.Now,
                        LeaderId = firstUser.Id,
                    }
                };

                await context.EventBoards.AddRangeAsync(eventBoards);
                await context.SaveChangesAsync();
            }

            // Ensure EventBoards exist before seeding dependent data
            if (!context.EventBoards.Any()) return;

            // Continue with seeding EventBoardColumns, EventBoardLabels, etc., using the same safety checks.
            // Ensure EventBoards exist before seeding dependent data
            var firstEventBoard = context.EventBoards.FirstOrDefault();
            if (firstEventBoard == null) return;

            // Seed EventBoardColumns
            if (!context.EventBoardColumns.Any())
            {
                var eventBoardColumns = new List<EventBoardColumn>
                {
                    new EventBoardColumn
                    {
                        EventBoardId = firstEventBoard.Id,
                        Name = "To Do",
                        Color = "#FF5733",
                        CreatedAt = DateTime.Now
                    },
                    new EventBoardColumn
                    {
                        EventBoardId = firstEventBoard.Id,
                        Name = "In Progress",
                        Color = "#33FF57",
                        CreatedAt = DateTime.Now
                    },
                    new EventBoardColumn
                    {
                        EventBoardId = firstEventBoard.Id,
                        Name = "Done",
                        Color = "#3357FF",
                        CreatedAt = DateTime.Now
                    }
                };

                await context.EventBoardColumns.AddRangeAsync(eventBoardColumns);
                await context.SaveChangesAsync();
            }

            // Seed EventBoardLabels
            if (!context.EventBoardLabels.Any())
            {
                var eventBoardLabels = new List<EventBoardLabel>
                {
                    new EventBoardLabel
                    {
                        EventId = firstEvent.Id,
                        Name = "High Priority",
                        Color = "#FF0000",
                        CreatedAt = DateTime.Now
                    },
                    new EventBoardLabel
                    {
                        EventId = firstEvent.Id,
                        Name = "Low Priority",
                        Color = "#00FF00",
                        CreatedAt = DateTime.Now
                    }
                };

                await context.EventBoardLabels.AddRangeAsync(eventBoardLabels);
                await context.SaveChangesAsync();
            }

            // Seed EventBoardLabelAssignments
            var firstEventBoardLabel = context.EventBoardLabels.FirstOrDefault();
            if (firstEventBoardLabel == null) return;

            if (!context.EventBoardLabelAssignments.Any())
            {
                var eventBoardLabelAssignments = new List<EventBoardLabelAssignment>
                {
                    new EventBoardLabelAssignment
                    {
                        EventBoardId = firstEventBoard.Id,
                        EventBoardLabelId = firstEventBoardLabel.Id,
                    }
                };

                await context.EventBoardLabelAssignments.AddRangeAsync(eventBoardLabelAssignments);
                await context.SaveChangesAsync();
            }

            // Seed EventBoardMembers
            if (!context.EventBoardMembers.Any())
            {
                var eventBoardMembers = new List<EventBoardMember>
                {
                    new EventBoardMember
                    {
                        EventBoardId = firstEventBoard.Id,
                        UserId = firstUser.Id
                    }
                };

                await context.EventBoardMembers.AddRangeAsync(eventBoardMembers);
                await context.SaveChangesAsync();
            }

            // Seed EventBoardTasks
            var firstEventBoardColumn = context.EventBoardColumns.FirstOrDefault();
            if (firstEventBoardColumn == null) return;

            if (!context.EventBoardTasks.Any())
            {
                var eventBoardTasks = new List<EventBoardTask>
                {
                    new EventBoardTask
                    {
                        Title = "Design UI",
                        Description = "Design the user interface for the event management system.",
                        EventBoardColumnId = firstEventBoardColumn.Id,
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedAt = DateTime.Now,
                    },
                    new EventBoardTask
                    {
                        Title = "Setup Backend",
                        Description = "Setup the backend architecture for the event management system.",
                        EventBoardColumnId = firstEventBoardColumn.Id,
                        DueDate = DateTime.Now.AddDays(10),
                        CreatedAt = DateTime.Now,
                    }
                };

                await context.EventBoardTasks.AddRangeAsync(eventBoardTasks);
                await context.SaveChangesAsync();
            }

            // Seed EventBoardTaskAssignments
            var firstEventBoardTask = context.EventBoardTasks.FirstOrDefault();
            if (firstEventBoardTask == null) return;

            if (!context.EventBoardTaskAssignments.Any())
            {
                var eventBoardTaskAssignments = new List<EventBoardTaskAssignment>
                {
                    new EventBoardTaskAssignment
                    {
                        EventBoardTaskId = firstEventBoardTask.Id,
                        UserId = firstUser.Id
                    }
                };

                await context.EventBoardTaskAssignments.AddRangeAsync(eventBoardTaskAssignments);
                await context.SaveChangesAsync();
            }

            // Seed EventBoardTaskLabels
            if (!context.EventBoardTaskLabels.Any())
            {
                var eventBoardTaskLabels = new List<EventBoardTaskLabel>
                {
                    new EventBoardTaskLabel
                    {
                        Name = "Urgent",
                        Color = "#FF0000",
                        EventBoardId = firstEventBoard.Id,
                        CreatedAt = DateTime.Now,
                    },
                    new EventBoardTaskLabel
                    {
                        Name = "Optional",
                        Color = "#00FF00",
                        EventBoardId = firstEventBoard.Id,
                        CreatedAt = DateTime.Now,
                    }
                };

                await context.EventBoardTaskLabels.AddRangeAsync(eventBoardTaskLabels);
                await context.SaveChangesAsync();
            }

            // Seed EventBoardTaskLabelAssignments
            var firstEventBoardTaskLabel = context.EventBoardTaskLabels.FirstOrDefault();
            if (firstEventBoardTaskLabel == null) return;

            if (!context.EventBoardTaskLabelAssignments.Any())
            {
                var eventBoardTaskLabelAssignments = new List<EventBoardTaskLabelAssignment>
                {
                    new EventBoardTaskLabelAssignment
                    {
                        EventBoardTaskId = firstEventBoardTask.Id,
                        EventBoardTaskLabelId = firstEventBoardTaskLabel.Id
                    }
                };

                await context.EventBoardTaskLabelAssignments.AddRangeAsync(eventBoardTaskLabelAssignments);
                await context.SaveChangesAsync();
            }
        }
    }
}