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
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using System.Diagnostics.Metrics;
using System.Globalization;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class BookPageViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private bool _isBusy;
        private V1User _user;
        private V1Book _selectedBook;
        private ObservableCollection<ReadingStatus> _readingStatuses;
        private ReadingStatus _selectedReadingStatus;
        private ObservableCollection<int> _ratings;
        private int _selectedRating;
        private DateTime _selectedDate;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public V1Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
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

        public ObservableCollection<ReadingStatus> ReadingStatuses
        {
            get => _readingStatuses;
            set
            {
                _readingStatuses = value;
                OnPropertyChanged();
            }
        }

        public ReadingStatus SelectedReadingStatus
        {
            get => _selectedReadingStatus;
            set
            {
                _selectedReadingStatus = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<int> Ratings
        {
            get => _ratings;
            set
            {
                _ratings = value;
                OnPropertyChanged();
            }
        }

        public int SelectedRating
        {
            get => _selectedRating;
            set
            {
                _selectedRating = value;
                OnPropertyChanged();
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }

        public BookPageViewModel(V1User user, V1Book book)
        {
            User = user;
            SelectedBook = book;
            ReadingStatuses = new ObservableCollection<ReadingStatus>();
            ReadingStatuses.Add(ReadingStatus.ToRead);
            ReadingStatuses.Add(ReadingStatus.Reading);
            ReadingStatuses.Add(ReadingStatus.Completed);

            SelectedDate = DateTime.Now;

            Ratings = new ObservableCollection<int>();
            for (int i = 1; i <= 10; i++)
            {
                Ratings.Add(i);
            }
        }

        public ICommand AddBookToLibraryCommand => new Command(async () => await AddBookToLibraryAsync());

        private async Task AddBookToLibraryAsync()
        {
            try
            {
                string createLibEntryUrl = $"{_apiBaseUrl}/libraries/CreateEntry";

                int? rating = null; 
                if (SelectedReadingStatus == ReadingStatus.Completed)
                {
                    if (SelectedRating == 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Oops!", "You have forgotten to choose a rating.", "OK");
                        return; 
                    }
                    rating = SelectedRating; 

                }

                bool isBookInLibraryResult = await isBookInLibrary(SelectedBook);
                bool answer = false;

                if (isBookInLibraryResult)
                {
                    answer = await Application.Current.MainPage.DisplayAlert("This book is already in your library", "Would you like to add it an additional time?", "Yes", "No");
                }

                if (answer || !isBookInLibraryResult)
                {
                    var requestBody = new V1LibraryEntry(User, SelectedBook, rating, SelectedDate, SelectedReadingStatus);

                    var requestBodyJson = JsonConvert.SerializeObject(requestBody);
                    var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _httpClient.PostAsync(createLibEntryUrl, requestContent);
                    if (response.IsSuccessStatusCode)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success!", "You have added this book to your library.", "OK");
                        Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().IsUserLibraryAltered = true;
                        if (SelectedReadingStatus == ReadingStatus.Completed)
                        {
                            await UpdateReadingLibrary(User.Id, SelectedDate);
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Uh oh!", "Something went wrong.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task<bool> isBookInLibrary(V1Book book)
        {
            string isbn = "";
            if (book.IndustryIdentifiers["ISBN_13"] != null)
            {
                isbn = book.IndustryIdentifiers["ISBN_13"];
            } else if (book.IndustryIdentifiers["ISBN_10"] != null)
            {
                isbn = book.IndustryIdentifiers["ISBN_10"];
            }
            
            string url = $"{_apiBaseUrl}/libraries/GetEntryFromSpecificUser?userId={User.Id}&isbn={isbn}";

            HttpResponseMessage result = await _httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var libraryEntry = await result.Content.ReadAsStringAsync();

                if (!libraryEntry.IsNullOrEmpty()) 
                { 
                    return true; 
                }
                
            }

            return false;
        }

        public void UpdateDate(DateTime dateTime)
        {
            SelectedDate = dateTime;
        }

        public async Task UpdateReadingLibrary(Guid userId, DateTime dateTime)
        {
            string url = $"{_apiBaseUrl}/goals/GetGoalId?userId={userId}&GoalDate={dateTime.ToString("yyyy/MM/dd")}";

            HttpResponseMessage result = await _httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var libraryId = await result.Content.ReadAsStringAsync();

                Guid libraryIdGuid = JsonConvert.DeserializeObject<Guid>(libraryId);

                int amount = 1;
                url = $"{_apiBaseUrl}/goals/IncrementReadingGoal?id={libraryIdGuid}&amount={amount}";
                var jsonString = JsonConvert.SerializeObject(new { id = libraryIdGuid, amount });
                var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, httpContent);
            }
        }



        public async Task LoadAsync()
        {
            IsBusy = true;

            IsBusy = false;
        }
    }
}
