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
    public class UserPageViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private bool _isBusy;
        public V1User User { get; set; }
        public V1User SelectedUser { get; set; }
        public byte[] SelectedUserDisplayPicture { get; set; }
        private V1LibraryCollection _userLibrary;
        public ObservableCollection<V1Book> UserBooks { get; set; }
        public ObservableCollection<V1ReadingGoals> UserReadingGoals { get; set; }


        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public V1LibraryCollection UserLibrary
        {
            get => _userLibrary;
            set
            {
                _userLibrary = value;
                OnPropertyChanged();
            }
        }

        public UserPageViewModel(V1User loggedInUser, V1User selectedUser, byte[] selectedUserDisplayPicture)
        {
            User = loggedInUser;
            SelectedUser = selectedUser;
            UserBooks = new ObservableCollection<V1Book>();
            UserReadingGoals = new ObservableCollection<V1ReadingGoals>();
            SelectedUserDisplayPicture = selectedUserDisplayPicture;
        }

        public async Task PopulateBooks()
        {

            try
            {
                UserBooks.Clear();

                string loginUrl = $"{_apiBaseUrl}/libraries/GetUserLibrary?userId={SelectedUser.Id}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                V1LibraryCollection library = JsonConvert.DeserializeObject<V1LibraryCollection>(json);
                UserLibrary = library;
                

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
                        UserBooks.Add(book);
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

        public async Task PopulateReadingGoals()
        {

            try
            {
                UserReadingGoals.Clear();

                string loginUrl = $"{_apiBaseUrl}/goals/GetAllGoals?userId={SelectedUser.Id}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();

                dynamic? jArrayReadingGoals = JsonConvert.DeserializeObject(json);

                foreach (JObject userJson in jArrayReadingGoals)
                {
                    V1ReadingGoals readingGoal = JsonConvert.DeserializeObject<V1ReadingGoals>(userJson.ToString());

                    UserReadingGoals.Add(readingGoal);
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
            await PopulateReadingGoals();
            IsBusy = false;
        }
    }
}
