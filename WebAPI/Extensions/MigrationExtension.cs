using EventZone.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventZone.WebAPI.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app, ILogger _logger)
        {
            try
            {
                using IServiceScope scope = app.ApplicationServices.CreateScope();

                using StudentEventForumDbContext dbContext =
                    scope.ServiceProvider.GetRequiredService<StudentEventForumDbContext>();

                dbContext.Database.Migrate();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An problem occurred during migration!");
            }
        }
    }
}
