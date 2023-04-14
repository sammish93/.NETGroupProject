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
        private V1User _user;
        private V1User _selectedUser;
        private byte[] _selectedUserDisplayPicture;
        private V1ReadingGoals _selectedUserRecentReadingGoal;
        private V1LibraryCollection _userLibrary;
        private ObservableCollection<V1Book> _userBooks;
        private ObservableCollection<V1ReadingGoals> _userReadingGoals;


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

        public byte[] SelectedUserDisplayPicture
        {
            get => _selectedUserDisplayPicture;
            set
            {
                _selectedUserDisplayPicture = value;
                OnPropertyChanged();
            }
        }

        public V1ReadingGoals SelectedUserRecentReadingGoal
        {
            get => _selectedUserRecentReadingGoal;
            set
            {
                _selectedUserRecentReadingGoal = value;
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

        public V1User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<V1Book> UserBooks
        {
            get => _userBooks;
            set
            {
                _userBooks = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<V1ReadingGoals> UserReadingGoals
        {
            get => _userReadingGoals;
            set
            {
                _userReadingGoals = value;
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

        public async Task PopulateBooks(V1User user)
        {

            try
            {
                UserBooks.Clear();

                string url = $"{_apiBaseUrl}/libraries/GetUserHighestRatedBooks?userId={user.Id}&numberOfResults=4";

                string libraryUrl = $"{_apiBaseUrl}/libraries/GetUserLibrary?userId={user.Id}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                V1LibraryCollection library = JsonConvert.DeserializeObject<V1LibraryCollection>(json);

                using HttpResponseMessage responseMessageLibrary = await _httpClient.GetAsync(libraryUrl);
                responseMessageLibrary.EnsureSuccessStatusCode();
                var jsonLibrary = await responseMessageLibrary.Content.ReadAsStringAsync();
                V1LibraryCollection libraryComplete = JsonConvert.DeserializeObject<V1LibraryCollection>(jsonLibrary);
                UserLibrary = libraryComplete;


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

        public async Task PopulateReadingGoals(V1User user)
        {

            try
            {
                UserReadingGoals.Clear();

                string loginUrl = $"{_apiBaseUrl}/goals/GetAllGoals?userId={user.Id}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();

                dynamic? jArrayReadingGoals = JsonConvert.DeserializeObject(json);

                foreach (JObject userJson in jArrayReadingGoals)
                {
                    V1ReadingGoals readingGoal = JsonConvert.DeserializeObject<V1ReadingGoals>(userJson.ToString());

                    if (readingGoal.Id != SelectedUserRecentReadingGoal.Id)
                    {
                        UserReadingGoals.Add(readingGoal);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task GetSelectedUserDisplayPicture(string username)
        {
            string displayPictureUrl = $"{_apiBaseUrl}/icons/GetIconByName?username={username}";
            HttpResponseMessage resultDisplayPicture = await _httpClient.GetAsync(displayPictureUrl);

            if (resultDisplayPicture.IsSuccessStatusCode)
            {
                var responseStringDisplayPicture = await resultDisplayPicture.Content.ReadAsStringAsync();

                V1UserIcon displayPicture = JsonConvert.DeserializeObject<V1UserIcon>(responseStringDisplayPicture);

                SelectedUserDisplayPicture = displayPicture.DisplayPicture;
            }
            else
            {
                SelectedUserDisplayPicture = App.DefaultDisplayPicture;
            }
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

        public async Task GetMostRecentReadingGoal(V1User user)
        {
            string url = $"{_apiBaseUrl}/goals/GetRecentGoal?userId={user.Id}";
            HttpResponseMessage result = await _httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var responseString = await result.Content.ReadAsStringAsync();

                V1ReadingGoals readingGoal = JsonConvert.DeserializeObject<V1ReadingGoals>(responseString);

                SelectedUserRecentReadingGoal = readingGoal;
            }
        }

        public async Task LoadAsync()
        {
            IsBusy = true;
            await GetSelectedUserDisplayPicture(SelectedUser.UserName);
            await PopulateBooks(SelectedUser);
            await PopulateReadingGoals(SelectedUser);
            IsBusy = false;
        }

        public async Task LoadProgressBarAsync()
        {
            IsBusy = true;
            await GetMostRecentReadingGoal(SelectedUser);
        }
    }
}
