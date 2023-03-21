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

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        public ObservableCollection<V1Book> Books { get; set; }
        public ObservableCollection<V1Book> RecentlyReadBooks { get; set; }
        public ObservableCollection<V1User> NearbyUsers { get; set; }
        public int ReadingGoalSize { get; set; } = 12;
        public int ReadingGoalTarget { get; set; } = 20;
        private bool _isBusy;
        public V1User LoggedInUser { get; set; }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel()
        {
            Books = new ObservableCollection<V1Book>();
            RecentlyReadBooks = new ObservableCollection<V1Book>();
            NearbyUsers = new ObservableCollection<V1User>();
            LoggedInUser = App.LoggedInUser;
        }

        //public ICommand PopulateBooksCommand => new Command(async () => await populateBooks());

        public async Task PopulateBooks()
        {
            
            try
            {
                string loginUrl = $"{_apiBaseUrl}/books/GetBookByCategory?subject=fantasy";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                var bookSearch = new V1BooksDto(json);

                foreach (V1Book book in bookSearch.Books)
                {
                    if (book.IndustryIdentifiers == null)
                    {
                        continue;
                    }

                    book.ImageLinks["smallThumbnail"].Replace("&", "&amp;");
                    book.ImageLinks["thumbnail"].Replace("&", "&amp;");
                    Books.Add(book);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task PopulateRecentlyReadBooks()
        {

            try
            {
                string loginUrl = $"{_apiBaseUrl}/libraries/GetUserLibrary?userId={LoggedInUser.Id}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                V1LibraryCollection library = JsonConvert.DeserializeObject<V1LibraryCollection>(json);

                foreach (V1LibraryEntry entry in library.Entries)
                {
                    string Isbn;
                    if (entry.LibraryEntryISBN10 != null)
                    {
                        Isbn = entry.LibraryEntryISBN10;
                    } else if (entry.LibraryEntryISBN13 != null)
                    {
                        Isbn = entry.LibraryEntryISBN13;
                    } else
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
            finally
            {
            }
        }

        public async Task PopulateNearbyUsers()
        {

            try
            {
                string loginUrl = $"{_apiBaseUrl}/users/GetAll";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();

                dynamic? jArrayUsers = JsonConvert.DeserializeObject(json);

                foreach (JObject userJson in jArrayUsers)
                {
                    V1User user = JsonConvert.DeserializeObject<V1User>(userJson.ToString());
                    //var book = new V1Book(jObjectItem.ToString());
                    NearbyUsers.Add(user);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task LoadAsync()
        {
            IsBusy = true;
            await PopulateBooks();
            await PopulateRecentlyReadBooks();
            await PopulateNearbyUsers();
            IsBusy = false;
        }
    }
}
