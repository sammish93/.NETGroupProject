
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ReadingGoalService.Controllers.V1;
using Hiof.DotNetCourse.V2023.Group14.ReadingGoalService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;



namespace ReadingGoals.Tests
{
    public class ReadingGoalsControllerTest
    {
        V1ReadingGoals readingGoal1 = new V1ReadingGoals()
        {
            Id = Guid.Parse("6ef0ab52-389a-478c-affe-052bd1fc2715"),
            UserId = Guid.Parse("bfcbf470-9ae4-4af9-b294-d37771f6eea2"),
            GoalStartDate = DateTime.Parse("2022-10-22T12:55:19.113"),
            GoalEndDate = DateTime.Parse("2022-11-22T12:55:19.113"),
            GoalTarget = 20,
            GoalCurrent = 3,
            LastUpdated = DateTime.Parse("2023-02-24T12:55:19.113")


        };
        V1ReadingGoals readingGoal2 = new V1ReadingGoals()
        {
            Id = Guid.Parse("e34910a0-5196-4777-9dfd-b402c15582dc"),
            UserId = Guid.Parse("bfcbf470-9ae4-4af9-b294-d37771f6eea2"),
            GoalStartDate = DateTime.Parse("2023-02-24T12:55:19.113"),
            GoalEndDate = DateTime.Parse("2023-03-24T12:55:19.113"),
            GoalTarget = 30,
            GoalCurrent = 6,
            LastUpdated = DateTime.Parse("2023-02-24T12:55:19.113")

        };
        V1ReadingGoals addNewGoal = new V1ReadingGoals()
        {
            Id = Guid.Parse("c218f762-e53a-48ef-b90e-f6dde9c1c09d"),
            UserId = Guid.Parse("8ac89236-b1d2-4812-8a0b-bdf7581bf650"),
            GoalStartDate = DateTime.Parse("2023-02-24T12:55:19.113"),
            GoalEndDate = DateTime.Parse("2023-03-24T12:55:19.113"),
            GoalTarget = 15,
            GoalCurrent = 3,
            LastUpdated = DateTime.Parse("2023-02-24T12:55:19.113")

        };
        V1ReadingGoals readingGoal4 = new V1ReadingGoals()
        {
            Id = Guid.Parse("296a0a6a-c564-49b7-8748-eec430cd4d1e"),
            UserId = Guid.Parse("8ac89236-b1d2-4812-8a0b-bdf7581bf650"),
            GoalStartDate = DateTime.Parse("2023-02-24T12:55:19.113"),
            GoalEndDate = DateTime.Parse("2023-03-24T12:55:19.113"),
            GoalTarget = 30,
            GoalCurrent = 6,
            LastUpdated = DateTime.Parse("2023-02-24T12:55:19.113")
        };
        V1ReadingGoals update = new V1ReadingGoals()
        {
            Id = Guid.Parse("e34910a0-5196-4777-9dfd-b402c15582dc"),
            UserId = Guid.Parse("bfcbf470-9ae4-4af9-b294-d37771f6eea2"),
            GoalStartDate = DateTime.Parse("2023-02-24T12:55:19.113"),
            GoalEndDate = DateTime.Parse("2023-03-24T12:55:19.113"),
            GoalTarget = 30,
            GoalCurrent = 6,
            LastUpdated = DateTime.Parse("2023-02-24T12:55:19.113")
        };
        [Fact]
        public async Task GetOkResponseOnCreateGoal()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ReadingGoalsContext(options);

           
           
            dbContext.Add(readingGoal2);

           
            var loggerMock = new Mock<ILogger<V1ReadingGoalsController>>();
            var controller = new V1ReadingGoalsController(dbContext, loggerMock.Object);

            
            var actionResult = await controller.CreateReadingGoal(addNewGoal);

           
            Assert.IsType<OkResult>(actionResult);
        }
        /*

        [Fact]
        public async Task GetBadRequestResponseOnCreateGoalWithNullReadingGoal()
        {

            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ReadingGoalsContext(options);

            var controller = new V1ReadingGoalsController(dbContext);


            var actionResult = await controller.CreateReadingGoal(null);


            var result = Assert.IsType<BadRequestObjectResult>(actionResult);
            Assert.Equal("Reading goal cannot be null", result.Value);
        }
        [Fact]
        public async Task CreateReadingGoalReturnsBadRequestOnOverlappingReadingGoal()
        {
            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ReadingGoalsContext(options);

            var existingReadingGoal = new V1ReadingGoals
            {
                UserId = Guid.Parse("296a0a6a-c564-49b7-8748-eec430cd4d1e"),
                GoalStartDate = DateTime.Now.Date.AddDays(-5),
                GoalEndDate = DateTime.Now.Date.AddDays(5)
            };
            await dbContext.ReadingGoals.AddAsync(existingReadingGoal);
            await dbContext.SaveChangesAsync();

            var overlappingReadingGoal = new V1ReadingGoals
            {
                UserId = Guid.Parse("296a0a6a-c564-49b7-8748-eec430cd4d1e"),
                GoalStartDate = DateTime.Now.Date,
                GoalEndDate = DateTime.Now.Date.AddDays(10)
            };

            var controller = new V1ReadingGoalsController(dbContext);

            
            var actionResult = await controller.CreateReadingGoal(overlappingReadingGoal);

           
            var result = Assert.IsType<BadRequestObjectResult>(actionResult);
            Assert.Equal("You already have a reading goal during this time period!", result.Value);
        }

        [Fact]
        public async Task GetOkResponseOnGetGoalByUserIdWithExistingGoals()
        {
       
            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ReadingGoalsContext(options);



            dbContext.Add(readingGoal1);
            await dbContext.SaveChangesAsync();

            var controller = new V1ReadingGoalsController(dbContext);


            var actionResult = await controller.GetGoalByUserId(readingGoal1.UserId);


            Assert.IsType<OkObjectResult>(actionResult);
        }
        [Fact]
        public async Task GetNotFoundResponseOnGetGoalByUserIdWithNoExistingGoals()
        {
            
            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ReadingGoalsContext(options);

            var userId = Guid.NewGuid();
            var controller = new V1ReadingGoalsController(dbContext);

            var actionResult = await controller.GetGoalByUserId(userId);

           
            var result = Assert.IsType<NotFoundObjectResult>(actionResult);
            Assert.Equal("There are no existing readings!", result.Value);
        }
        [Fact]
        public async Task GetGoalIdUsingUserIdAndDate_ReturnsOkWithGoalId()
        {
            
            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ReadingGoalsContext(options);

            var userId = Guid.NewGuid();
            var goalDate = DateTime.Now.Date;

            var readingGoal = new V1ReadingGoals
            {
                UserId = userId,
                GoalStartDate = goalDate.AddDays(-5),
                GoalEndDate = goalDate.AddDays(5)
            };
            await dbContext.ReadingGoals.AddAsync(readingGoal);
            await dbContext.SaveChangesAsync();

            var controller = new V1ReadingGoalsController(dbContext);

            
            var actionResult = await controller.GetGoalIdUsingUserIdAndDate(userId, goalDate);

            var result = Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(readingGoal.Id, result.Value);
        }

        [Fact]
        public async Task GetGoalIdUsingUserIdAndDateReturnsNotFound()
        {
            
            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ReadingGoalsContext(options);

            var userId = Guid.NewGuid();
            var goalDate = DateTime.Now.Date;

            var controller = new V1ReadingGoalsController(dbContext);

            
            var actionResult = await controller.GetGoalIdUsingUserIdAndDate(userId, goalDate);

            
            var result = Assert.IsType<NotFoundObjectResult>(actionResult);
            Assert.Equal("No reading goals found!", result.Value);
        }

        [Fact]
        public async Task GetRecentReadingGoalsReturnsOk()
        {
            
            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ReadingGoalsContext(options);

            var userId = Guid.NewGuid();

            var readingGoal1 = new V1ReadingGoals
            {
                UserId = userId,
                GoalStartDate = DateTime.Now.Date.AddDays(-10),
                GoalEndDate = DateTime.Now.Date.AddDays(-5)
            };
            var readingGoal2 = new V1ReadingGoals
            {
                UserId = userId,
                GoalStartDate = DateTime.Now.Date.AddDays(-4),
                GoalEndDate = DateTime.Now.Date.AddDays(1)
            };
            await dbContext.ReadingGoals.AddRangeAsync(readingGoal1, readingGoal2);
            await dbContext.SaveChangesAsync();

            var controller = new V1ReadingGoalsController(dbContext);

            
            var actionResult = await controller.GetRecentReadingGoals(userId);

            
            var result = Assert.IsType<OkObjectResult>(actionResult);
            var goal = Assert.IsType<V1ReadingGoals>(result.Value);
            Assert.Equal(readingGoal2.Id, goal.Id);
        }

        [Fact]
        public async Task GetRecentReadingGoalsReturnsNotFoundWhenNoGoalFound()
        {
           
            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ReadingGoalsContext(options);

            var userId = Guid.NewGuid();

            var controller = new V1ReadingGoalsController(dbContext);

            
            var actionResult = await controller.GetRecentReadingGoals(userId);

           
            var result = Assert.IsType<NotFoundObjectResult>(actionResult);
            Assert.Equal("No Goals Yet", result.Value);
        }
        [Fact]
        public async Task IncrementReadingGoalReturnsOkWithIncrementedReadingGoal()
        {
            
            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ReadingGoalsContext(options);

            var readingGoal = new V1ReadingGoals
            {
                GoalCurrent = 10,
                LastUpdated = DateTime.UtcNow.AddDays(-1)
            };
            await dbContext.ReadingGoals.AddAsync(readingGoal);
            await dbContext.SaveChangesAsync();

            var controller = new V1ReadingGoalsController(dbContext);

            
            var actionResult = await controller.IncrementReadingGoal(readingGoal.Id, 5);

          
            var result = Assert.IsType<OkResult>(actionResult);
            Assert.Equal(15, readingGoal.GoalCurrent);
            Assert.True(readingGoal.LastUpdated > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public async Task IncrementReadingGoalReturnsNotFoundWhenGoalNotFound()
        {
            
            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ReadingGoalsContext(options);

            var controller = new V1ReadingGoalsController(dbContext);

            
            var actionResult = await controller.IncrementReadingGoal(Guid.NewGuid(), 5);

           
            var result = Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task ModifyReadingGoalReturnsOkWithUpdatedReadingGoal()
        {
            
            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ReadingGoalsContext(options);

            
            await dbContext.ReadingGoals.AddAsync(readingGoal2);
            await dbContext.SaveChangesAsync();

           

            var controller = new V1ReadingGoalsController(dbContext);

            
            var actionResult = await controller.ModifyReadingGoal(readingGoal2.Id, update);

            
            var result = Assert.IsType<OkResult>(actionResult);
            Assert.Equal(update.GoalStartDate, readingGoal2.GoalStartDate);
            Assert.Equal(update.GoalEndDate, readingGoal2.GoalEndDate);
            Assert.Equal(update.GoalTarget, readingGoal2.GoalTarget);
            Assert.True(readingGoal2.LastUpdated > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public async Task ModifyReadingGoalReturnsNotFoundWhenGoalNotFound()
        {
           
            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ReadingGoalsContext(options);

           

            var controller = new V1ReadingGoalsController(dbContext);

            
            var actionResult = await controller.ModifyReadingGoal(Guid.NewGuid(), readingGoal1);

            
            var result = Assert.IsType<NotFoundObjectResult>(actionResult);
        }
        [Fact]
        public async Task DeleteReadingGoal_ReturnsOkWhenReadingGoalDeleted()
        {
            
            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ReadingGoalsContext(options);

            await dbContext.ReadingGoals.AddAsync(readingGoal4);
            await dbContext.SaveChangesAsync();

            var controller = new V1ReadingGoalsController(dbContext);

            
            var actionResult = await controller.DeleteReadingGoal(readingGoal4.Id);

            
            var result = Assert.IsType<OkResult>(actionResult);

            var deletedReadingGoal = await dbContext.ReadingGoals.FindAsync(readingGoal4.Id);
            Assert.Null(deletedReadingGoal);
        }

        [Fact]
        public async Task DeleteReadingGoal_ReturnsNotFoundWhenGoalNotFound()
        {
            
            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ReadingGoalsContext(options);

            var controller = new V1ReadingGoalsController(dbContext);

            
            var actionResult = await controller.DeleteReadingGoal(Guid.NewGuid());

            
            var result = Assert.IsType<NotFoundObjectResult>(actionResult);
        }

        */

    }

}