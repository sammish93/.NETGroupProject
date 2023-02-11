using System;
namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes
{
	// Class used to verify users in Login Service.
	public class LoginInfo
	{
		private Guid _id;
		private string _username;
		private string _password;
		private readonly string? _token;


		public LoginInfo(string username, string password, string token)
		{
			_id = Guid.NewGuid();
			_username = username;
			_password = password;
			_token = token;
		}

		public Guid Id { get => _id; }
		public string UserName { get => _username; set => _username = value; }
		public string Password { get => _password; set => _password = value; }
		public string? Token { get => _token; }
	}
}

