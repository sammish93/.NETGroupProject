using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1
{
	/// <summary>
	/// Contract for the V1MessagingController API.
	/// </summary>
	public interface V1IMessages
	{
		Task<V1ConversationModel?> GetByParticipant(string participant);

		Task<V1ConversationModel?> GetByConversationId(Guid participantId);

		Task AddMessageToConversation(Guid conversationId, string sender, string message);

		Task AddReactionToMessage(Guid messageId, ReactionType reaction);

		Task CreateNewConversation(Guid conversationId, IEnumerable<string> participants);

		Task UpdateMessage(Guid messageId, string message);

		Task DeleteMessage(Guid messageId);

		Task DeleteConversation(Guid conversationId);
	}
}

