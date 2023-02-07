using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes
{
    public class User
    {
        private Guid _id;
        // Assume that a user cannot change their username. This is the simplest way of maintaining integrity in our database during our first few sprints.
        private readonly string _username;
        private string _email;
        // The password should be hashed using SHA256 or higher.
        private string _password;
        private string _firstName;
        private string _lastName;
        private string _country;
        // City and town are interchangeable in this case. Assume that the user will just pick the town they want to be associated with for finding nearby users and events.
        private string _city;
        // A two letter language code that we can use for localisation. 'EN' is English, and 'NO' is Norwegian.
        private string _langPreference;
        // Used for establishing permissions and a user hierarchy. A 'User' can only view and edit their own data, whereas an 'Admin' can view and edit all data.
        // Decided against a single user having multiple roles (using ISet or ICollection) because a hierarchy of increasing privileges makes more sense.
        private UserRole _role;
        // Assume for localisation purposes that Coordinated Universal Time (UTC) will be used to always store information relating to DateTime, but DateTime.Now converts UTC to
        // the user's Local time, and we can use the latter in the GUI.
        private readonly DateTime _registrationDate;
        private DateTime _lastActive;

        public User(string username, string email, string password, string firstName, string lastName, string country, string city, string langPreference, UserRole role)
        {
            _id = Guid.NewGuid();
            _username = username;
            _email = email;
            // This will store a hash that uses a salt stored in secrets.
            // NOTE: The password won't be implemented like this. We can create a method.
            // public string createSha256(string password) {
            //     string salt = <salt from secrets>; 
            //     string saltedPassword = salt + password;
            //     return ShaCreator(saltedPassword);
            _password = password;
            _firstName = firstName;
            _lastName = lastName;
            _country = country;
            _city = city;
            _langPreference = langPreference;
            _role = role;
            _registrationDate = DateTime.UtcNow;
            _lastActive = DateTime.UtcNow;
        }

        // Getters and Setters.
        public Guid Id { get => _id; set => _id = value; }
        public string Username => _username;
        public string Email { get => _email; set => _email = value; }
        public string Password { get => _password; set => _password = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string Country { get => _country; set => _country = value; }
        public string City { get => _city; set => _city = value; }
        public string LangPreference { get => _langPreference; set => _langPreference = value; }
        public UserRole Role { get => _role; set => _role = value; }
        // Shouldn't be able to set a new date of registration.
        public DateTime RegistrationDate  => _registrationDate;
        public DateTime LastActive { get => _lastActive; set => _lastActive = value; }
    }
}