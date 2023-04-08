using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels
{
    /// <summary>
    /// Model the represent the messages sent between users.
    /// </summary>
    [Table("messages", Schema = "dbo")]
    public class V1Messages
	{
		[Key]
		public Guid MessageId { get; set; }

		public string Sender { get; set; }

		public string Message { get; set; }

		public DateTime Date { get; set; }

        public List<V1Reactions> Reactions { get; set; }

        [JsonIgnore]
        public Guid ConversationId { get; set; }

	}
}

