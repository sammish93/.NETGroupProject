
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
        V1ReadingGoals readingGoal3 = new V1ReadingGoals()
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
            var options = new DbContextOptionsBuilder<ReadingGoalsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }
    }
}