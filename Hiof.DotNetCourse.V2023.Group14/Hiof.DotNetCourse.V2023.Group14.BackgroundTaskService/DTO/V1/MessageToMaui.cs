using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.DTO.V1
{
	public class MessageToMaui
	{
		public delegate void NewMessageRecievedHandler(object sender, List<V1Participant> newMessages);
		public event NewMessageRecievedHandler NewMessagesReceived;

        // This method will notify maui when the message checker background
        //job finds new messages in the database.
        public void OnNewMessagesReceived(List<V1Participant> newMessages)
        {
            NewMessagesReceived?.Invoke(this, newMessages);
        }
    }
}

