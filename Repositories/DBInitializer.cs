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

                await context.SaveChangesAsync();
            }
        }
    }
}