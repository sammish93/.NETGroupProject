using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data
{
    public class DbOrmTestClassContext : DbContext
    {
        public DbOrmTestClassContext(DbContextOptions<DbOrmTestClassContext> dbContextOptions) : base(dbContextOptions) 
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();

                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public DbSet<DbOrmTestClass> Tests { get; set; }
    }
}
