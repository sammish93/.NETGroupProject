using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    // I (Sam) haven't fully defined this class. There may be possible issues with db transactions because of private or readonly values. I also haven't annotated the fields.
    // I also haven't created a table in the database. Read Info.txt in UserAccountService, as well as the test classes and comments in that project beforehand.
    [Table("users", Schema = "dbo")]
    public class V1User : V1IUserValidator
    {
        [Key]
        [Column("id", TypeName = "char(36)")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(20, ErrorMessage = "{0} must be between {2} and {1}.", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9]+$",
         ErrorMessage = "Only alphanumeric characters in username")]
        [Column("username", TypeName = "nvarchar(500)")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Column("email", TypeName = "nvarchar(500)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]

        // The password should be hashed using SHA256 or higher.
        [Column("password", TypeName = "nvarchar(500)")]
        public string Password { get; set; }
        [Column("first_name", TypeName = "nvarchar(500)")]
        public string FirstName { get; set; }
        [Column("last_name", TypeName = "nvarchar(500)")]
        public string LastName { get; set; }
        [Column("country", TypeName = "nvarchar(500)")]
        public string Country { get; set; }
        // City and town are interchangeable in this case. Assume that the user will just pick the town they want to be associated with for finding nearby users and events.
        [Column("city", TypeName = "nvarchar(500)")]
        public string City { get; set; }
        // A two letter language code that we can use for localisation. 'EN' is English, and 'NO' is Norwegian.
        [Column("lang_preference", TypeName = "nvarchar(500)")]
        public string LangPreference { get; set; }
        // Used for establishing permissions and a user hierarchy. A 'User' can only view and edit their own data, whereas an 'Admin' can view and edit all data.
        // Decided against a single user having multiple roles (using ISet or ICollection) because a hierarchy of increasing privileges makes more sense.
        [Column("user_role", TypeName = "nvarchar(20)")]
        public UserRole Role { get; set; }
        // Assume for localisation purposes that Coordinated Universal Time (UTC) will be used to always store information relating to DateTime, but DateTime.Now converts UTC to
        // the user's Local time, and we can use the latter in the GUI.

        [Column("registration_date", TypeName = "datetime")]
        // Shouldn't be able to set a new date of registration.
        public DateTime RegistrationDate { get; set; }
        [Column("last_active", TypeName = "datetime")]
        public DateTime LastActive { get; set; }

        public V1User(string userName, string email, string password, string firstName, string lastName, string country, string city, string langPreference, UserRole role)
        {
            Id = Guid.NewGuid();
            UserName = userName;
            Email = email;
            // This will store a hash that uses a salt stored in secrets.
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Country = country;
            City = city;
            LangPreference = langPreference;
            Role = role;
            RegistrationDate = DateTime.UtcNow;
            LastActive = DateTime.UtcNow;
        }

        public V1User()
        {
        }

        // Email must be of a valid format.
        public static bool ValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$");
        }

        // Password must have at least one lower-case letter, one upper-case letter, one number, and one special character, and be at least 5 characters long.
        public static bool ValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,20}$");
        }

        // Username must be alphanumeric and between 5 and 20 characters.
        public static bool ValidUsername(string username)
        {
            return Regex.IsMatch(username, @"^[a-zA-Z0-9].{4,20}$");
        }
    }
}