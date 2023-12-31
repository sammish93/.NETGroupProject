﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels
{
	[Table("participants", Schema = "dbo")]
    [PrimaryKey("Participant", "ConversationId")]
    public class V1Participant
	{
        public string Participant { get; set; }

        public bool IsRead { get; set; }

        [JsonIgnore]
        [ForeignKey("ConversationId")]
        [Column("ConversationId", TypeName = "char(36)")]
        public Guid ConversationId { get; set; }
	}
}

