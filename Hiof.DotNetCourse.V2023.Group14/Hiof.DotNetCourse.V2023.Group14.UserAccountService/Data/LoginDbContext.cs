using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data
{
    public class LoginDbContext : DbContext
	{
        public LoginDbContext(DbContextOptions<LoginDbContext> options) : base(options) { }

		public DbSet<V1LoginModel> LoginModel { get; set; }
        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<V1LoginModel>().HasData(
                new V1LoginModel
                {
                    Id = 1,
                    UserName = "stian",
                    Password = "86D4CF04EDF276BA6AF1",
                    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1IiwibmJmIjoxNjc2Mzg2MDMyLCJleHAiOjE2NzY2NDUyMzIsImlhdCI6MTY3NjM4NjAzMn0.q4akzYxENnXvykOPEYCqqZ9tSPT0fnoIvPtfFAs5IwQ",
                    Salt = "20OVaK6glLyqxg=="
                }
            );
        }
        */
    }
}

