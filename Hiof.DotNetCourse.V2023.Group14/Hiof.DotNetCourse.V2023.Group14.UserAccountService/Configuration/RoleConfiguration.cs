using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Configuration
{
	public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
	{
	
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
			builder.HasData(new IdentityRole
			{
				Name = "Admin",
				NormalizedName = "ADMIN"
			},
			new IdentityRole
			{
				Name = "User",
				NormalizedName = "USER"
			},
			new IdentityRole
			{
				Name = "Author",
				NormalizedName = "AUTOR"
			},
			new IdentityRole
			{
				Name = "Moderator",
				NormalizedName = "MODERATOR"
			});	
        }
    }
}

