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
using System.ComponentModel;
using System.Web;
using CommunityToolkit.Mvvm.Messaging;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class SearchPageViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private ObservableCollection<V1Book> _booksBasedOnTitle;
        private ObservableCollection<V1Book> _booksBasedOnAuthor;
        private ObservableCollection<V1UserWithDisplayPicture> _users;
        private bool _isBusy;
        private string _queryString;
        private V1User _user;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        public string QueryString
        {
            get => _queryString;
            set
            {
                _queryString = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<V1Book> BooksBasedOnTitle
        {
            get => _booksBasedOnTitle;
            set
            {
                _booksBasedOnTitle = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<V1Book> BooksBasedOnAuthor
        {
            get => _booksBasedOnAuthor;
            set
            {
                _booksBasedOnAuthor = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<V1UserWithDisplayPicture> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public V1User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }
        public SearchPageViewModel(V1User user)
        {
            BooksBasedOnTitle = new ObservableCollection<V1Book>();
            BooksBasedOnAuthor = new ObservableCollection<V1Book>();
            Users = new ObservableCollection<V1UserWithDisplayPicture>();
            User = user;
        }

        // Retrieves the books that include the string search parameter as a result based on book title (via Google Books API).
        public async Task PopulateBookTitleResultsAsync(string query)
        {
            
            try
            {
                BooksBasedOnTitle.Clear();

                var queryReplaced = query;
                // Replaces empty space symbol with html encoding.
                queryReplaced.Replace(" ", "%20");
                string url = $"{_apiBaseUrl}/books/GetByTitle?title={queryReplaced}&maxResults=40";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var json = await responseMessage.Content.ReadAsStringAsync();
                    var bookSearch = new V1BooksDto(json);

                    foreach (V1Book book in bookSearch.Books)
                    {
                        // Doesn't show books that don't contain -either- a valid ISBN 10 or 13. 
                        // Note: not all books on Google Books follow the same format.
                        if (book.IndustryIdentifiers == null
                            || (!book.IndustryIdentifiers.ContainsKey("ISBN_10") && !book.IndustryIdentifiers.ContainsKey("ISBN_13")))
                        {
                            continue;
                        }

                        book.ImageLinks["smallThumbnail"].Replace("&", "&amp;");
                        book.ImageLinks["thumbnail"].Replace("&", "&amp;");
                        BooksBasedOnTitle.Add(book);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        // Retrieves the books that include the string search parameter as a result based on author (via Google Books API).
        public async Task PopulateBookAuthorResultsAsync(string query)
        {
            try
            {
                BooksBasedOnAuthor.Clear();

                var queryReplaced = query;
                queryReplaced.Replace(" ", "%20");
                string url = $"{_apiBaseUrl}/books/GetByAuthor?name={queryReplaced}&maxResults=40";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var json = await responseMessage.Content.ReadAsStringAsync();
                    var bookSearch = new V1BooksDto(json);

                    foreach (V1Book book in bookSearch.Books)
                    {
                        if (book.IndustryIdentifiers == null
                            || (!book.IndustryIdentifiers.ContainsKey("ISBN_10") && !book.IndustryIdentifiers.ContainsKey("ISBN_13")))
                        {
                            continue;
                        }

                        book.ImageLinks["smallThumbnail"].Replace("&", "&amp;");
                        book.ImageLinks["thumbnail"].Replace("&", "&amp;");
                        BooksBasedOnAuthor.Add(book);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        // Retrieves all users with a username including the search string parameter ('testaccount' is displayed as a result for 'test', 'account', 'stacco' etc).
        public async Task PopulateUserResultsAsync(string query)
        {
            try
            {
                Users.Clear();

                var queryReplaced = query;
                queryReplaced.Replace(" ", "%20");

                string url = $"{_apiBaseUrl}/users/GetUsersByName?name={queryReplaced}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
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

                        Users.Add(userWithDisplayPicture);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        // Navigates to the user page when a user is selected via an on-click event. Note that QueryIdentifier is passed but not fully integrated in the current 
        // version of Maui.
        public async Task NavigateToUserPageAsync(V1User user)
        {
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedUser = user;
            await Shell.Current.GoToAsync($"user?userid={user.Id}");
        }

        // Navigates to the book page when a book is selected via an on-click event. Note that QueryIdentifier is passed but not fully integrated in the current 
        // version of Maui.
        public async Task NavigateToBookPageAsync(V1Book book)
        {
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedBook = book;
            string bookId = "";

            if (book.IndustryIdentifiers["ISBN_13"] != null)
            {
                bookId = book.IndustryIdentifiers["ISBN_13"];
            }
            else if (book.IndustryIdentifiers["ISBN_10"] != null)
            {
                bookId = book.IndustryIdentifiers["ISBN_10"];
            }

            await Shell.Current.GoToAsync($"book?bookid={bookId}");
        }

        public async Task LoadAsync(string searchQuery)
        {
            IsBusy = true;

            await PopulateBookTitleResultsAsync(searchQuery);
            await PopulateBookAuthorResultsAsync(searchQuery);
            await PopulateUserResultsAsync(searchQuery);

            IsBusy = false;
        }
    }
}
