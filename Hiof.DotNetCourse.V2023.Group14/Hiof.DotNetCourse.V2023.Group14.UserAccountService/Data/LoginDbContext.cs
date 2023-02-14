using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Security;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data
{
	public class LoginDbContext : DbContext
	{
        public LoginDbContext(DbContextOptions<LoginDbContext> options) : base(options) { }

		public DbSet<LoginModel> LoginModel { get; set; }
	}
}

