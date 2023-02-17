using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Security;
using Hiof.DotNetCourse.V2023.Group14.LoginService.Controllers;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Controllers;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace APICommunicator.Tests
{
	public class LoginControllerTest
	{
		/*
		[Fact]
		public void TestValidLoginAttempt()
		{
			var mock = new Mock<LoginDbContext>(new DbContextOptionsBuilder<LoginDbContext>().Options);
            var loginModel = new LoginModel { UserName = "stian" };
            mock.Setup(m => m.LoginModel.Single(l => l.UserName == "stian")).Returns(loginModel);

            var controller = new LoginController(mock.Object);

			var result = controller.VerifyLogin(new LoginInfo("stian", "abc123"));

			Assert.Equal("Login Success", result);
		}
		*/

	}
}

