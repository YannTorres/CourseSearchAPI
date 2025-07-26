using CourseSearch.Infrastructure.DataAcess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CourseSearch.Infrastructure.Migrations;
public static class DatabaseMigration
{
    public static async Task MigrateDataBase(IServiceProvider serviceProvider)
    {
        var dbcontext = serviceProvider.GetRequiredService<CourseSearchDbContext>();

        await dbcontext.Database.MigrateAsync();
    }
}
