using System;
namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    // Class used to get the urls in the appsettings.json file in ProxyService.
    public class V1UserAccountApiUrls
    {
        public string GetUsers { get; set; }
        public string GetUserById { get; set; }
        public string GetUserByName { get; set; }
        public string GetUserByEmail { get; set; }
        public string CreateUserAccount { get; set; }
    }

}

