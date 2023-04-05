using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

using CommunityToolkit.Mvvm.Input;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Diagnostics;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private V1User _loggedInUser { get; set; }
        private byte[] _selectedUserDisplayPicture { get; set; }
        private string _username;
        private string _password;
        private string _email;
        private string _firstName;
        private string _lastName;
        private string _country;
        private string _city;
        private string _langpref;
        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public V1User LoggedInUser
        {
            get => _loggedInUser;
            set
            {
                _loggedInUser = value;
                OnPropertyChanged();
            }
        }

        public byte[] SelectedUserDisplayPicture
        {
            get => _selectedUserDisplayPicture;
            set
            {
                _selectedUserDisplayPicture = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(20, ErrorMessage = "{0} must be between {2} and {1}.", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9]+$",
        ErrorMessage = "Only alphanumeric characters in username")]
        public string UserName
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public string FirstName { get => _firstName; set => SetProperty(ref _firstName, value); }
        public string LastName { get => _lastName; set => SetProperty(ref _lastName, value); }
        public string Country { get => _country; set => SetProperty(ref _country, value); }
        public string City { get => _city; set => SetProperty(ref _city, value); }
        public string Lang_Preference { get => _langpref; set => SetProperty(ref _langpref, value); }

        public SettingsPageViewModel(V1User user)
        {
            LoggedInUser = user;
        }

        public ICommand SignUpCommand => new Command(async () => await SignupAsync());
        public async Task SignupAsync()
        {
            try
            {
                IsBusy = true;

                string signUpUrl = $"{_apiBaseUrl}/users/CreateUserAccount";
                var requestBody = new V1User
                {
                    Id = Guid.NewGuid(),
                    UserName = UserName,
                    Email = Email,
                    Password = Password,
                    FirstName = FirstName,
                    LastName = LastName,
                    Country = Country,
                    City = City,
                    LangPreference = Lang_Preference,
                    Role = UserRole.User,
                    RegistrationDate = DateTime.UtcNow,
                    LastActive = DateTime.UtcNow

                };
                var requestBodyJson = JsonConvert.SerializeObject(requestBody);
                var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(signUpUrl, requestContent);

                IsBusy = false;

                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.GoToAsync("///login");

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public ICommand SaveCommand => new Command(async () => await SaveAsync());

        private async Task SaveAsync()
        {
            string content = "";
            StringBuilder errorMessage = new StringBuilder();

            try
            {
                IsBusy = true;

                string url = $"{_apiBaseUrl}/users/UpdateAccountById";

                var userChanged = new V1User
                {
                    Id = LoggedInUser.Id,
                    UserName = UserName,
                    Email = Email,
                    Password = Password,
                    FirstName = FirstName,
                    LastName = LastName,
                    Country = Country,
                    City = City,
                    LangPreference = Lang_Preference,
                    Role = LoggedInUser.Role,
                    RegistrationDate = LoggedInUser.RegistrationDate,
                    LastActive = LoggedInUser.LastActive

                };
                var requestBodyJson = JsonConvert.SerializeObject(userChanged);
                var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, requestContent);

                IsBusy = false;

                if (response.IsSuccessStatusCode)
                {
                    App.LoggedInUser = userChanged;
                    App.IsUserLibraryAltered = true;
                    await Application.Current.MainPage.DisplayAlert("Success!", "Your changes have been saved.", "OK");
                    await Shell.Current.GoToAsync("///home");

                } else
                {
                    content = await response.Content.ReadAsStringAsync();

                    dynamic json = JsonConvert.DeserializeObject(content);

                    foreach (var error in json.errors)
                    {
                        errorMessage.Append($"{error.Value[0]}\n");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(content, "Some of the fields you entered are of the incorrect format.", "OK");
                Debug.WriteLine(ex);
            }
        }

        private void PopulateEntries(V1User user)
        {
            UserName = user.UserName;
            Password = user.Password;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Country = user.Country;
            City = user.City;
            Lang_Preference = user.LangPreference;
    }

        public async Task LoadAsync(V1User user)
        {
            IsBusy = true;
            PopulateEntries(user);
            IsBusy = false;
        }
    }
}
