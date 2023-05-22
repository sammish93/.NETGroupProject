using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using System.Text;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Threading;

namespace Hiof.DotNetCourse.V2023.Group14.MessagingService.Data
{
	public class MessagingContext : DbContext
	{
		public MessagingContext(DbContextOptions<MessagingContext> options) : base(options)
		{
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<V1ConversationModel>()
                .HasKey(c => c.ConversationId);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<V1Messages>()
                .HasOne<V1ConversationModel>()
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .HasConstraintName("FK_messages_conversations")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<V1Reactions>()
                .HasOne<V1Messages>()
                .WithMany(m => m.Reactions)
                .HasForeignKey(m => m.MessageId)
                .HasConstraintName("FK_reactions_messages")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<V1ConversationModel>().HasData(
                new
                {
                    ConversationId = Guid.Parse("34eef5ed-35be-408a-a562-95ea91f24fd1")
                },
                new
                {
                    ConversationId = Guid.Parse("3ad21e84-4ed0-4483-8ca7-1b29ae06fa00")
                });

            modelBuilder.Entity<V1Participant>().HasData(
                    new
                    {
                        Participant = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                        IsRead = false,
                        ConversationId = Guid.Parse("34eef5ed-35be-408a-a562-95ea91f24fd1")
                    },
                    new
                    {
                        Participant = "54af86bf-346a-4cba-b36f-527748e1cb93",
                        IsRead = true,
                        ConversationId = Guid.Parse("34eef5ed-35be-408a-a562-95ea91f24fd1")
                    },
                    new
                    {
                        Participant = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                        IsRead = true,
                        ConversationId = Guid.Parse("3ad21e84-4ed0-4483-8ca7-1b29ae06fa00")
                    },
                    new
                    {
                        Participant = "e8cc12ba-4df6-4b06-b96e-9ad00a927a93",
                        IsRead = true,
                        ConversationId = Guid.Parse("3ad21e84-4ed0-4483-8ca7-1b29ae06fa00")
                    }
                );

            modelBuilder.Entity<V1Messages>().HasData(
                new
                {
                    MessageId = Guid.Parse("71456bc4-16d4-4b26-9f55-9ce88fa179f6"),
                    Sender = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    Message = "hey!",
                    Date = DateTime.Parse("2023-05-19 11:13:28.8309960"),
                    ConversationId = Guid.Parse("34eef5ed-35be-408a-a562-95ea91f24fd1")
                },
                new
                {
                    MessageId = Guid.Parse("b24c9d48-79a3-4ef3-bb8e-a978ac4a7170"),
                    Sender = "54af86bf-346a-4cba-b36f-527748e1cb93",
                    Message = "Hello there :)",
                    Date = DateTime.Parse("2023-05-19 11:09:59.3995659"),
                    ConversationId = Guid.Parse("34eef5ed-35be-408a-a562-95ea91f24fd1")
                },
                new
                {
                    MessageId = Guid.Parse("d0d316e0-10f3-4460-8571-35c23c3ae15b"),
                    Sender = "54af86bf-346a-4cba-b36f-527748e1cb93",
                    Message = "nice to hear from you",
                    Date = DateTime.Parse("2023-05-19 11:15:09.7096212"),
                    ConversationId = Guid.Parse("34eef5ed-35be-408a-a562-95ea91f24fd1")
                },
                new
                {
                    MessageId = Guid.Parse("e3d9836a-208f-4e04-9e22-63b43e6f76e9"),
                    Sender = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    Message = "Hello!!",
                    Date = DateTime.Parse("2023-05-19 08:35:18.8946909"),
                    ConversationId = Guid.Parse("3ad21e84-4ed0-4483-8ca7-1b29ae06fa00")
                });
        }

        public DbSet<V1ConversationModel> ConversationModel { get; set; }

		public DbSet<V1Messages> Messages { get; set; }

		public DbSet<V1Reactions> MessageReaction { get; set; }

		public DbSet<V1Participant> Participant { get; set; }
	}
}

