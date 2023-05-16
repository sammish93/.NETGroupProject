using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels
{
    /// <summary>
    /// Model that holds the participants and the messages between them.
    /// </summary>
    [Table("conversations", Schema = "dbo")]
    public class V1ConversationModel
	{
        [Key]
        [Column("ConversationId", TypeName = "char(36)")]
        public Guid ConversationId { get; set; }

        // Here is a list of participants.
		public List<V1Participant> Participants { get; set; }

        [NotMapped]
        public List<V1UserWithDisplayPicture>? ParticipantsAsObjects { get; set; }

        public List<V1Messages> Messages { get; set; }

        [NotMapped]
        public V1Messages? LastMessage { get; set; }
    }
}

