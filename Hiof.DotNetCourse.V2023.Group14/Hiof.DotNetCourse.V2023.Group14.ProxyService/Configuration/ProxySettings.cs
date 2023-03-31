﻿using System;
namespace Hiof.DotNetCourse.V2023.Group14.ProxyService.Configuration
{
    public class ProxySettings
    {
        // Url for login
        public string LoginVerification { get; set; }

        // Urls for users
        public string GetUsers { get; set; }
        public string GetUserById { get; set; }
        public string GetUserByName { get; set; }
        public string GetUsersByName { get; set; }
        public string GetUsersByCity { get; set; }
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
        public string GetUserMostRecentBooks { get; set; }
        public string GetUserHighestRatedBooks { get; set; }
        public string LibraryDeleteEntry { get; set; }
        public string LibraryDeleteUserLibrary { get; set; }
        public string LibraryChangeRating { get; set; }
        public string LibraryChangeDateRead { get; set; }
        public string LibraryChangeReadingStatus { get; set; }

        // Urls for icons
        public string GetIconById { get; set; }
        public string GetIconByName { get; set; }
        public string AddIcon { get; set; }
        public string UpdateIcon { get; set; }
        public string DeleteIcon { get; set; }

        //Urls for readingGoals

        public string CreateReadingGoal { get; set; }

        public string GetAllGoals { get; set; }
        public string GetGoalId { get; set; }
        public string GetRecentGoal { get; set;}

        public string IncrementReadingGoal { get; set; }
        public string ModifyReadingGoal { get; set; }
        public string DeleteReadingGoal { get; set; }
    }
}

