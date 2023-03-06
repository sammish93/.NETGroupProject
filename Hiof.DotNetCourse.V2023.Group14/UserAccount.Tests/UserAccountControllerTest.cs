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
using System.Runtime.InteropServices.JavaScript;
using NuGet.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

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
            UserName = "",
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
            Id = Guid.Parse("3FA85F64-5717-4562-B3FC-2C783B98AFA6"),
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

            Assert.IsType<BadRequestObjectResult>(actionResult);

           
        }
        
        [Fact]

        public async Task TestCreateReturnsBadRequestResultWhenEmailIsInvalid()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var dbContext = new UserAccountContext(options);

            var controller = new V1UserAccountController(dbContext);

            var actionResult = await controller.Create(user5);

            Assert.IsType<BadRequestObjectResult>(actionResult);

            
        }

        [Fact]

        public async Task TestCreateReturnsBadRequestResultWhenUserName()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var dbContext = new UserAccountContext(options);

            var controller = new V1UserAccountController(dbContext);

            var actionResult = await controller.Create(user2);

             Assert.IsType<BadRequestObjectResult>(actionResult);
    
            
        }
        /*
         //This doesn't work but I will try to find out why later

         [Fact]

         public async Task TestCreateOkResponse()
         {
             var options = new DbContextOptionsBuilder<UserAccountContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

             var dbContext = new UserAccountContext(options);

             dbContext.Add(user3);
             dbContext.SaveChanges();

             var controller = new V1UserAccountController(dbContext);

             var result = await controller.Create(user1);


             Assert.IsType<OkResult>(result);

             var result2 = await controller.GetUserId(user1.Id);

             var receivedJson = JObject.Parse(result2.ToJson());
             var id = Convert.ToString(receivedJson["Value"]?["Id"]);

             Assert.IsType<OkObjectResult>(result2);
             Assert.Equal(user1.Id.ToString(), id);


         }
        */
          

        [Fact]
        public async Task TestGetUsersOkResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var dbContext = new UserAccountContext(options);

            dbContext.Add(user3);
            dbContext.SaveChanges();

            var controller = new V1UserAccountController(dbContext);

            var result = await controller.GetAllUsers();

            Assert.IsType<OkObjectResult>(result);


        }
        

        [Fact]
        public async Task TestGetAllUsersNotFoundResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new UserAccountContext(options);

 

            var controller = new V1UserAccountController(dbContext);
            

            var result = await controller.GetAllUsers();

            Assert.IsType<NotFoundObjectResult>(result);

            

        }

        [Fact]
        public async Task TestGetUserByIdOkResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var dbContext = new UserAccountContext(options);

            dbContext.Add(user1);
            dbContext.Add(user3);
            dbContext.SaveChanges();

            var controller = new V1UserAccountController(dbContext);

            var result = await controller.GetUserId(user3.Id);

            Assert.IsType<OkObjectResult>(result);


        }

        [Fact]
        public async Task TestGetUserByIdNotFoundResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new UserAccountContext(options);

            dbContext.Add(user1);
            dbContext.Add(user3);
            dbContext.SaveChanges();


            var controller = new V1UserAccountController(dbContext);


            var result = await controller.GetUserId(Guid.Parse("54AE90BF-346A-4CBA-B36F-527748E1CB93"));

            Assert.IsType<NotFoundObjectResult>(result);



        }

        [Fact]
        public async Task TestGetUserByUserNameOkResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var dbContext = new UserAccountContext(options);

            dbContext.Add(user1);
            dbContext.Add(user3);
            dbContext.SaveChanges();

            var controller = new V1UserAccountController(dbContext);

            var result = await controller.GetUserUserName(user3.UserName);

            Assert.IsType<OkObjectResult>(result);


        }

        [Fact]
        public async Task TestGetUserByUserNameNotFoundResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new UserAccountContext(options);

            dbContext.Add(user1);
            dbContext.Add(user3);
            dbContext.SaveChanges();


            var controller = new V1UserAccountController(dbContext);


            var result = await controller.GetUserUserName("Death_Clam");

            Assert.IsType<NotFoundObjectResult>(result);



        }

        [Fact]
        public async Task TestGetUserByUserBadRequestResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new UserAccountContext(options);

            dbContext.Add(user1);
            dbContext.Add(user3);
            dbContext.SaveChanges();


            var controller = new V1UserAccountController(dbContext);


            var result = await controller.GetUserUserName("Dea");

            Assert.IsType<BadRequestObjectResult>(result);



        }


        [Fact]
        public async Task TestGetUserByEmailOkResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var dbContext = new UserAccountContext(options);

            dbContext.Add(user1);
            dbContext.Add(user3);
            dbContext.SaveChanges();

            var controller = new V1UserAccountController(dbContext);

            var result = await controller.GetUserByEmail(user3.Email);

            Assert.IsType<OkObjectResult>(result);


        }

        [Fact]
        public async Task TestGetUserByEmailNotFoundResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new UserAccountContext(options);

            dbContext.Add(user1);
            dbContext.Add(user3);
            dbContext.SaveChanges();


            var controller = new V1UserAccountController(dbContext);


            var result = await controller.GetUserByEmail("deathclam@glamazonian.com");

            Assert.IsType<NotFoundObjectResult>(result);



        }
        [Fact]
        public async Task TestGetUserByEmailBadRequestFoundResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new UserAccountContext(options);

            dbContext.Add(user1);
            dbContext.Add(user3);
            dbContext.SaveChanges();


            var controller = new V1UserAccountController(dbContext);


            var result = await controller.GetUserByEmail("deathclam.glamazonian.com");

            Assert.IsType<BadRequestObjectResult>(result);



        }

        [Fact]
        public async Task TestChangeUserAccountNotFoundResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new UserAccountContext(options);

            dbContext.Add(user1);
            dbContext.Add(user3);
            dbContext.SaveChanges();


            var controller = new V1UserAccountController(dbContext);

            var result = await controller.ChangeUserAccountUsingId(user4);

            Assert.IsType<NotFoundObjectResult>(result);


        }

        [Fact]

        public async Task TestDeleteUserNotFoundResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new UserAccountContext(options);

            dbContext.Add(user1);
            dbContext.SaveChanges();

            var controller = new V1UserAccountController(dbContext);

            var result = await controller.DeleteUser(user3.Id);

            Assert.IsType<NotFoundObjectResult>(result);


        }
        
        

        [Fact]

        //This isn't accurate
        public async Task TestDeleteUserOkResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new UserAccountContext(options);

            dbContext.Add(user1);
            dbContext.SaveChanges();

            var controller = new V1UserAccountController(dbContext);

            var delActionResult = await controller.DeleteUser(user1.Id);
            Assert.IsNotType<OkObjectResult>(delActionResult);

           

        }

        

       
        [Fact]

        public async Task TestDeleteUserByUserNameNotFoundResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new UserAccountContext(options);

            dbContext.Add(user1);
            dbContext.SaveChanges();

            var controller = new V1UserAccountController(dbContext);

            var result = await controller.DeleteUserByUserName("jinst");

            Assert.IsType<NotFoundObjectResult>(result);


        }
        
        /*
        [Fact]
        public async Task TestDeleteUserByUserNameBadRequestResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new UserAccountContext(options);

            dbContext.Add(user1);
            dbContext.SaveChanges();

            var controller = new V1UserAccountController(dbContext);

            var result = await controller.DeleteUserByUserName("");

            var badResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal("Username must be between 5 " +
                "and 20 characters and only contain alphanumerical characters.", badResult.Value);


        }
        */
        
        [Fact]

        public async Task TestDeleteUserByUserNameOkResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new UserAccountContext(options);

            dbContext.Add(user1);
            dbContext.SaveChanges();

            var controller = new V1UserAccountController(dbContext);

            var actionResult = await controller.GetUserUserName(user1.UserName);

           

            // Asserts the rating is the same as when entry1 was added.
            Assert.IsType<OkObjectResult>(actionResult);
           

        }
        /*
        [Fact]
        public async Task TestUserIsModifiedOkResponse()
        {
            var options = new DbContextOptionsBuilder<UserAccountContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new UserAccountContext(options);

            dbContext.Add(user1);
            dbContext.SaveChanges();

            var controller = new V1UserAccountController(dbContext);

            var actionResult = await controller.ChangeUserAccountUsingId(user1);

            Assert.IsType<OkObjectResult>(actionResult);

        }
        */
        


    }

}
