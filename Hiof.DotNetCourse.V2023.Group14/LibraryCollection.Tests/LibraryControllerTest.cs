using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.LibraryCollectionService.Controllers.V1;
using Hiof.DotNetCourse.V2023.Group14.LibraryCollectionService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text.Json;
using Hiof.DotNetCourse.V2023.Group14.LibraryCollectionService.Data;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.Security;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using NuGet.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LibraryCollection.Tests
{
    public class LibraryControllerTest
    {
        // Objects to insert and delete in a database.
        V1LibraryEntry entry1 = new V1LibraryEntry
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
        };

        V1LibraryEntry entry2 = new V1LibraryEntry
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
        };

        V1LibraryEntry entry3 = new V1LibraryEntry
        {
            Id = Guid.Parse("b77cc25f-68ed-40ab-9b0e-91ab588557f2"),
            UserId = Guid.Parse("54AF86BF-346A-4CBA-B36F-527748E1CB93"),
            LibraryEntryISBN10 = "1119797209",
            LibraryEntryISBN13 = "9781119797203",
            Title = "Professional C# and .NET",
            MainAuthor = "Christian Nagel",
            ReadingStatus = ReadingStatus.ToRead
        };

        V1LibraryEntry entry4 = new V1LibraryEntry
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
        };

        V1LibraryEntry entry5 = new V1LibraryEntry
        {
            Id = Guid.Parse("f26d0753-c47a-4745-9cd7-b207790617d0"),
            UserId = Guid.Parse("E8CC12BA-4DF6-4B06-B96E-9AD00A927A93"),
            LibraryEntryISBN10 = "144810369X",
            LibraryEntryISBN13 = "9781448103690",
            Title = "Kafka on the Shore",
            MainAuthor = "Haruki Murakami",
            ReadingStatus = ReadingStatus.Reading
        };

        V1LibraryEntry entry6 = new V1LibraryEntry
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
        };

        [Fact]
        public async Task GetOkResponseOnGetUserLibrary()
        {
            // Creates a db with a Guid to ensure that databases aren't overridden for each test.
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            // Dispose of context once test is complete.
            using var dbContext = new LibraryCollectionContext(options);


            dbContext.Add(entry1);
            dbContext.Add(entry2);
            dbContext.Add(entry3);
            dbContext.Add(entry4);
            dbContext.Add(entry5);
            dbContext.Add(entry6);
            dbContext.SaveChanges();

            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.GetUserLibrary(entry5.UserId);

            var receivedJson = JObject.Parse(actionResult.ToJson());
            var userId = Convert.ToString(receivedJson["Value"]?["UserId"]);


            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(entry5.UserId.ToString(), userId);
        }

        [Fact]
        public async Task GetNotFoundResponseOnGetUserLibrary()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);


            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.GetUserLibrary(entry5.UserId);


            Assert.IsType<NotFoundObjectResult>(actionResult);
        }

        [Fact]
        public async Task GetOkResponseOnGetAllEntries()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);

            // Adds 6 entries.
            dbContext.Add(entry1);
            dbContext.Add(entry2);
            dbContext.Add(entry3);
            dbContext.Add(entry4);
            dbContext.Add(entry5);
            dbContext.Add(entry6);
            dbContext.SaveChanges();

            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.GetAllEntries();

            var receivedJson = JObject.Parse(actionResult.ToJson())["Value"];


            Assert.IsType<OkObjectResult>(actionResult);
            // Checks that there's 6 entries in the database.
            Assert.Equal("6", receivedJson?.Count().ToString());
        }

        [Fact]
        public async Task GetNotFoundResponseOnGetAllEntries()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);


            // Adds 0 entries.

            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.GetAllEntries();


            Assert.IsType<NotFoundObjectResult>(actionResult);
        }

        [Fact]
        public async Task GetOkResponseOnGetEntry()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);


            dbContext.Add(entry1);
            dbContext.SaveChanges();

            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.GetEntry(entry1.Id);

            var receivedJson = JObject.Parse(actionResult.ToJson());
            var entryId = Convert.ToString(receivedJson["Value"]?["Id"]);


            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(entry1.Id.ToString(), entryId);
        }

        [Fact]
        public async Task GetNotFoundResponseOnGetEntry()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);


            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.GetEntry(entry1.Id);


            Assert.IsType<NotFoundObjectResult>(actionResult);
        }

        [Fact]
        public async Task GetOkResponseOnDeleteEntry()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);


            dbContext.Add(entry1);
            dbContext.SaveChanges();

            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.GetEntry(entry1.Id);

            var receivedJson = JObject.Parse(actionResult.ToJson());
            var entryId = Convert.ToString(receivedJson["Value"]?["Id"]);

            // Asserts that entry exists.
            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(entry1.Id.ToString(), entryId);


            var delActionResult = await controller.DeleteEntry(entry1.Id);


            // Asserts that the deletion was successful
            Assert.IsType<OkResult>(delActionResult);


            var actionResultTwo = await controller.GetEntry(entry1.Id);

            // Asserts that the entry has been deleted.
            Assert.IsType<NotFoundObjectResult>(actionResultTwo);
        }

        [Fact]
        public async Task GetNotFoundResponseOnDeleteEntry()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);


            // No entries added.

            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.GetEntry(entry1.Id);


            // Asserts that the entry doesn't exist.
            Assert.IsType<NotFoundObjectResult>(actionResult);


            var delActionResult = await controller.DeleteEntry(entry1.Id);


            // Asserts that there was nothing to be deleted.
            Assert.IsType<NotFoundObjectResult>(delActionResult);
        }

        [Fact]
        public async Task GetNotFoundResponseOnDeleteUserLibrary()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);


            // No entries added.

            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.GetUserLibrary(entry1.UserId);


            // Asserts that the user library doesn't exist.
            Assert.IsType<NotFoundObjectResult>(actionResult);


            var delActionResult = await controller.DeleteUserLibrary(entry1.UserId);


            // Asserts that there was nothing to be deleted.
            Assert.IsType<NotFoundObjectResult>(delActionResult);
        }

        [Fact]
        public async Task GetOkResponseOnDeleteUserLibrary()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);



            dbContext.Add(entry1);
            dbContext.Add(entry2);
            dbContext.Add(entry3);
            dbContext.Add(entry4);
            dbContext.Add(entry5);
            dbContext.Add(entry6);
            dbContext.SaveChanges();

            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.GetUserLibrary(entry5.UserId);

            var receivedJson = JObject.Parse(actionResult.ToJson());
            var userId = Convert.ToString(receivedJson["Value"]?["UserId"]);

            // Asserts that the library exists.
            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(entry5.UserId.ToString(), userId);


            // Asserts that the deletion was successful.
            var delActionResult = await controller.DeleteUserLibrary(entry5.UserId);
            Assert.IsType<OkResult>(delActionResult);


            // Asserts that the library no longer exists.
            var actionResultTwo = await controller.GetUserLibrary(entry5.UserId);
            Assert.IsType<NotFoundObjectResult>(actionResultTwo);
        }

        [Fact]
        public async Task GetOkResponseChangeRating()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);


            dbContext.Add(entry1);
            dbContext.SaveChanges();

            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.GetEntry(entry1.Id);

            var receivedJson = JObject.Parse(actionResult.ToJson());
            var entryId = Convert.ToString(receivedJson["Value"]?["Rating"]);

            // Asserts the rating is the same as when entry1 was added.
            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(entry1.Rating.ToString(), entryId);


            // Asserts that the rating was successfully changed.
            var modActionResult = await controller.ChangeRating(entry1.Id, 4);
            Assert.IsType<OkResult>(modActionResult);
            Assert.NotEqual(entry1.Rating.ToString(), entryId);
        }

        [Fact]
        public async Task GetBadRequestResponseChangeRatingToEleven()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);


            dbContext.Add(entry1);
            dbContext.SaveChanges();

            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.GetEntry(entry1.Id);

            var receivedJson = JObject.Parse(actionResult.ToJson());
            var entryId = Convert.ToString(receivedJson["Value"]?["Rating"]);

            // Asserts the entry exists.
            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(entry1.Rating.ToString(), entryId);


            // Asserts that the rating was not changed.
            var modActionResult = await controller.ChangeRating(entry1.Id, 11);
            Assert.IsType<BadRequestObjectResult>(modActionResult);
        }

        public async Task GetBadRequestResponseChangeRatingToZero()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);


            dbContext.Add(entry1);
            dbContext.SaveChanges();

            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.GetEntry(entry1.Id);

            var receivedJson = JObject.Parse(actionResult.ToJson());
            var entryId = Convert.ToString(receivedJson["Value"]?["Rating"]);

            // Asserts the entry exists.
            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(entry1.Rating.ToString(), entryId);


            // Asserts that the rating was not changed.
            var modActionResult = await controller.ChangeRating(entry1.Id, 0);
            Assert.IsType<BadRequestObjectResult>(modActionResult);
        }

        [Fact]
        public async Task GetOkResponseChangeDateRead()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);


            dbContext.Add(entry1);
            dbContext.SaveChanges();

            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.GetEntry(entry1.Id);

            var receivedJson = JObject.Parse(actionResult.ToJson());
            var entryId = Convert.ToString(receivedJson["Value"]?["DateRead"]);

            // Asserts the rating is the same as when entry1 was added.
            Assert.IsType<OkObjectResult>(actionResult);
            // Convert time into UTC.
            var dateTime = DateTime.Parse(entry1.DateRead.ToString()).ToUniversalTime();
            Assert.Equal(dateTime.ToString(), entryId);


            // Asserts that the rating was successfully changed.
            var modActionResult = await controller.ChangeDateRead(entry1.Id, DateTime.Parse("2012-12-12"));
            Assert.IsType<OkResult>(modActionResult);
            Assert.NotEqual(entry1.DateRead.ToString(), entryId);
        }

        [Fact]
        public async Task GetOkResponseAddEntry()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);


            V1LibraryEntry entry = new V1LibraryEntry
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
            };

            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.CreateEntry(entry);


            // Asserts that the add request was successful.
            Assert.IsType<OkResult>(actionResult);


            var actionResultTwo = await controller.GetEntry(entry.Id);

            var receivedJson = JObject.Parse(actionResultTwo.ToJson());
            var entryId = Convert.ToString(receivedJson["Value"]?["Id"]);


            // Asserts that the entry exists.
            Assert.IsType<OkObjectResult>(actionResultTwo);
            Assert.Equal(entry.Id.ToString(), entryId);
        }

        [Fact]
        public async Task GetBadRequestResponseAddEntryNoISBN()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);


            V1LibraryEntry entry = new V1LibraryEntry
            {
                Id = Guid.Parse("2d87b44e-20da-45a8-abdf-8296f251a680"),
                UserId = Guid.Parse("54AF86BF-346A-4CBA-B36F-527748E1CB93"),
                LibraryEntryISBN10 = "",
                LibraryEntryISBN13 = "",
                Title = "The Moon Is Down",
                MainAuthor = "John Steinbeck",
                Rating = 8,
                DateRead = DateTime.Parse("2023-02-24T12:55:19.113"),
                ReadingStatus = ReadingStatus.Completed
            };

            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.CreateEntry(entry);


            // Asserts that the add request was not successful.
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task GetBadRequestResponseAddEntryInvalidISBN()
        {
            var options = new DbContextOptionsBuilder<LibraryCollectionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new LibraryCollectionContext(options);


            V1LibraryEntry entry = new V1LibraryEntry
            {
                Id = Guid.Parse("2d87b44e-20da-45a8-abdf-8296f251a680"),
                UserId = Guid.Parse("54AF86BF-346A-4CBA-B36F-527748E1CB93"),
                LibraryEntryISBN10 = "12345678901234567890",
                LibraryEntryISBN13 = "",
                Title = "The Moon Is Down",
                MainAuthor = "John Steinbeck",
                Rating = 8,
                DateRead = DateTime.Parse("2023-02-24T12:55:19.113"),
                ReadingStatus = ReadingStatus.Completed
            };

            var controller = new V1LibraryCollectionController(dbContext);

            var actionResult = await controller.CreateEntry(entry);


            // Asserts that the add request was not successful.
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

    }
}