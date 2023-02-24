using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hiof.DotNetCourse.V2023.Group14.LibraryCollectionService.Data
{
    public class LibraryCollectionContext : DbContext
    {
        public LibraryCollectionContext(DbContextOptions<LibraryCollectionContext> dbContextOptions) : base(dbContextOptions) { }


        public DbSet<V1LibraryEntry> LibraryEntries { get; set; }

    }
}
