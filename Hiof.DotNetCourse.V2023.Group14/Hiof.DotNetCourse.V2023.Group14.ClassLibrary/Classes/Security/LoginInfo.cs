using System;
using System.Text.Json.Serialization;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Security
{
	// Class used to verify users in Login Service.
	public class LoginInfo
	{
		private int _id;
		private string? _token;
		private string _username;
		private string _password;


		public LoginInfo(int id, string username, string password)
		{
			_id = id;
			_username = username;
			_password = password;
		}

		public int Id { get => _id; }
		[JsonIgnore]
		public string? Token { get => _token; set => _token = value; }
		public string UserName { get => _username; set => _username = value; }
		public string Password { get => _password; set => _password = value; }
	}
}

