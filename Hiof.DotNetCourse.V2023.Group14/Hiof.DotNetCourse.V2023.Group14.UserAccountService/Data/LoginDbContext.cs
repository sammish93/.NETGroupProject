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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginModel>().HasData(
                new LoginModel
                {
                    Id = 1,
                    UserName = "stian",
                    Password = "86D4CF04EDF276BA6AF1",
                    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1IiwibmJmIjoxNjc2Mzg2MDMyLCJleHAiOjE2NzY2NDUyMzIsImlhdCI6MTY3NjM4NjAzMn0.q4akzYxENnXvykOPEYCqqZ9tSPT0fnoIvPtfFAs5IwQ",
                    Salt = "20OVaK6glLyqxg=="
                }
            );
        }
    }
}

