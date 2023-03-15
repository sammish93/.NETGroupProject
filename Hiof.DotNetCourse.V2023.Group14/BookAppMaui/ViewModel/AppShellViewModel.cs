using System;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using System.Windows.Input;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class AppShellViewModel : BaseViewModel
    {
        private V1User _user;
        private Guid _userId;
        private string _userName;
        private string _email;
        private string _firstName = "defaultNameNotSet";
        private string _lastName;
        private string _country;
        private string _city;
        private string _langPreference;
        private UserRole _userRole;
        private DateTime _registrationDate;
        private DateTime _lastActive;

        public V1User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        public Guid UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }
        public String UserName
        {
            get => _userName;
            set => SetProperty(ref _firstName, value);
        }
        public String Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public String FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }
        public String LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        public String Country
        {
            get => _country;
            set => SetProperty(ref _country, value);
        }
        public String City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }
        public String LangPreference
        {
            get => _langPreference;
            set => SetProperty(ref _langPreference, value);
        }
        public UserRole UserRole
        {
            get => _userRole;
            set => SetProperty(ref _userRole, value);
        }
        public DateTime RegistrationDate
        {
            get => _registrationDate;
            set => SetProperty(ref _registrationDate, value);
        }
        public DateTime LastActive
        {
            get => _lastActive;
            set => SetProperty(ref _lastActive, value);
        }


        public AppShellViewModel()
        {

        }

        public AppShellViewModel(V1User user)
        {
            User = user;
            UserId = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Country = user.Country;
            City = user.City;
            LangPreference = user.LangPreference;
            UserRole = user.Role;
            RegistrationDate = user.RegistrationDate;
            LastActive = user.LastActive;
        }
    }
}
