using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes;
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

        public DbSet<DbOrmTestClass> Tests { get; set; } 
    }
}
