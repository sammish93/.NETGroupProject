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
using System.Collections.ObjectModel;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private ObservableCollection<V1Book> _highestRatedBooks;
        private ObservableCollection<V1Book> _recentlyReadBooks;
        private ObservableCollection<V1UserWithDisplayPicture> _nearbyUsers;
        private bool _isBusy;
        private V1User _loggedInUser;
        private V1ReadingGoals _loggedInUserRecentReadingGoal;
        private double _readingGoalProgress;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
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

        public ObservableCollection<V1Book> HighestRatedBooks
        {
            get => _highestRatedBooks;
            set
            {
                _highestRatedBooks = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<V1Book> RecentlyReadBooks
        {
            get => _recentlyReadBooks;
            set
            {
                _recentlyReadBooks = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<V1UserWithDisplayPicture> NearbyUsers
        {
            get => _nearbyUsers;
            set
            {
                _nearbyUsers = value;
                OnPropertyChanged();
            }
        }

        public V1ReadingGoals LoggedInUserRecentReadingGoal
        {
            get => _loggedInUserRecentReadingGoal;
            set
            {
                _loggedInUserRecentReadingGoal = value;
                OnPropertyChanged();
            }
        }

        public double ReadingGoalProgress
        {
            get => _readingGoalProgress;
            set
            {
                _readingGoalProgress = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel(V1User user)
        {
            HighestRatedBooks = new ObservableCollection<V1Book>();
            RecentlyReadBooks = new ObservableCollection<V1Book>();
            NearbyUsers = new ObservableCollection<V1UserWithDisplayPicture>();
            LoggedInUser = user;
        }

        public async Task PopulateHighestRatedBooks()
        {
            if (HighestRatedBooks.IsNullOrEmpty() || App.IsUserLibraryAltered)
            {
                try
                {
                    HighestRatedBooks.Clear();

                    string url = $"{_apiBaseUrl}/libraries/GetUserHighestRatedBooks?userId={LoggedInUser.Id}&numberOfResults=10";

                    using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                    responseMessage.EnsureSuccessStatusCode();
                    var json = await responseMessage.Content.ReadAsStringAsync();
                    V1LibraryCollection library = JsonConvert.DeserializeObject<V1LibraryCollection>(json);

                    foreach (V1LibraryEntry entry in library.Entries)
                    {
                        string Isbn;
                        if (entry.LibraryEntryISBN10 != null)
                        {
                            Isbn = entry.LibraryEntryISBN10;
                        }
                        else if (entry.LibraryEntryISBN13 != null)
                        {
                            Isbn = entry.LibraryEntryISBN13;
                        }
                        else
                        {
                            continue;
                        }

                        var loginUrlTwo = $"{_apiBaseUrl}/books/GetByIsbn?isbn={Isbn}";

                        using HttpResponseMessage responseMessageTwo = await _httpClient.GetAsync(loginUrlTwo);
                        responseMessageTwo.EnsureSuccessStatusCode();
                        var jsonTwo = await responseMessageTwo.Content.ReadAsStringAsync();
                        V1BooksDto bookSearch = new V1BooksDto(jsonTwo);

                        foreach (V1Book book in bookSearch.Books)
                        {

                            book.ImageLinks["smallThumbnail"].Replace("&", "&amp;");
                            book.ImageLinks["thumbnail"].Replace("&", "&amp;");
                            HighestRatedBooks.Add(book);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public async Task PopulateRecentlyReadBooks()
        {
            if (RecentlyReadBooks.IsNullOrEmpty() || App.IsUserLibraryAltered)
            {
                try
                {
                    RecentlyReadBooks.Clear();

                    string url = $"{_apiBaseUrl}/libraries/GetUserMostRecentBooks?userId={LoggedInUser.Id}&numberOfResults=10";

                    using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                    responseMessage.EnsureSuccessStatusCode();
                    var json = await responseMessage.Content.ReadAsStringAsync();
                    V1LibraryCollection library = JsonConvert.DeserializeObject<V1LibraryCollection>(json);

                    foreach (V1LibraryEntry entry in library.Entries)
                    {
                        string Isbn;
                        if (entry.LibraryEntryISBN10 != null)
                        {
                            Isbn = entry.LibraryEntryISBN10;
                        }
                        else if (entry.LibraryEntryISBN13 != null)
                        {
                            Isbn = entry.LibraryEntryISBN13;
                        }
                        else
                        {
                            continue;
                        }

                        var loginUrlTwo = $"{_apiBaseUrl}/books/GetByIsbn?isbn={Isbn}";

                        using HttpResponseMessage responseMessageTwo = await _httpClient.GetAsync(loginUrlTwo);
                        responseMessageTwo.EnsureSuccessStatusCode();
                        var jsonTwo = await responseMessageTwo.Content.ReadAsStringAsync();
                        V1BooksDto bookSearch = new V1BooksDto(jsonTwo);

                        foreach (V1Book book in bookSearch.Books)
                        {

                            book.ImageLinks["smallThumbnail"].Replace("&", "&amp;");
                            book.ImageLinks["thumbnail"].Replace("&", "&amp;");
                            RecentlyReadBooks.Add(book);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public async Task PopulateNearbyUsers()
        {
            if (NearbyUsers.IsNullOrEmpty())
            {
                try
                {
                    NearbyUsers.Clear();

                    string loginUrl = $"{_apiBaseUrl}/users/GetUsersByCity?city={LoggedInUser.City}";

                    using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                    responseMessage.EnsureSuccessStatusCode();
                    var json = await responseMessage.Content.ReadAsStringAsync();

                    dynamic? jArrayUsers = JsonConvert.DeserializeObject(json);

                    foreach (JObject userJson in jArrayUsers)
                    {
                        V1User user = JsonConvert.DeserializeObject<V1User>(userJson.ToString());

                        V1UserWithDisplayPicture userWithDisplayPicture;


                        string displayPictureUrl = $"{_apiBaseUrl}/icons/GetIconByName?username={user.UserName}";
                        HttpResponseMessage resultDisplayPicture = await _httpClient.GetAsync(displayPictureUrl);

                        if (resultDisplayPicture.IsSuccessStatusCode)
                        {
                            var responseStringDisplayPicture = await resultDisplayPicture.Content.ReadAsStringAsync();

                            V1UserIcon displayPicture = JsonConvert.DeserializeObject<V1UserIcon>(responseStringDisplayPicture);

                            userWithDisplayPicture = new V1UserWithDisplayPicture(user, displayPicture.DisplayPicture);
                        }
                        else
                        {
                            userWithDisplayPicture = new V1UserWithDisplayPicture(user, App.DefaultDisplayPicture);
                        }

                        NearbyUsers.Add(userWithDisplayPicture);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public async Task NavigateToUserPage(V1User user)
        {
            App.SelectedUser = user;
            await GetSelectedUserDisplayPicture(user.UserName);
            await Shell.Current.GoToAsync($"///user?userid={user.Id}");
        }

        public async Task GetSelectedUserDisplayPicture(string username)
        {
            string displayPictureUrl = $"{_apiBaseUrl}/icons/GetIconByName?username={username}";
            HttpResponseMessage resultDisplayPicture = await _httpClient.GetAsync(displayPictureUrl);

            if (resultDisplayPicture.IsSuccessStatusCode)
            {
                var responseStringDisplayPicture = await resultDisplayPicture.Content.ReadAsStringAsync();

                V1UserIcon displayPicture = JsonConvert.DeserializeObject<V1UserIcon>(responseStringDisplayPicture);

                App.SelectedUserDisplayPicture = displayPicture.DisplayPicture;
            } else
            {
                App.SelectedUserDisplayPicture = App.DefaultDisplayPicture;
            }
        }

        public async Task NavigateToBookPage(V1Book book)
        {
            App.SelectedBook = book;
            string bookId = "";

            if (book.IndustryIdentifiers["ISBN_13"] != null)
            {
                bookId = book.IndustryIdentifiers["ISBN_13"];
            } else if (book.IndustryIdentifiers["ISBN_10"] != null)
            {
                bookId = book.IndustryIdentifiers["ISBN_10"];
            }

            await Shell.Current.GoToAsync($"///book?bookid={bookId}");
        }

        public async Task GetMostRecentReadingGoal()
        {
            string url = $"{_apiBaseUrl}/goals/GetRecentGoal?userId={LoggedInUser.Id}";
            HttpResponseMessage result = await _httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var responseString = await result.Content.ReadAsStringAsync();

                V1ReadingGoals readingGoal = JsonConvert.DeserializeObject<V1ReadingGoals>(responseString);

                LoggedInUserRecentReadingGoal = readingGoal;
            }
        }

        public async Task LoadAsync()
        {
            IsBusy = true;
            await PopulateHighestRatedBooks();
            await PopulateRecentlyReadBooks();
            App.IsUserLibraryAltered = false;
            await PopulateNearbyUsers();
            IsBusy = false;
        }

        public async Task LoadProgressBarAsync()
        {
            IsBusy = true;
            await GetMostRecentReadingGoal();
        }
    }
}
