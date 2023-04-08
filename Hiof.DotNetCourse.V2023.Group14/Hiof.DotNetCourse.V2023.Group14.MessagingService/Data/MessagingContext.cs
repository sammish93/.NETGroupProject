using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
                .HasConstraintName("FK_messages_conversations");

            modelBuilder.Entity<V1Reactions>()
                .HasOne<V1Messages>()
                .WithMany(m => m.Reactions)
                .HasForeignKey(m => m.MessageId)
                .HasConstraintName("FK_reactions_messages");
        }

        public DbSet<V1ConversationModel> ConversationModel { get; set; }

		public DbSet<V1Messages> Messages { get; set; }

		public DbSet<V1Reactions> MessageReaction { get; set; }

		public DbSet<V1Participant> Participant { get; set; }
	}
}

