using System;
namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels
{
	/// <summary>
	/// Model that holds the participants and the messages between them.
	/// </summary>
	public class V1ConversationModel
	{
		public Guid ConversationId { get; set; }

		public List<string> Participants { get; set; }

		public List<V1Messages> Messages { get; set; }
	}
}

