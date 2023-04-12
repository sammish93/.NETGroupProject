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

        public async Task PopulateBookTitleResults(string query)
        {
            
            try
            {
                BooksBasedOnTitle.Clear();

                var queryReplaced = query;
                queryReplaced.Replace(" ", "%20");
                string url = $"{_apiBaseUrl}/books/GetByTitle?title={queryReplaced}&maxResults=40";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
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
                    BooksBasedOnTitle.Add(book);
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task PopulateBookAuthorResults(string query)
        {

            try
            {
                BooksBasedOnAuthor.Clear();

                var queryReplaced = query;
                queryReplaced.Replace(" ", "%20");
                string url = $"{_apiBaseUrl}/books/GetByAuthor?name={queryReplaced}&maxResults=40";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
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
            catch (Exception ex)
            {

            }
        }

        public async Task PopulateUserResults(string query)
        {

            try
            {
                Users.Clear();

                var queryReplaced = query;
                queryReplaced.Replace(" ", "%20");

                string url = $"{_apiBaseUrl}/users/GetUsersByName?name={queryReplaced}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
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

                    Users.Add(userWithDisplayPicture);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task NavigateToUserPage(V1User user)
        {
            App.SelectedUser = user;
            await Shell.Current.GoToAsync($"///user?userid={user.Id}");
        }

        public async Task NavigateToBookPage(V1Book book)
        {
            App.SelectedBook = book;
            string bookId = "";

            if (book.IndustryIdentifiers["ISBN_13"] != null)
            {
                bookId = book.IndustryIdentifiers["ISBN_13"];
            }
            else if (book.IndustryIdentifiers["ISBN_10"] != null)
            {
                bookId = book.IndustryIdentifiers["ISBN_10"];
            }

            await Shell.Current.GoToAsync($"///book?bookid={bookId}");
        }

        public async Task LoadAsync(string searchQuery)
        {
            IsBusy = true;

            await PopulateBookTitleResults(searchQuery);
            await PopulateBookAuthorResults(searchQuery);
            await PopulateUserResults(searchQuery);

            IsBusy = false;
        }
    }
}
