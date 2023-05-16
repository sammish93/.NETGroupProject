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
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Drawing;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";

        private string _username;
        private string _password;
        private bool _isLoggingIn;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsLoggingIn
        {
            get => _isLoggingIn;
            set => SetProperty(ref _isLoggingIn, value);
        }


        public ICommand LoginCommand => new Command(async () => await LoginAsync());

        // Retrieves a username and password combination from entry fields and passes them to an API call that hashes the password and validates that username and 
        // password combination exists. If true then the login attempt is successful, and a user is redirected to the main page.
        private async Task LoginAsync()
        {
            IsLoggingIn = true;

            try
            {
                string loginUrl = $"{_apiBaseUrl}/login/Verification";
                var requestBody = new { Username = Username, Password = Password };
                var requestBodyJson = JsonConvert.SerializeObject(requestBody);
                var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(loginUrl, requestContent);

                if (response.IsSuccessStatusCode)

                {
                    loginUrl = $"{_apiBaseUrl}/users/getByName?name={Username}";

                    HttpResponseMessage result = await _httpClient.GetAsync(loginUrl);
                    var responseString = await result.Content.ReadAsStringAsync();

                    V1User user = JsonConvert.DeserializeObject<V1User>(responseString);

                    var currentViewModel = Shell.Current.BindingContext as AppShellViewModel;
                    Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().LoggedInUser= user;

                    string displayPictureUrl = $"{_apiBaseUrl}/icons/GetIconByName?username={user.UserName}";
                    HttpResponseMessage resultDisplayPicture = await _httpClient.GetAsync(displayPictureUrl);

                    // Note: Very clunky solution that can be fixed in future versions when we have more time.
                    // This code only works on Windows OS, and retrieves both a user's display picture (if it exists), as well as the default display picture 
                    // from the resources folder.
                    var defaultDisplayPicture = System.Drawing.Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "/../../../../../Resources/Images/default_display_picture.png");
                    ImageConverter converter = new ImageConverter();
                    byte[] displayPictureInBytes = (byte[])converter.ConvertTo(defaultDisplayPicture, typeof(byte[]));
                    Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().DefaultDisplayPicture = displayPictureInBytes;
                    Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedUserDisplayPicture = displayPictureInBytes;

                    if (resultDisplayPicture.IsSuccessStatusCode) 
                    {
                        var responseStringDisplayPicture = await resultDisplayPicture.Content.ReadAsStringAsync();

                        V1UserIcon displayPicture = JsonConvert.DeserializeObject<V1UserIcon>(responseStringDisplayPicture);

                        // Updates when the user last logged in and passes it to a method that includes an API call to update the database.
                        user.LastActive = DateTime.Now;

                        await UpdateUserAsync(user);

                        Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().UserDisplayPicture = displayPicture.DisplayPicture;
                        Shell.Current.BindingContext = new AppShellViewModel(user, displayPicture.DisplayPicture);
                    } else
                    {
                        Shell.Current.BindingContext = new AppShellViewModel(user, displayPictureInBytes);
                    }

                    // Prompt is displayed if the user currently doesn't have full internet access. This mostly affects retrieval from the external Google Books API.
                    if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                    {
                        await Application.Current.MainPage.DisplayAlert("No Internet Connection!", "You don't seem to be connected to the internet. Please be aware that " +
                            "some services may be unavailable or won't function as intended.", "OK");
                    }

                    // User is redirected to the main page.
                    await Shell.Current.GoToAsync("///home");
                } else
                {
                    string content = await response.Content.ReadAsStringAsync();

                    dynamic json = JsonConvert.DeserializeObject(content);

                    StringBuilder errorMessage = new StringBuilder();
                    foreach (var error in json.errors)
                    {
                        // Therefore we can append the right errormessage at runtime.
                        errorMessage.Append($"{error.Value[0]}\n");
                    }

                    await Application.Current.MainPage.DisplayAlert("Error", errorMessage.ToString(), "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Login Error", ex.Message, "OK");
            }
            finally
            {
                IsLoggingIn = false;
            }
        }

        // Updates the user in the database (date last active).
        public async Task UpdateUserAsync(V1User user)
        {
            var url = $"{_apiBaseUrl}/users/UpdateAccountById";

            var jsonString = JsonConvert.SerializeObject(user);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, httpContent);
        }

        public ICommand SignupCommand => new Command(async () => await SignupAsync());

        // Redirects new users to a sign-up page.
        private async Task SignupAsync()
        {
            await Shell.Current.GoToAsync("signup");
        }

        private Task DisplayAlert(string v1, string v2, string v3)
        {
            return Task.CompletedTask;
        }
    }
}
