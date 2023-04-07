using System;
namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels
{
	public class V1Reactions
	{
        public Guid ReactionId { get; set; }

        public Guid MessageId { get; set; }

        public ReactionType Type { get; set; }
    }
}

