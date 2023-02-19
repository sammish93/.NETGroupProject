using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService
{
	public interface ILoginDbContext
	{
        DbSet<V1LoginModel> LoginModel { get; set; }
        int SaveChanges();
    }
}

 