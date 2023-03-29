using System.Security.Cryptography;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;


namespace Hiof.DotNetCourse.V2023.Group14.ReadingGoalService.Data
{
    public class ReadingGoalsContext : DbContext
    {
        public ReadingGoalsContext(DbContextOptions<ReadingGoalsContext> options) : base(options) { }

        public DbSet<V1ReadingGoals> ReadingGoals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<V1ReadingGoals>().HasData(
                new
                {
                    Id = Guid.Parse("1dd99ec1-aaae-4df5-b9e2-fc233a9e1dd9"),
                    UserId = Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA6"),
                    GoalStartDate = DateTime.Parse("2023-02-21T07:43:11.453"),
                    GoalEndDate = DateTime.Parse("2023-03-21T07:43:11.453"),
                    GoalTarget = 20,
                    GoalCurrent = 5,
                    LastUpdated = DateTime.Parse("2023-02-21T07:43:11.453")

                },
                 new
                 {
                     Id = Guid.Parse("3b6c06da-250e-4f7a-b867-facbc6a48574"),
                     UserId = Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA6"),
                     GoalStartDate = DateTime.Parse("2022-10-21T07:43:11.453"),
                     GoalEndDate = DateTime.Parse("2022-12-21T07:43:11.453"),
                     GoalTarget = 20,
                     GoalCurrent = 17,
                     LastUpdated = DateTime.Parse("2023-02-21T07:43:11.453")

                 },
                  new
                  {
                      Id = Guid.Parse("c6050d5c-8b0a-402a-a020-7c2c4e82584a"),
                      UserId = Guid.Parse("54af86bf-346a-4cba-b36f-527748e1cb93"),
                      GoalStartDate = DateTime.Parse("2022-05-21T07:43:11.453"),
                      GoalEndDate = DateTime.Parse("2022-07-21T07:43:11.453"),
                      GoalTarget = 15,
                      GoalCurrent = 1,
                      LastUpdated = DateTime.Parse("2023-06-21T07:43:11.453")

                  },
                  new
                  {
                      Id = Guid.Parse("273b0fd5-cf41-4089-8611-f41d8099992c"),
                      UserId = Guid.Parse("54af86bf-346a-4cba-b36f-527748e1cb93"),
                      GoalStartDate = DateTime.Parse("2023-01-21T07:43:11.453"),
                      GoalEndDate = DateTime.Parse("2023-04-21T07:43:11.453"),
                      GoalTarget = 20,
                      GoalCurrent = 7,
                      LastUpdated = DateTime.Parse("2023-02-21T11:54:29.123")

                  }

                );
        }
      
    }
}
