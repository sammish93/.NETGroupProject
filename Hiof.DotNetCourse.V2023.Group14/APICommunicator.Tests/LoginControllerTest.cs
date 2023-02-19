using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Security;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Controllers;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace APICommunicator.Tests
{
	public class LoginControllerTest
	{

        [Fact]
        public async Task TestOkResponse200()
        {
            // Opprett en instans av LoginDbContext
            var options = new DbContextOptionsBuilder<LoginDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            var dbContext = new LoginDbContext(options);

            // Legg til eksisterende bruker i databasen.
            var user = new LoginModel
            {
                UserName = "stian",
                Password = "86D4CF04EDF276BA6AF1",
                Salt = "20OVaK6glLyqxg=="
            };
            dbContext.Add(user);
            dbContext.SaveChanges();

            // Opprett en instans av LoginController med LoginDbContext.
            var controller = new LoginController(dbContext);

            // Oppretter en testbruker 
            var test = new LoginInfo("stian", "abc123");

            var actionResult = await controller.VerifyLogin(test);

            var okResult = Assert.IsType<OkObjectResult>(actionResult);

            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task TestBadResponse401()
        {
            // Opprett en instans av LoginDbContext
            var options = new DbContextOptionsBuilder<LoginDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            var dbContext = new LoginDbContext(options);

            // Legg til eksisterende bruker i databasen.
            var user = new LoginModel
            {
                UserName = "stian",
                Password = "86D4CF04EDF276BA6AF1",
                Salt = "20OVaK6glLyqxg=="
            };
            dbContext.Add(user);
            dbContext.SaveChanges();

            // Opprett en instans av LoginController med LoginDbContext.
            var controller = new LoginController(dbContext);

            // Oppretter en testbruker 
            var test = new LoginInfo("fakeUser", "fake123");

            var actionResult = await controller.VerifyLogin(test);

            var badResult = Assert.IsType<UnauthorizedObjectResult>(actionResult);

            Assert.Equal(401, badResult.StatusCode);

        }

    }
}

