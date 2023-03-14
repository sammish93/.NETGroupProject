using System;
namespace Hiof.DotNetCourse.V2023.Group14.ProxyService.Configuration
{
	public class ProxySettings
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
        public string LibraryEntry { get; set; }
        public string LibraryGetEntries { get; set; }
        public string LibraryGetEntry { get; set; }
        public string GetUserLibrary { get; set; }
        public string LibraryDeleteEntry { get; set; }
        public string LibraryDeleteUserLibrary { get; set; }
        public string LibraryChangeRating { get; set; }
        public string LibraryChangeDateRead { get; set; }
        public string LibraryChangeReadingStatus { get; set; }
    }
}

