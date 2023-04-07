using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels
{
    /// <summary>
    /// Model that holds the participants and the messages between them.
    /// </summary>
    [Table("conversations", Schema = "dbo")]
    public class V1ConversationModel
	{
        [Key]
		public Guid ConversationId { get; set; }

		public List<V1Participant> Participants { get; set; }

		public List<V1Messages> Messages { get; set; }
    }
}

