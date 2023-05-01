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
                    //Preferences.Set("UserIsLoggedIn", true);
                    loginUrl = $"{_apiBaseUrl}/users/getByName?name={Username}";

                    HttpResponseMessage result = await _httpClient.GetAsync(loginUrl);
                    var responseString = await result.Content.ReadAsStringAsync();

                    V1User user = JsonConvert.DeserializeObject<V1User>(responseString);

                    var currentViewModel = Shell.Current.BindingContext as AppShellViewModel;
                   // UserSingleton.Instance.SetUser(user, true);
                    Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().LoggedInUser= user;

                    string displayPictureUrl = $"{_apiBaseUrl}/icons/GetIconByName?username={user.UserName}";
                    HttpResponseMessage resultDisplayPicture = await _httpClient.GetAsync(displayPictureUrl);


                    var defaultDisplayPicture = System.Drawing.Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "/../../../../../Resources/Images/default_display_picture.png");
                    ImageConverter converter = new ImageConverter();
                    byte[] displayPictureInBytes = (byte[])converter.ConvertTo(defaultDisplayPicture, typeof(byte[]));
                    Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().DefaultDisplayPicture = displayPictureInBytes;
                    Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedUserDisplayPicture = displayPictureInBytes;

                    if (resultDisplayPicture.IsSuccessStatusCode) 
                    {
                        var responseStringDisplayPicture = await resultDisplayPicture.Content.ReadAsStringAsync();

                        V1UserIcon displayPicture = JsonConvert.DeserializeObject<V1UserIcon>(responseStringDisplayPicture);

                        user.LastActive = DateTime.Now;

                        await UpdateUser(user);

                        Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().UserDisplayPicture = displayPicture.DisplayPicture;
                        Shell.Current.BindingContext = new AppShellViewModel(user, displayPicture.DisplayPicture);
                    } else
                    {
                        Shell.Current.BindingContext = new AppShellViewModel(user, displayPictureInBytes);
                    }

                    if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                    {
                        await Application.Current.MainPage.DisplayAlert("No Internet Connection!", "You don't seem to be connected to the internet. Please be aware that " +
                            "some services may be unavailable or won't function as intended.", "OK");
                    }

                    await Shell.Current.GoToAsync("///home");
                } else
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

        public async Task UpdateUser(V1User user)
        {
            var url = $"{_apiBaseUrl}/users/UpdateAccountById";

            var jsonString = JsonConvert.SerializeObject(user);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, httpContent);
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
