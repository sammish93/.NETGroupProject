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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Messaging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public partial class AppShellViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private readonly HttpClient _httpClient = new HttpClient();
        private V1User _user;
        private string _titleCurrentPage = "defaultTitle";
        private byte[] _displayPicture;


        public V1User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public string TitleCurrentPage
        {
            get => _titleCurrentPage;
            set => SetProperty(ref _titleCurrentPage, value);
        }

        public byte[] DisplayPicture
        {
            get => _displayPicture;
            set => SetProperty(ref _displayPicture, value);
        }

        public AppShellViewModel()
        {
        }

        public AppShellViewModel(V1User user, byte[] displayPicture)
        {
            User = user;
            DisplayPicture = displayPicture;
        }

        public ICommand HomeButtonCommand => new Command(async () => await NavButtonAsync("///home"));
        public ICommand ProfileButtonCommand => new Command(async () => await NavigateToUserPageAsync(User));
        public ICommand LibraryCommand => new Command(async () => await NavButtonAsync("library"));
        public ICommand MarketplaceCommand => new Command(async () => await NavButtonAsync("marketplace"));
        public ICommand SettingsCommand => new Command(async () => await NavButtonAsync("settings"));
        public ICommand MessagesButtonCommand => new Command(async () => await NavButtonAsync("messages"));
        public ICommand LogOutCommand => new Command(async () => await LogOutAsync());
        public ICommand PerformSearch => new Command<string>(async (string query) =>
        {
            await NavigateToSearchPageAsync(query);
        });

        // Navigates to the search page after a text string is entered in the shell search bar.
        public async Task NavigateToSearchPageAsync(string query)
        {
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SearchQuery = query;
            await Shell.Current.GoToAsync($"///home");
            // Note that the parameters passed via QueryProperty doesn't currently work as intended in the current version of Maui. Query string is therefore 
            // saved in the UserSingleton class used as a singleton.
            await Shell.Current.GoToAsync($"search?query={query}");
        }

        public async Task NavigateToUserPageAsync(V1User user)
        {
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedUser = user;
            await GetSelectedUserDisplayPictureAsync(user.UserName);
            await Shell.Current.GoToAsync($"user?userid={user.Id}");
        }

        public async Task LogOutAsync()
        {
            // Turns off the background tasker for said user.
            await SwitchOffBackgroundTaskerAsync(Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().LoggedInUser);

            // Removes all saved data relating to the previously logged in user.
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedEntry = null;
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedUser = null;
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedBook = null;
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SearchQuery = null;
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedUser = null;
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedUserDisplayPicture = null;
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().DefaultDisplayPicture = null;
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().IsUserLibraryAltered = true;
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().LoggedInUser = null;
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SearchQuery = null;
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedBook = null;
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedEntry = null;
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedUser = null;
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedUserDisplayPicture = null;
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().UserDisplayPicture = null;
            // Navigates back to the login page and removes all other pages from the nav stack.
            await Shell.Current.GoToAsync($"///login");
        }

        // Retrieves the logged in user's display picture.
        public async Task GetSelectedUserDisplayPictureAsync(string username)
        {
            string displayPictureUrl = $"{_apiBaseUrl}/icons/GetIconByName?username={username}";
            HttpResponseMessage resultDisplayPicture = await _httpClient.GetAsync(displayPictureUrl);

            if (resultDisplayPicture.IsSuccessStatusCode)
            {
                var responseStringDisplayPicture = await resultDisplayPicture.Content.ReadAsStringAsync();

                V1UserIcon displayPicture = JsonConvert.DeserializeObject<V1UserIcon>(responseStringDisplayPicture);

                Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedUserDisplayPicture = displayPicture.DisplayPicture;
            }
            else
            {
                // If the user has no display picture then a default one is shown.
                Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedUserDisplayPicture = Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().DefaultDisplayPicture;
            }
        }

        // Stops background tasks for the logged in user.
        private async Task SwitchOffBackgroundTaskerAsync(V1User user)
        {
            try
            {
                string url = $"https://localhost:7125/api/BackgroundJob/MessageChecker/StopMessageJob?userId={user.Id}";
                var response = await _httpClient.DeleteAsync(url);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task NavButtonAsync(string root)
        {
            await Shell.Current.GoToAsync(root);
        }
    }
}

