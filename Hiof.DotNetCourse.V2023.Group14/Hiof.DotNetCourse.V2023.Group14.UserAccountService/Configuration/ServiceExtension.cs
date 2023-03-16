using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Configuration
{
	public static class ServiceExtension
	{
	
		public static void ConfigureIdentity(this IServiceCollection services)
		{
			// Adding and configuring identity for a spesific type.
			// In this case, the V1UserIdentity and IdentityRole.
			var builder = services.AddIdentity<V1UserIdentity, IdentityRole>(user =>
			{
                user.User.RequireUniqueEmail = true;
                user.Password.RequireDigit = true;
				user.Password.RequireLowercase = false;
				user.Password.RequireUppercase = false;
				user.Password.RequireNonAlphanumeric = false;
				user.Password.RequiredLength = 8;
			})
			.AddEntityFrameworkStores<UserIdentityContext>()
			.AddDefaultTokenProviders();

        }
	}
}

