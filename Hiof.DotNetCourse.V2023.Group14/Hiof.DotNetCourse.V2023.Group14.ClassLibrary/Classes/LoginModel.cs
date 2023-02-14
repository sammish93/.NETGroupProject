using System;
using System.ComponentModel.DataAnnotations;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes
{
	public class LoginClass
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string? UserName { get; set; }

		[Required]
		public string? Password { get; set; }

		[Required]
		public string? Token { get; set; }

		[Required]
        public string? Salt { get; set; }
    }
}

