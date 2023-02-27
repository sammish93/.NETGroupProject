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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<V1LibraryEntry>().HasData(
                new
                {
                    Id = Guid.Parse("2d87b44e-20da-45a8-abdf-8296f251a680"),
                    UserId = Guid.Parse("54AF86BF-346A-4CBA-B36F-527748E1CB93"),
                    LibraryEntryISBN10 = "1440674132",
                    LibraryEntryISBN13 = "9781440674136",
                    Title = "The Moon Is Down",
                    MainAuthor = "John Steinbeck",
                    Rating = 8,
                    DateRead = DateTime.Parse("2023-02-24T12:55:19.113"),
                    ReadingStatus = ReadingStatus.Completed
                },
                new
                {
                    Id = Guid.Parse("3bba26a9-3d8e-4f51-9ff4-1ad2d8da112b"),
                    UserId = Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA6"),
                    LibraryEntryISBN10 = "1440674132",
                    LibraryEntryISBN13 = "9781440674136",
                    Title = "The Moon Is Down",
                    MainAuthor = "John Steinbeck",
                    Rating = 7,
                    DateRead = DateTime.Parse("2023-01-24T11:54:29.123"),
                    ReadingStatus = ReadingStatus.Completed
                },
                new
                {
                    Id = Guid.Parse("b77cc25f-68ed-40ab-9b0e-91ab588557f2"),
                    UserId = Guid.Parse("54AF86BF-346A-4CBA-B36F-527748E1CB93"),
                    LibraryEntryISBN10 = "1119797209",
                    LibraryEntryISBN13 = "9781119797203",
                    Title = "Professional C# and .NET",
                    MainAuthor = "Christian Nagel",
                    ReadingStatus = ReadingStatus.ToRead
                },
                new
                {
                    Id = Guid.Parse("5c7629a7-bca3-481e-bddb-ffc263f7232a"),
                    UserId = Guid.Parse("54AF86BF-346A-4CBA-B36F-527748E1CB93"),
                    LibraryEntryISBN10 = "0486415872",
                    LibraryEntryISBN13 = "9780486415871",
                    Title = "Crime and Punishment",
                    MainAuthor = "Fyodor Dostoyevsky",
                    Rating = 9,
                    DateRead = DateTime.Parse("2023-02-18T08:53:21.423"),
                    ReadingStatus = ReadingStatus.Completed
                },
                new
                {
                    Id = Guid.Parse("f26d0753-c47a-4745-9cd7-b207790617d0"),
                    UserId = Guid.Parse("E8CC12BA-4DF6-4B06-B96E-9AD00A927A93"),
                    LibraryEntryISBN10 = "144810369X",
                    LibraryEntryISBN13 = "9781448103690",
                    Title = "Kafka on the Shore",
                    MainAuthor = "Haruki Murakami",
                    ReadingStatus = ReadingStatus.Reading
                },
                new
                {
                    Id = Guid.Parse("8cae4a7d-a7e3-4d19-a20d-cb6b07641e95"),
                    UserId = Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA6"),
                    LibraryEntryISBN10 = "144810369X",
                    LibraryEntryISBN13 = "9781448103690",
                    Title = "Kafka on the Shore",
                    MainAuthor = "Haruki Murakami",
                    Rating = 10,
                    DateRead = DateTime.Parse("2023-02-21T07:43:11.453"),
                    ReadingStatus = ReadingStatus.Completed
                });
        }
    }
}
