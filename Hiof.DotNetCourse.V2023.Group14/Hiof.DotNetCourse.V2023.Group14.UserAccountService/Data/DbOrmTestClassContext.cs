using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

// Apparently the convention is to have DbContext related classes in a 'Data' folder.
namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data
{
    public class DbOrmTestClassContext : DbContext
    {
        // Constructor that uses dependency injection in the TestProgram.cs file to inject the database connection string.
        public DbOrmTestClassContext(DbContextOptions<DbOrmTestClassContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<V1DbOrmTestClass> Tests { get; set; }

        // Seeding the database by inserting 5 new rows.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<V1DbOrmTestClass>().HasData(
                new V1DbOrmTestClass
                {
                    Id = 1,
                    Name = "Jonas",
                    Age = 42
                },
                new V1DbOrmTestClass
                {
                    Id = 2,
                    Name = "Dobby the House Dog",
                    Age = 1
                },
                new V1DbOrmTestClass
                {
                    Id = 3,
                    Name = "Margaret of Anjou",
                    Age = 743
                },
                new V1DbOrmTestClass
                {
                    Id = 4,
                    Name = "Sam",
                    Age = 29
                },
                new V1DbOrmTestClass
                {
                    Id = 5,
                    Name = "Squiggle",
                    Age = 64
                }
            );
        }
    }
}
