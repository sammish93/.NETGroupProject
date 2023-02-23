using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data
{
    public class UserAccountContext : DbContext
    {
        public UserAccountContext(DbContextOptions<UserAccountContext> dbContextOptions) : base(dbContextOptions) { }


        public DbSet<V1User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<V1User>().HasData(
                new
                {
                    Id = Guid.NewGuid(),
                    UserName = "JinkxMonsoon",
                    Email = "joojoo@gmail.com",
                    Password = "Itismonsoonseason!",
                    FirstName = "Jinkx",
                    LastName = "Monsoon",
                    Country = "USA",
                    City = "Seattle",
                    LangPreference = "en",
                    Role = UserRole.User,
                    RegistrationDate = DateTime.Now,
                    LastActive= DateTime.Now,
                },
                 new
                 {
                     Id = Guid.NewGuid(),
                     UserName = "QueenOfTheNorth",
                     Email = "b_hyteso@gmail.com",
                     Password = "CanadianTurkeyBacon",
                     FirstName = "Brooklyn",
                     LastName = "Hytes",
                     Country = "Canada",
                     City = "Toronto",
                     LangPreference = "en",
                     Role = UserRole.User,
                     RegistrationDate = DateTime.Now,
                     LastActive = DateTime.Now,
                 },
                  new
                  {
                      Id = Guid.NewGuid(),
                      UserName = "ClownBeauty",
                      Email = "bdelrio@yahoo.com",
                      Password = "Baloney2123",
                      FirstName = "Bianca",
                      LastName = "Del Rio",
                      Country = "USA",
                      City = "Palm Springs",
                      LangPreference = "en",
                      Role = UserRole.Admin,
                      RegistrationDate = DateTime.Now,
                      LastActive = DateTime.Now,
                  });
        }
    }
}
