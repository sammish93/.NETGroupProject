﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels
{
	public class V1Participant
	{
		public string Participant { get; set; }
		[Key]
		public Guid ConversationId { get; set; }
	}
}

