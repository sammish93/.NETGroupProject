using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;

namespace Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Data
{


    public class UserIconContext : DbContext
    {
        public UserIconContext(DbContextOptions<UserIconContext> options) : base(options)
        {
        }

        public DbSet<V1UserIcon> UserIcons { get; set; }

    }
}

