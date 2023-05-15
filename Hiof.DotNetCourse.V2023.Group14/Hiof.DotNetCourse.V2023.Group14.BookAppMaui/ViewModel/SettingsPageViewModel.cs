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
using System.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private V1User _loggedInUser { get; set; }
        private byte[] _userDisplayPicture { get; set; }
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

        public byte[] UserDisplayPicture
        {
            get => _userDisplayPicture;
            set
            {
                _userDisplayPicture = value;
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

        public SettingsPageViewModel(V1User user, byte[] userDisplayPicture)
        {
            LoggedInUser = user;
            UserDisplayPicture = userDisplayPicture;
        }

        public ICommand UploadImageCommand => new Command(async () => await FileSelectorAsync(PickOptions.Images, LoggedInUser));
        public ICommand SaveCommand => new Command(async () =>
        {
            await UpdateDisplayPictureAsync(LoggedInUser, UserDisplayPicture);
            await SaveAsync();
        });

        // Saves changes made to a user's profile (name, location, password etc.), as well as changes to a user's display picture.
        // Changes are only saved once this method is called, even if the GUI shows a 'preview' of the new display picture etc.
        private async Task SaveAsync()
        {
            string content = "";
            StringBuilder errorMessage = new StringBuilder();

            try
            {
                IsBusy = true;

                string url = $"{_apiBaseUrl}/users/UpdateAccountById";

                // Creates a new V1User object based on entry form elements on the GUI.
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
                    // Updates the data pertaining to the user that is logged in after a successful change.
                    // Important in cases where a username or display picture has been changed, so all other pages are updated.
                    Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().LoggedInUser = userChanged;
                    Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().IsUserLibraryAltered = true;
                    await Application.Current.MainPage.DisplayAlert("Success!", "Your changes have been saved.", "OK");
                    // Redirects the user to the main page once a change has been successfully saved.
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

        // Populates entry forms in the GUI with the currently saved data of the logged in user.
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

        // Retrieves a display picture of a specific user. A default display picture is provided if no existing display picture is present.
        public async Task GetUserDisplayPictureAsync(V1User user)
        {
            string displayPictureUrl = $"{_apiBaseUrl}/icons/GetIconByName?username={user.UserName}";
            HttpResponseMessage resultDisplayPicture = await _httpClient.GetAsync(displayPictureUrl);

            if (resultDisplayPicture.IsSuccessStatusCode)
            {
                var responseStringDisplayPicture = await resultDisplayPicture.Content.ReadAsStringAsync();

                V1UserIcon displayPicture = JsonConvert.DeserializeObject<V1UserIcon>(responseStringDisplayPicture);

                UserDisplayPicture = displayPicture.DisplayPicture;
            }
            else
            {
                UserDisplayPicture = Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().DefaultDisplayPicture;
            }
        }

        // The GUI only currently supports .jpg and .png images, and these can only be deserialised and shown on Windows OS machines currently.
        public async Task<FileResult> FileSelectorAsync(PickOptions pickOptions, V1User user)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(pickOptions);
                if (result != null)
                {
                    if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    {
                        using var stream = await result.OpenReadAsync();
                        // Retrieves an image file and converts it into a byte stream which can then be easily saved to a database, as well as displayed
                        // in the GUI.
                        var newDisplayPicture = System.Drawing.Image.FromStream(stream);

                        ImageConverter converter = new ImageConverter();
                        byte[] displayPictureInBytes = (byte[])converter.ConvertTo(newDisplayPicture, typeof(byte[]));

                        UserDisplayPicture = displayPictureInBytes;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "The file is not a valid image.", "OK");
            }

            return null;
        }

        // Used to display the -updated- display picture throughout the life of the application. 
        // Note: the picture is already saved to the database, but this method updates the logged in user object without restarting the application.
        private async Task UpdateDisplayPictureAsync(V1User user, byte[] displayPicture)
        {
            try
            {
                IsBusy = true;

                Guid id = new Guid();

                string displayPictureUrl = $"{_apiBaseUrl}/icons/GetIconByName?username={user.UserName}";
                HttpResponseMessage resultDisplayPicture = await _httpClient.GetAsync(displayPictureUrl);

                if (resultDisplayPicture.IsSuccessStatusCode)
                {
                    var responseStringDisplayPicture = await resultDisplayPicture.Content.ReadAsStringAsync();

                    V1UserIcon picture = JsonConvert.DeserializeObject<V1UserIcon>(responseStringDisplayPicture);

                    id = picture.Id;
                }
                    
                string url = $"{_apiBaseUrl}/icons/Update";

                var icon = new V1UserIcon
                {
                    Id = id,
                    Username = user.UserName,
                    DisplayPicture = displayPicture
                };


                var requestBodyJson = JsonConvert.SerializeObject(icon);
                var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, requestContent);

                IsBusy = false;

                if (response.IsSuccessStatusCode)
                {
                    UserDisplayPicture = displayPicture;
                    Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().UserDisplayPicture = displayPicture;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "The file is not a valid image.", "OK"); 
                Debug.WriteLine(ex);
            }
        }

        public async Task LoadAsync(V1User user)
        {
            IsBusy = true;
            PopulateEntries(user);
            await GetUserDisplayPictureAsync(user);
            IsBusy = false;
        }
    }
}
