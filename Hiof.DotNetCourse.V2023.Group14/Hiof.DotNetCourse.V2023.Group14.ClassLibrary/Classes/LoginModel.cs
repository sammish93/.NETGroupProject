using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes
{
	public class LoginModel
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string? UserName { get; set; }

		[Required]
		public string? Password { get; set; } 

		[JsonIgnore]
		public string? Token { get; set; }

		[JsonIgnore]
        public string? Salt { get; set; }
    }
}

