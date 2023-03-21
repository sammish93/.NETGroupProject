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
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.Messages;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class SearchPageViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        public ObservableCollection<V1Book> BooksBasedOnTitle { get; set; }
        public ObservableCollection<V1Book> BooksBasedOnAuthor { get; set; }
        public ObservableCollection<V1User> Users { get; set; }
        private bool _isBusy;
        public string QueryString { get; set; } = "oscar wilde";
        public V1User User { get; set; }


        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public SearchPageViewModel(V1User user)
        {
            BooksBasedOnTitle = new ObservableCollection<V1Book>();
            BooksBasedOnAuthor = new ObservableCollection<V1Book>();
            Users = new ObservableCollection<V1User>();
            User = user;
        }

        public async Task PopulateBookTitleResults(string query)
        {
            
            try
            {
                BooksBasedOnTitle.Clear();

                var queryReplaced = query;
                queryReplaced.Replace(" ", "%20");
                string loginUrl = $"{_apiBaseUrl}/books/GetByTitle?title={queryReplaced}&maxResults=40";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                var bookSearch = new V1BooksDto(json);

                foreach (V1Book book in bookSearch.Books)
                {
                    if (book.IndustryIdentifiers == null
                        || (book.IndustryIdentifiers["ISBN_10"].IsNullOrEmpty() && book.IndustryIdentifiers["ISBN_13"].IsNullOrEmpty()))
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
                string loginUrl = $"{_apiBaseUrl}/books/GetByAuthor?name={queryReplaced}&maxResults=40";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                var bookSearch = new V1BooksDto(json);

                foreach (V1Book book in bookSearch.Books)
                {
                    if (book.IndustryIdentifiers == null
                        || (book.IndustryIdentifiers["ISBN_10"].IsNullOrEmpty() && book.IndustryIdentifiers["ISBN_13"].IsNullOrEmpty()))
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

                string loginUrl = $"{_apiBaseUrl}/users/GetAll";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();

                dynamic? jArrayUsers = JsonConvert.DeserializeObject(json);

                foreach (JObject userJson in jArrayUsers)
                {
                    V1User user = JsonConvert.DeserializeObject<V1User>(userJson.ToString());
                    Users.Add(user);
                }
            }
            catch (Exception ex)
            {

            }
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
