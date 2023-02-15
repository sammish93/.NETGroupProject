using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes
{
    // I (Sam) haven't fully defined this class. There may be possible issues with db transactions because of private or readonly values. I also haven't annotated the fields.
    // I also haven't created a table in the database. Read Info.txt in UserAccountService, as well as the test classes and comments in that project beforehand.
    [Table("Users", Schema = "dbo")]
    public class User
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set ; }
        // Assume that a user cannot change their username. This is the simplest way of maintaining integrity in our database during our first few sprints.
        [Required]
        [Column("UserName")]
        public string UserName { get; set; }
        [Column("email")]
        public string Email { get; set; }
        // The password should be hashed using SHA256 or higher.
        [Column("password")]
        public string Password { get ; set ; }
        [Column("FirstName")]
        public string FirstName { get ; set ; }
        [Column("lastName")]
        public string LastName { get ; set ; }
        [Column("Country")]
        public string Country { get ; set ; }
        // City and town are interchangeable in this case. Assume that the user will just pick the town they want to be associated with for finding nearby users and events.
        [Column("City")]
        public string City { get; set ; }
        // A two letter language code that we can use for localisation. 'EN' is English, and 'NO' is Norwegian.
        [Column("LangPreference")]
        public string LangPreference { get; set; }
        // Used for establishing permissions and a user hierarchy. A 'User' can only view and edit their own data, whereas an 'Admin' can view and edit all data.
        // Decided against a single user having multiple roles (using ISet or ICollection) because a hierarchy of increasing privileges makes more sense.
        [Column("UserRole")]
        public UserRole Role { get ; set ; }
        // Assume for localisation purposes that Coordinated Universal Time (UTC) will be used to always store information relating to DateTime, but DateTime.Now converts UTC to
        // the user's Local time, and we can use the latter in the GUI.

        [Column("RegistrationDate", TypeName = "date")]
        // Shouldn't be able to set a new date of registration.
        public DateTime RegistrationDate { get; set; }
        [Column("LastActive", TypeName = "date")]
        public DateTime LastActive { get; set; }

        public User(string userName, string email, string password, string firstName, string lastName, string country, string city, string langPreference, UserRole role)
        {
            Id = Guid.NewGuid();
            UserName = userName;
            Email = email;
            // This will store a hash that uses a salt stored in secrets.
            // NOTE: The password won't be implemented like this. We can create a method.
            // public string createSha256(string password) {
            //     string salt = <salt from secrets>; 
            //     string saltedPassword = salt + password;
            //     return ShaCreator(saltedPassword);
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
        //public User() { }


        // Getters and Setters.
       

        
        
       
       
       
        

        
      
    }
}