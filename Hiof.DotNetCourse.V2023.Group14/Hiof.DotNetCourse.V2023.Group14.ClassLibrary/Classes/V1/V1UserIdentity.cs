using System;
using Microsoft.AspNetCore.Identity;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
	// Class to test the Identity library. I dont want to mess anything
	// up in V1User, so i made this test class.
	public class V1UserIdentity : IdentityUser
	{

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Password { get; set; }
	}
}

