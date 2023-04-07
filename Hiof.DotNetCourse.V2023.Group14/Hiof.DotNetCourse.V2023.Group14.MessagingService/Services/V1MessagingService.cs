using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;

namespace Hiof.DotNetCourse.V2023.Group14.MessagingService.Services
{
	public class V1MessagingService : V1IMessages
	{
		public V1MessagingService()
		{
		}

        public Task AddMessageToConversation(Guid conversationId, Guid messageId, string message)
        {
            throw new NotImplementedException();
        }

        public Task CreateNewConversation(Guid conversationId, IEnumerable<string> participants)
        {
            throw new NotImplementedException();
        }

        public Task DeleteConversation(Guid conversationId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMessage(Guid conversationId, Guid messageId)
        {
            throw new NotImplementedException();
        }

        public Task<V1ConversationModel> GetByConversationId(Guid participantId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<V1ConversationModel>> GetByParticipant(string participant)
        {
            throw new NotImplementedException();
        }

        public Task UpdateExistingMessage(Guid conversationId, Guid messageId, string message)
        {
            throw new NotImplementedException();
        }
    }
}

