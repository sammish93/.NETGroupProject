using System;
namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels
{
	/// <summary>
	/// Model the represent the messages sent between users.
	/// </summary>
	public class V1Messages
	{
		public Guid MessageId { get; set; }

		public string Sender { get; set; }

		public string Message { get; set; }

		public DateTime Date { get; set; }

        public Guid ConversationId { get; set; }

        public List<V1Reactions> Reactions { get; set; }

	}
}

