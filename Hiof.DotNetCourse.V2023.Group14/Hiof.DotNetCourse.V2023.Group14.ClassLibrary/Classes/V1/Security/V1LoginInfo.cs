using System;
using System.Text.Json.Serialization;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.Security
{
    // Class used to verify users in Login Service.
    public class V1LoginInfo
    {
        private string _username;
        private string _password;

        public V1LoginInfo(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public string UserName { get => _username; set => _username = value; }
        public string Password { get => _password; set => _password = value; }

    }
}

