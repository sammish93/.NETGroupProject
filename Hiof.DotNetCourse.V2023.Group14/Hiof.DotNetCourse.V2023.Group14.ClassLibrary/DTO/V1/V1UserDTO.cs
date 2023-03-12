using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.DTO.V1
{
	// Got a circular reference problem when i tried to save all the users from
	// the database in the cachememory. This is why i have created this dto.

	public class V1UserDTO
	{
        public Guid Id { get; set; }
        public string Username { get; set; }
		public string Email { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastActive { get; set; }
    }
}

