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

		public DbSet<V1ConversationModel> ConversationModel;

		public DbSet<V1Messages> Messages;

		public DbSet<V1Reactions> MessageReaction;

		public DbSet<V1Participant> Participant;
	}
}

