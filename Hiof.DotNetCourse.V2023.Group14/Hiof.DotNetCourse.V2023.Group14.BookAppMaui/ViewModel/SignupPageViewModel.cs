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
    public class SignupPageViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
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

        public ICommand SignUpCommand => new Command(async () => await SignupAsync());

        // Allows the user to fill entry forms, selection boxes, and other page elements, and then fetches the information and creates a V1User object which is then used 
        // to save data to a database by creating a new user entry.
        public async Task SignupAsync()
        {
            try
            {
                bool answer = false;
                // Prompt for a rudimentary explanation of some of the GDPR laws relating to data collection. 
                // Note: as of now, the GUI doesn't support users who aren't logged in, so data collection and agreement is necessary.
                answer = await Application.Current.MainPage.DisplayAlert("Our application stores personal information about our users." ,
                    "We store information entered by each user on account registration, " +
                    "as well as any comments or messages entered by users during use of the application. " +
                    "Do you agree to the storage of information listed above?", "Agree", "Disagree");

                if (answer)
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
                        // Redirects the user to the login page on a successful signup.
                        await Shell.Current.GoToAsync("///login");

                    }
                } else
                {
                    // Prompt that informs the user that as of right now there is no functionality available for anonymous/users not logged in to use the application.
                    await Application.Current.MainPage.DisplayAlert("Uh oh!", "Sadly we cannot provide a functional service to our users without agreeing to the collection " +
                        "of data. Functionality to provide users without accounts to access parts of our service is coming soon. Stay tuned!", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public ICommand BackCommand => new Command(async () => await BackAsync());

        // Navigates the user back to the login page.
        private async Task BackAsync()
        {
            await Shell.Current.GoToAsync("///login");
        }
    }
}
