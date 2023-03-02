using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Controllers.V1;
using Microsoft.Extensions.Options;

namespace UserAccount.Tests
{
    public class UserAccountControllerTest
    {
        V1User user1 = new()
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
        };
        V1User user2 = new()
        {
            Id = Guid.Parse("E8CC12BA-4DF6-4B06-B900-9AD00A927A93"),
            UserName = "Min",
            Email = "ginggy@comcast.com",
            //Password is "Theglamourtodad06"
            Password = "BFB48F7A237192EFA6AB",
            FirstName = "Ginger",
            LastName = "Minge",
            Country = "Canada",
            City = "Birmingham",
            LangPreference = "en",
            Role = UserRole.User,
            RegistrationDate = DateTime.Parse("2023 - 03 - 02 09:07:57.820"),
            LastActive = DateTime.Parse("2023-03-02 09:07:57.820"),
        };
        V1User user3 = new()
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
            LastActive = DateTime.Parse("2023-02-24 10:37:08.387"),
        };
        V1User user4 = new()
        {
            Id = Guid.Parse("3FA85F64-5717-4562-B3FC-2C963B98AFA6"),
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
            LastActive = DateTime.Parse("2023-02-24 10:37:08.387"),
        };
        V1User user5 = new()
        {
            Id = Guid.Parse("54AF86BF-346A-4CBA-B36F-527748E1CB93"),
            UserName = "testaccount1",
            Email = "testme",
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
        };
        V1User user6 = new()
        {
            Id = Guid.Parse("54AF86BF-346A-4CBA-B36F-527748E1CB93"),
            UserName = "testaccount",
            Email = "testme@test.no",
            // Password is "TestPassword123!"
            Password = "whoo",
            FirstName = "Ola",
            LastName = "Nordmann",
            Country = "Norway",
            City = "Oslo",
            LangPreference = "no",
            Role = UserRole.Admin,
            RegistrationDate = DateTime.Parse("2023-02-24 10:42:49.373"),
            LastActive = DateTime.Parse("2023-02-24 10:42:49.373"),
        };

        [Fact]

        public async Task TestCreateReturnsBadRequestResultWhenUsernameAlreadyExists()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var dbContext = new UserAccountContext(options);

            dbContext.Add(user3);
            dbContext.SaveChanges();

            var controller = new V1UserAccountController(dbContext);

            var actionResult = await controller.Create(user4);

            var badResult = Assert.IsType<BadRequestObjectResult>(actionResult);

            Assert.Equal("The username is already in use.", badResult.Value);

        }

        [Fact]
        public async Task TestCreateReturnsBadRequestResultWhenPasswordInvalid()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var dbContext = new UserAccountContext(options);

            var controller = new V1UserAccountController(dbContext);

            var actionResult = await controller.Create(user6);

            var badResult = Assert.IsType<BadRequestObjectResult>(actionResult);

            Assert.Equal("Password must have at least one lower-case letter, " +
                         "one upper-case letter," +
                        " one number, and one special character, " +
                        "and be at least 5 characters long.", badResult.Value);
        }
        /*
        [Fact]

        public async Task estCreateReturnsBadRequestResultWhenEmailIsInvalid()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var dbContext = new UserAccountContext(options);

            var controller = new V1UserAccountController(dbContext);

            var actionResult = await controller.;

            var badResult = Assert.IsType<BadRequestObjectResult>(actionResult);

            Assert.Equal("Email must be of a valid format.", badResult.Value);
        }
        */

        [Fact]

        public async Task TestCreateOkResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var dbContext = new UserAccountContext(options);

            var controller = new V1UserAccountController(dbContext);

            var result = await controller.Create(user1);

            Assert.IsType<OkResult>(result);

            
        }

        
    }
      
}
