using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Microsoft.Maui.Layouts;
using Microsoft.Maui;


namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class LibraryPageViewModel : BaseViewModel
    {

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private V1User _loggedInUser { get; set; }
        private V1Book _book { get; set; }
        private V1LibraryEntryWithImage _selectedEntry;
        private V1Book _selectedEntryBook;
        private V1LibraryCollection _completeLibrary { get; set; }
        private ObservableCollection<V1LibraryEntry> _readEntries { get; set; }
        private ObservableCollection<V1LibraryEntry> _toBeRead { get; set; }
        private ObservableCollection<V1LibraryEntry> _currentlyReading { get; set; }
        private bool _isBusy;

        private ObservableCollection<ReadingStatus> _readingStatuses;
        private ReadingStatus? _selectedReadingStatus;
        private ObservableCollection<int> _ratings;
        private int? _selectedRating;
        private DateTime? _selectedDate;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;

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

        public V1Book Book
        {
            get => _book;
            set
            {
                _book = value;
                OnPropertyChanged();

            }
        }

        public V1LibraryEntryWithImage SelectedEntry
        {
            get { return _selectedEntry; }
            set
            {
                _selectedEntry = value;
                OnPropertyChanged();

            }
        }

        public V1Book SelectedEntryBook
        {
            get { return _selectedEntryBook; }
            set
            {
                _selectedEntryBook = value;
                OnPropertyChanged();

            }
        }

        public V1LibraryCollection CompleteLibrary
        {
            get => _completeLibrary;
            set
            {
                _completeLibrary = value;
                OnPropertyChanged();

            }
        }

        public ObservableCollection<V1LibraryEntry> ReadEntries
        {
            get => _readEntries;
            set
            {
                _readEntries = value;
                OnPropertyChanged();

            }
        }

        public ObservableCollection<V1LibraryEntry> ToBeRead
        {
            get => _toBeRead;
            set
            {
                _toBeRead = value;
                OnPropertyChanged();

            }
        }

        public ObservableCollection<V1LibraryEntry> CurrentlyReading
        {
            get => _currentlyReading;
            set
            {
                _currentlyReading = value;
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

        public ReadingStatus? SelectedReadingStatus
        {
            get => _selectedReadingStatus;
            set
            {
                _selectedReadingStatus = value;
                OnPropertyChanged();
            }
        }
        private List<ReadingStatus> _readingStatusValues;

        public List<ReadingStatus> ReadingStatusValues
        {
            get => _readingStatusValues;
            set
            {
                _readingStatusValues = value;
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

        public int? SelectedRating
        {
            get => _selectedRating;
            set
            {
                _selectedRating = value;
                OnPropertyChanged();
            }
        }


        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }


        public LibraryPageViewModel()
        {
            LoggedInUser = Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().LoggedInUser;
            ReadEntries = new ObservableCollection<V1LibraryEntry>();
            ToBeRead = new ObservableCollection<V1LibraryEntry>();
            CurrentlyReading = new ObservableCollection<V1LibraryEntry>();

            SelectedDate = DateTime.Now;

            // Populates the drop-down selector with ReadingStatus enums.
            ReadingStatusValues = new List<ReadingStatus>
            {
                ReadingStatus.Completed,
                ReadingStatus.ToRead,
                ReadingStatus.Reading
            };

            Ratings = new ObservableCollection<int>();
            // Populates the drow-down selector to allow a user to rate a book from 1 to 10.
            for (int i = 1; i <= 10; i++)
            {
                Ratings.Add(i);
            }
        }


        // Retrieves the library page of a specifi user (in this case the user that is currently logged in).
        public async Task GetUserLibraryAsync(V1User user)
        {
            try
            {

                string url = $"{_apiBaseUrl}/libraries/GetUserLibrary?userId={user.Id}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var json = await responseMessage.Content.ReadAsStringAsync();
                    V1LibraryCollection library = JsonConvert.DeserializeObject<V1LibraryCollection>(json);

                    CompleteLibrary = library;

                    // Empties the collections to avoid duplicate values appearing on multuiple page refreshes/visits.
                    ToBeRead.Clear();
                    CurrentlyReading.Clear();
                    ReadEntries.Clear();

                    // Adds the libraries to three differing arrays that can be filtered by the user.
                    foreach (V1LibraryEntry entry in library.Entries)
                    {
                        if (entry.ReadingStatus == ReadingStatus.Completed)
                        {
                            ReadEntries.Add(entry);
                        }
                        else if (entry.ReadingStatus == ReadingStatus.ToRead)
                        {
                            ToBeRead.Add(entry);
                        }
                        else if (entry.ReadingStatus == ReadingStatus.Reading)
                        {
                            CurrentlyReading.Add(entry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Retrieves a V1Book object based on an ISBN identifier. This is used to show additional information about said book when clicked on.
        public async Task<V1Book> GetBookWithEntryAsync(string isbn)
        {
            string url = $"{_apiBaseUrl}/books/GetByIsbn?isbn={isbn}";

            using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var json = await responseMessage.Content.ReadAsStringAsync();
                V1BooksDto bookSearch = new V1BooksDto(json);

                foreach (V1Book book in bookSearch.Books)
                {
                    if (book.ImageLinks != null && book.ImageLinks.ContainsKey("thumbnail"))
                    {
                        return book;
                    }
                }
            }

            return null;
        }

        // Populates selectors and page elements with the reading status, rating, and date of completion when added by the user at an earlier instance.
        public void PopulateSelectedEntryFields()
        {
            SelectedReadingStatus = SelectedEntry.ReadingStatus;


            if (SelectedEntry.Rating.HasValue)
            {
                SelectedRating = (int)SelectedEntry.Rating;
            }

            if (SelectedEntry.DateRead.HasValue)
            {
                SelectedDate = (DateTime)SelectedEntry.DateRead;
            }
        }

        public ICommand SaveChangesCommand => new Command(async () => await SaveChangesAsync());
        public ICommand DeleteEntryCommand => new Command(async () => await DeleteEntryAsync());
        public ICommand NavigateToBookPageCommand => new Command(async () => await NavigateToBookPageAsync(SelectedEntryBook));

        // Saves the information modified by the user and changes data in the database.
        public async Task SaveChangesAsync()
        {

            try
            {
                // Currently each modification requires its own API call. Could be improved by having a single API call to modify a book.
                string readingStatusUrl = $"{_apiBaseUrl}/libraries/ChangeReadingStatus?entryId={SelectedEntry.Id}&readingStatus={SelectedReadingStatus}";
                string ratingUrl = $"{_apiBaseUrl}/libraries/ChangeRating?entryId={SelectedEntry.Id}&rating={SelectedRating}";
                string dateUrl = $"{_apiBaseUrl}/libraries/ChangeDateRead?entryId={SelectedEntry.Id}&dateTime={SelectedDate}";

                HttpResponseMessage response = null;


                if (SelectedReadingStatus?.CompareTo(SelectedEntry.ReadingStatus) != 0)
                {
                    var jsonString = JsonConvert.SerializeObject(SelectedReadingStatus);
                    var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    response = await _httpClient.PutAsync(readingStatusUrl, httpContent);

                    SelectedEntry.ReadingStatus = (ReadingStatus)SelectedReadingStatus;
                }

                if (SelectedRating?.CompareTo(SelectedEntry.Rating) != 0)
                {
                    var jsonString = JsonConvert.SerializeObject(SelectedRating);
                    var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    response = await _httpClient.PutAsync(ratingUrl, httpContent);

                    SelectedEntry.Rating = SelectedRating;
                }

                if (SelectedDate?.CompareTo(SelectedEntry.DateRead) != 0)
                {
                    var jsonString = JsonConvert.SerializeObject(SelectedDate);
                    var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    response = await _httpClient.PutAsync(dateUrl, httpContent);

                    // Decrements the book's original reading goal by 1.
                    await UpdateReadingLibraryAsync(LoggedInUser.Id, (DateTime)SelectedEntry.DateRead, -1);
                    // Increments the book's new reading goal by 1.
                    await UpdateReadingLibraryAsync(LoggedInUser.Id, (DateTime)SelectedDate, 1);

                    SelectedEntry.DateRead = SelectedDate;
                }

                if (response != null && response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Success!", "Your changes have been saved.", "OK");
                    Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().IsUserLibraryAltered = true;
                    await LoadAsync();

                } else if (response == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Oops!", "You haven't made any changes.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Oops!", "Your changes could not be saved.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        // Removes an entry from a user's library
        public async Task DeleteEntryAsync()
        {
            try
            {
                if (SelectedEntry == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Oops!", "You haven't selected an entry.", "OK");
                    return;
                } else
                {
                    string deleteEntryUrl = $"{_apiBaseUrl}/libraries/DeleteEntry?entry={SelectedEntry.Id}";
                    var response = await _httpClient.DeleteAsync(deleteEntryUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success!", "The entry has been deleted.", "OK");
                        // Decrements the book's reading goal by 1.
                        await UpdateReadingLibraryAsync(LoggedInUser.Id, (DateTime)SelectedEntry.DateRead, -1);
                        // Prompts the main page to reload data after a change.
                        Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().IsUserLibraryAltered = true;
                        // 'Reloads' the current page.
                        await LoadAsync();
                        SelectedEntry = null;
                        SelectedReadingStatus = null;
                        SelectedDate = DateTime.Now;
                        SelectedRating = null;
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Oops!", "This entry no longer exists.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        // Used by DeleteEntryAsync() and SaveChangesAsync() to modify the increment count of the a user's reading goal.
        public async Task UpdateReadingLibraryAsync(Guid userId, DateTime dateTime, int amount)
        {
            string url = $"{_apiBaseUrl}/goals/GetGoalId?userId={userId}&GoalDate={dateTime.ToString("yyyy/MM/dd")}";

            HttpResponseMessage result = await _httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var libraryId = await result.Content.ReadAsStringAsync();

                Guid libraryIdGuid = JsonConvert.DeserializeObject<Guid>(libraryId);

                url = $"{_apiBaseUrl}/goals/IncrementReadingGoal?id={libraryIdGuid}&amount={amount}";

                var jsonString = JsonConvert.SerializeObject(new { id = libraryIdGuid, amount = amount });
                var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, httpContent);
            }
        }

        public void UpdateDate(DateTime dateTime)
        {
            SelectedDate = dateTime;
        }

        // Allows the user to navigate to a book's dedicated page (with additional information, comments etc) from their library.
        public async Task NavigateToBookPageAsync(V1Book book)
        {
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().SelectedBook = book;
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

        public async Task LoadAsync()
        {
            IsBusy = true;

            await GetUserLibraryAsync(LoggedInUser);

            IsBusy = false;
        }
    }
}