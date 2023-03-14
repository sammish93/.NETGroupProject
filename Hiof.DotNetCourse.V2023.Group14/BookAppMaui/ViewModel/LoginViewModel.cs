using System;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using BookAppMaui;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using System.Windows.Input;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7021/api/1.0";

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
                string loginUrl = $"{_apiBaseUrl}/login/verification";
                var requestBody = new { Username = Username, Password = Password };
                var requestBodyJson = JsonConvert.SerializeObject(requestBody);
                var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(loginUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.GoToAsync(nameof(MainPage));
                    
                }
                else
                {
                    Debug.WriteLine("Login Failed");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Login Error");
            }
            finally
            {
                IsLoggingIn = false;
            }
        }

        
    }
}
