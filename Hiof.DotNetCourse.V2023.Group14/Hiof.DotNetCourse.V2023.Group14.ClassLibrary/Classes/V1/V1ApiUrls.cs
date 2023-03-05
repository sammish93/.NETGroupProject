using System;
namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    // Class used to get the urls in the appsettings.json file in ProxyService.
    public class V1ApiUrls
    {
        // Urls for users
        public string GetUsers { get; set; }
        public string GetUserById { get; set; }
        public string GetUserByName { get; set; }
        public string GetUserByEmail { get; set; }
        public string CreateUserAccount { get; set; }
        public string UpdateUserAccount { get; set; }
        public string Delete { get; set; }
        public string DeleteByUsername { get; set; }

        // Urls for books.
        public string GetBookByIsbn { get; set; }
        public string GetBookByTitle { get; set; }
        public string GetBookByAuthor { get; set; }
        public string GetBookByCategory { get; set; }

        // Urls for library

    }

}

