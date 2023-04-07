using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels
{
    [Table("user_messages", Schema = "dbo")]
    public class V1Reactions
	{
        [Key]
        public Guid ReactionId { get; set; }

        public Guid MessageId { get; set; }

        public ReactionType Type { get; set; }
    }
}

