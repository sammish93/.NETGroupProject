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

        private ObservableCollection<V1Comments> _recentComments;
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

        public ObservableCollection<V1Comments> RecentComments
        {
            get => _recentComments;
            set
            {
                _recentComments = value;
                OnPropertyChanged();
            }
        }


        public MainPageViewModel(V1User user)
        {
            HighestRatedBooks = new ObservableCollection<V1Book>();
            RecentlyReadBooks = new ObservableCollection<V1Book>();
            NearbyUsers = new ObservableCollection<V1UserWithDisplayPicture>();
            RecentComments = new ObservableCollection<V1Comments>();
            LoggedInUser = user;
        }

        public async Task PopulateHighestRatedBooks()
        {
            if (HighestRatedBooks.IsNullOrEmpty() || Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().IsUserLibraryAltered)
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
            if (RecentlyReadBooks.IsNullOrEmpty() || Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().IsUserLibraryAltered)
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
                            userWithDisplayPicture = new V1UserWithDisplayPicture(user, Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().DefaultDisplayPicture);
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
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedUser = user;
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

                Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedUserDisplayPicture = displayPicture.DisplayPicture;
            } else
            {
                Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedUserDisplayPicture = Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().DefaultDisplayPicture;
            }
        }

        public async Task NavigateToBookPage(V1Book book)
        {
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedBook = book;
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

        public async Task PopulateComments(V1User selectedUser)
        {

            try
            {
                RecentComments.Clear();

                string url = $"{_apiBaseUrl}/comments/GetCommentsByAuthorId?authorId={selectedUser.Id}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                dynamic? jArrayComments = JsonConvert.DeserializeObject(json);

                foreach (JObject commentsJson in jArrayComments["response"])
                {
                    V1Comments comment = JsonConvert.DeserializeObject<V1Comments>(commentsJson.ToString());
                    if (comment.CommentType != ClassLibrary.Enums.V1.CommentType.Reply)
                    {
                        if (comment.UserId != null)
                        {
                            comment.AuthorObject = await GetUserWithDisplayPictureAsync(comment.UserId.ToString());
                            comment.CommentSummary = $"Commented on {comment.AuthorObject.User.UserName}'s profile";
                        }
                        else if (!comment.ISBN13.IsNullOrEmpty())
                        {
                            comment.BookObject = await GetBookAsync(comment.ISBN13);
                            comment.AuthorObject = await GetUserWithDisplayPictureAsync(LoggedInUser.Id.ToString());
                            comment.CommentSummary = $"Commented on {comment.BookObject.Title}";
                        }
                        else if (!comment.ISBN10.IsNullOrEmpty())
                        {
                            comment.BookObject = await GetBookAsync(comment.ISBN10);
                            comment.AuthorObject = await GetUserWithDisplayPictureAsync(LoggedInUser.Id.ToString());
                            comment.CommentSummary = $"Commented on {comment.BookObject.Title}";
                        }

                        RecentComments.Add(comment);
                    } else
                    {
                        string urlTwo = $"{_apiBaseUrl}/comments/GetCommentById?id={comment.ParentCommentId}";

                        using HttpResponseMessage responseMessageTwo = await _httpClient.GetAsync(urlTwo);
                        responseMessageTwo.EnsureSuccessStatusCode();
                        var jsonTwo = await responseMessageTwo.Content.ReadAsStringAsync();
                        var commentTwo = JsonConvert.DeserializeObject<V1Comments>(jsonTwo);

                            if (commentTwo.UserId != null)
                            {
                                comment.AuthorObject = await GetUserWithDisplayPictureAsync(commentTwo.UserId.ToString());
                                comment.CommentSummary = $"Replied to a comment on {comment.AuthorObject.User.UserName}'s profile";
                            }
                            else if (!commentTwo.ISBN13.IsNullOrEmpty())
                            {
                                comment.BookObject = await GetBookAsync(commentTwo.ISBN13);
                                comment.AuthorObject = await GetUserWithDisplayPictureAsync(LoggedInUser.Id.ToString());
                                comment.CommentSummary = $"Replied to a comment on {comment.BookObject.Title}";
                            }
                            else if (!commentTwo.ISBN10.IsNullOrEmpty())
                            {
                                comment.BookObject = await GetBookAsync(commentTwo.ISBN10);
                                comment.AuthorObject = await GetUserWithDisplayPictureAsync(LoggedInUser.Id.ToString());
                                comment.CommentSummary = $"Replied to a comment on {comment.BookObject.Title}";
                            }

                            RecentComments.Add(comment);
                        }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<V1UserWithDisplayPicture> GetUserWithDisplayPictureAsync(String guidString)
        {
            try
            {

                var guid = Guid.Parse(guidString);
                string loginUrl = $"{_apiBaseUrl}/users/GetById?id={guid}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();

                V1User user = JsonConvert.DeserializeObject<V1User>(json.ToString());

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
                    userWithDisplayPicture = new V1UserWithDisplayPicture(user, Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().DefaultDisplayPicture);
                }

                return userWithDisplayPicture;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<V1Book> GetBookAsync(string isbn)
        {
            try
            {

                var url = $"{_apiBaseUrl}/books/GetByIsbn?isbn={isbn}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                V1BooksDto bookSearch = new V1BooksDto(json);

                foreach (V1Book book in bookSearch.Books)
                {

                    book.ImageLinks["smallThumbnail"].Replace("&", "&amp;");
                    book.ImageLinks["thumbnail"].Replace("&", "&amp;");
                    return book;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task LoadAsync()
        {
            IsBusy = true;
            await PopulateHighestRatedBooks();
            await PopulateRecentlyReadBooks();
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().IsUserLibraryAltered = false;
            await PopulateNearbyUsers();
            await PopulateComments(LoggedInUser);
            IsBusy = false;
        }

        public async Task LoadProgressBarAsync()
        {
            IsBusy = true;
            await GetMostRecentReadingGoal();
        }
    }
}
