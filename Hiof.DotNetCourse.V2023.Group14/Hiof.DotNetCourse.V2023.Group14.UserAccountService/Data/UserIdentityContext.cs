using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data
{
    // The class inherits from IdentityDbContext, not DbContext, because
    // we want to integrate our context with identity.
	public class UserIdentityContext : IdentityDbContext<V1UserIdentity>
	{
		public UserIdentityContext(DbContextOptions options) : base(options)
		{
		}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<V1UserIdentity> UserIdentity { get; set; }
    }
}

