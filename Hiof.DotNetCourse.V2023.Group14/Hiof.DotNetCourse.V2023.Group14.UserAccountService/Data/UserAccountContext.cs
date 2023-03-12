using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.DTO.V1;
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

        public DbSet<V1LoginModel> LoginModel { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<V1User>().HasData(
                new 
                {
                    Id = Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA6"),
                    UserName = "JinkxMonsoon",
                    Email = "joojoo@gmail.com",
                    // Password is "Itismonsoonseason1!"
                    Password = "70A1AF97C0496AD874DC",
                    FirstName = "Jinkx",
                    LastName = "Monsoon",
                    Country = "USA",
                    City = "Seattle",
                    LangPreference = "en",
                    Role = UserRole.User,
                    RegistrationDate = DateTime.Parse("2023-02-24 10:37:08.387"),
                    LastActive= DateTime.Parse("2023-02-24 10:37:08.387"),
                },
                 new 
                 {
                     Id = Guid.Parse("E8CC12BA-4DF6-4B06-B96E-9AD00A927A93"),
                     UserName = "QueenOfTheNorth",
                     Email = "b_hyteso@gmail.com",
                     // Password is "CanadianTurkeyBacon?43"
                     Password = "B1A8A1223DCA3A102726",
                     FirstName = "Brooklyn",
                     LastName = "Hytes",
                     Country = "Canada",
                     City = "Toronto",
                     LangPreference = "en",
                     Role = UserRole.User,
                     RegistrationDate = DateTime.Parse("2023-02-24 10:39:32.540"),
                     LastActive = DateTime.Parse("2023-02-24 10:39:32.540"),
                 },
                  new 
                  {
                      Id = Guid.Parse("54AF86BF-346A-4CBA-B36F-527748E1CB93"),
                      UserName = "testaccount",
                      Email = "testme@test.no",
                      // Password is "TestPassword123!"
                      Password = "A7E220F0781BE0C248A3",
                      FirstName = "Ola",
                      LastName = "Nordmann",
                      Country = "Norway",
                      City = "Oslo",
                      LangPreference = "no",
                      Role = UserRole.Admin,
                      RegistrationDate = DateTime.Parse("2023-02-24 10:42:49.373"),
                      LastActive = DateTime.Parse("2023-02-24 10:42:49.373"),
                  });
            modelBuilder.Entity<V1LoginModel>().HasData(
                new
                {
                    Id = Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA6"),
                    UserName = "JinkxMonsoon",
                    Password = "70A1AF97C0496AD874DC",
                    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIzZmE4NWY2NC01NzE3LTQ1NjItYjNmYy0yYzk2M2Y2NmFmYTYiLCJuYmYiOjE2NzcyMzUxMDEsImV4cCI6MTY3NzQ5NDMwMSwiaWF0IjoxNjc3MjM1MTAxfQ.LqUQyhrnWwkNtkQUYcatydTdeAqvCaZZ4tEYovAGkJI",
                    Salt = "247432D4ED93DCE32929"
                },
                new
                {
                    Id = Guid.Parse("54AF86BF-346A-4CBA-B36F-527748E1CB93"),
                    UserName = "testaccount",
                    Password = "A7E220F0781BE0C248A3",
                    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1NGFmODZiZi0zNDZhLTRjYmEtYjM2Zi01Mjc3NDhlMWNiOTMiLCJuYmYiOjE2NzcyMzU0OTQsImV4cCI6MTY3NzQ5NDY5NCwiaWF0IjoxNjc3MjM1NDk0fQ._VGOcvMVXmtj741AoUGLYnWsAvG5geuLHX_phvfOuT8",
                    Salt = "3E921C45F3A9089BDC7E"
                },
                new
                {
                    Id = Guid.Parse("E8CC12BA-4DF6-4B06-B96E-9AD00A927A93"),
                    UserName = "QueenOfTheNorth",
                    Password = "B1A8A1223DCA3A102726",
                    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJlOGNjMTJiYS00ZGY2LTRiMDYtYjk2ZS05YWQwMGE5MjdhOTMiLCJuYmYiOjE2NzcyMzUyNDcsImV4cCI6MTY3NzQ5NDQ0NywiaWF0IjoxNjc3MjM1MjQ3fQ.NMCEXx8Dhr40krHQrpz4Zwgslj9N_HN3fi_Qrt4oMes",
                    Salt = "A91F72A37D0E46037B85"
                });
        }
    }
}
