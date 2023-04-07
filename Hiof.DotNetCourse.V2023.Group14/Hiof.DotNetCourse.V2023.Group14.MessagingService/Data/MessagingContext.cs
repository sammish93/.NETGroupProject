using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;
using Microsoft.EntityFrameworkCore;

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
        }

        public DbSet<V1ConversationModel> ConversationModel { get; set; }

		public DbSet<V1Messages> Messages { get; set; }

		public DbSet<V1Reactions> MessageReaction { get; set; }

		public DbSet<V1Participant> Participant { get; set; }
	}
}

