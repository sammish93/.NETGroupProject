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

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7021/api/1.0";

        private string _username;
        private string _password;
        private bool _isLoggingIn;
        private bool _isSuccessLabelVisible;

        

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

        public bool IsSuccessLabelVisible
        {
            get => _isSuccessLabelVisible;
            set => SetProperty(ref _isSuccessLabelVisible, value);
        }

        public ICommand LoginCommand => new Command(async () => await LoginAsync());

        private async Task LoginAsync()
        {
            IsLoggingIn = true;

            // IsSuccessLabelVisible = false;

            try
            {
                string loginUrl = $"{_apiBaseUrl}/login/verification";
                var requestBody = new { Username = Username, Password = Password };
                var requestBodyJson = JsonConvert.SerializeObject(requestBody);
                var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(loginUrl, requestContent);

                if (response.IsSuccessStatusCode)

                {
                    // IsSuccessLabelVisible = true;
                   // Preferences.Set("UserIsLoggedIn", true);

                    loginUrl = $"{_apiBaseUrl}/users/getUserByUserName?userName={Username}";

                    HttpResponseMessage result = await _httpClient.GetAsync(loginUrl);
                    var responseString = await result.Content.ReadAsStringAsync();

                    V1User user = JsonConvert.DeserializeObject<V1User>(responseString);

                    
                    Shell.Current.BindingContext = new AppShellViewModel(user);
                    await Shell.Current.GoToAsync("///home");
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();

                    // Dynamic does so the type is decieded at runtime.
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
        public ICommand SignupCommand => new Command(async () => await SignupAsync());

        private async Task SignupAsync()
        {
            await Shell.Current.GoToAsync("///signup");
        }

  
        private Task DisplayAlert(string v1, string v2, string v3)
        {
            return Task.CompletedTask;
        }


    }
}
