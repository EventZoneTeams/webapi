using Microsoft.AspNetCore.Identity;
using Repositories.Entities;

namespace Repositories
{
    public static class DBInitializer
    {
        public static async Task Initialize(StudentEventForumDbContext context, UserManager<User> userManager)
        {
            //context.Database.EnsureCreated();

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
                };
                //Add roles
                await userManager.CreateAsync(admin, "123456");
                await userManager.AddToRoleAsync(admin, "Admin");

                var manager = new User
                {
                    UserName = "manager",
                    Email = "manager@gmail.com"
                };
                //Add roles
                await userManager.CreateAsync(manager, "123456");
                await userManager.AddToRoleAsync(manager, "Manager");

                var student = new User
                {
                    UserName = "student",
                    Email = "student@gmail.com"
                };
                //Add roles
                await userManager.CreateAsync(student, "123456");
                await userManager.AddToRoleAsync(student, "Student");

                var uydev = new User
                {
                    UserName = "uydev",
                    Email = "lequocuy@gmail.com",
                    FullName = "Le Quoc Uy",
                    University = "FPTU HCM",
                    PointBalance = 1000,
                    Dob = new DateTime(2003, 7, 11),
                    Gender = true,
                    Image = "https://scontent.fsgn15-1.fna.fbcdn.net/v/t39.30808-1/430878538_2206677789683723_4464660377243750146_n.jpg?stp=dst-jpg_p200x200&_nc_cat=106&ccb=1-7&_nc_sid=5f2048&_nc_eui2=AeE_Vr1x6BHZ_S__ovdDg7zS5W9udhABzaHlb252EAHNoS38q_urtNeTErRYpa0zqYNo-vOAf49-zjjLBslYOw-p&_nc_ohc=8En2AdNVtaUQ7kNvgEn1g25&_nc_ht=scontent.fsgn15-1.fna&oh=00_AYA_Dyr3Kzs4J5lFKCiaYlu6-KlRK4icdur4m-IrU68PPA&oe=664E1D9B"
                };
                //Add roles
                await userManager.CreateAsync(uydev, "123456");
                await userManager.AddToRoleAsync(uydev, "Student");

                var namthhse172294 = new User
                {
                    UserName = "namthhse172294",
                    Email = "namthhse172294@fpt.edu.vn",
                    FullName = "Trương Hà Hào Nam",
                    University = "FPTU HCM",
                    PointBalance = 1000,
                    Dob = new DateTime(2003, 1, 1), // Replace with the actual date of birth
                    Gender = true, // Assuming true means male
                    Image = "https://avatar.iran.liara.run/public/boy?username=namthhse172294" // Replace with the actual image URL
                };

                await userManager.CreateAsync(namthhse172294, "123456"); // Replace "password" with the actual password
                await userManager.AddToRoleAsync(namthhse172294, "Student");

                var vunse172437 = new User
                {
                    UserName = "vunse172437",
                    Email = "vunse172437@fpt.edu.vn",
                    FullName = "Nguyễn Vũ",
                    University = "FPTU HCM",
                    PointBalance = 1000,
                    Dob = new DateTime(2003, 2, 15), // Replace with the actual date of birth
                    Gender = true, // Assuming true means male
                    Image = "https://avatar.iran.liara.run/public/boy?username=vunse172437" // Replace with the actual image URL
                };

                await userManager.CreateAsync(vunse172437, "123456"); // Replace "password" with the actual password
                await userManager.AddToRoleAsync(vunse172437, "Student");

                var huanngse171018 = new User
                {
                    UserName = "huanngse171018",
                    Email = "huanngse171018@fpt.edu.vn",
                    FullName = "Ngô Gia Huấn",
                    University = "FPTU HCM",
                    PointBalance = 1000,
                    Dob = new DateTime(2003, 3, 20), // Replace with the actual date of birth
                    Gender = true, // Assuming true means male
                    Image = "https://avatar.iran.liara.run/public/boy?username=huanngse171018" // Replace with the actual image URL
                };

                await userManager.CreateAsync(huanngse171018, "123456"); // Replace "password" with the actual password
                await userManager.AddToRoleAsync(huanngse171018, "Student");

                var tienhmse172436 = new User
                {
                    UserName = "tienhmse172436",
                    Email = "tienhmse172436@fpt.edu.vn",
                    FullName = "Hoàng Minh Tiến Lmao",
                    University = "FPTU HCM",
                    PointBalance = 1000,
                    Dob = new DateTime(2003, 4, 5), // Replace with the actual date of birth
                    Gender = true, // Assuming true means male
                    Image = "https://avatar.iran.liara.run/public/boy?username=tienhmse172436" // Replace with the actual image URL
                };

                await userManager.CreateAsync(tienhmse172436, "123456"); // Replace "password" with the actual password
                await userManager.AddToRoleAsync(tienhmse172436, "Student");

                await context.SaveChangesAsync();
            }


            if (!context.Events.Any())
            {
                var events = new List<Event>
                {
                    new Event
                    {
                        Name = "Event 1",
                        Description = "Description 1",
                        EventStartDate = new DateTime(2022, 1, 1),
                        EventEndDate = new DateTime(2022, 1, 2),
                        Location = "Location 1",
                        UserId = 1,
                        Status = "0",
                        DonationStatus = "0"
                    }
                };

                foreach (var eventItem in events)
                {
                    await context.Events.AddAsync(eventItem);
                }
                await context.SaveChangesAsync();
            }

            if (!context.EventProducts.Any())
            {
                var eventProducts = new List<EventProduct>
                {
                    new EventProduct
                    {
                        Name = "Product 1",
                        Description = "Description 1",
                        Price = 1000,
                        EventId = 1
                    }
                };

                foreach (var eventProduct in eventProducts)
                {
                    await context.EventProducts.AddAsync(eventProduct);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}