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
        [Column("MessageId", TypeName = "char(36)")]
        public Guid MessageId { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Sender { get; set; }

        [NotMapped]
        public V1UserWithDisplayPicture? SenderObject { get; set; }

        [Required]
        [Column(TypeName = "varchar(1000)")]
        public string Message { get; set; }

		public DateTime Date { get; set; }

        public List<V1Reactions> Reactions { get; set; }

        [JsonIgnore]
        [Column("ConversationId", TypeName = "char(36)")]
        public Guid ConversationId { get; set; }

	}
}

