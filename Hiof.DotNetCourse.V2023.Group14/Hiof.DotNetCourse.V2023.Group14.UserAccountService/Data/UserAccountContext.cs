using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data
{
    public class UserAccountContext : DbContext
    {
        public UserAccountContext(DbContextOptions<UserAccountContext> dbContextOptions) : base(dbContextOptions) { }


        public DbSet<V1User> Users { get; set; }
    }
}
