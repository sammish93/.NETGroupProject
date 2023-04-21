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


namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class LibraryPageViewModel : BaseViewModel
    {

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private V1User _loggedInUser { get; set; }
        private V1Book _book { get; set; }
        private V1LibraryEntryWithImage _selectedEntry;
        private V1LibraryCollection _completeLibrary { get; set; }
        private ObservableCollection<V1LibraryEntry> _readEntries { get; set; }
        private ObservableCollection<V1LibraryEntry> _toBeRead { get; set; }
        private ObservableCollection<V1LibraryEntry> _currentlyReading { get; set; }
        private bool _isBusy;

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

        public ReadingStatus SelectedReadingStatus
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


        public LibraryPageViewModel()
        {
            LoggedInUser = App.LoggedInUser;
            ReadEntries = new ObservableCollection<V1LibraryEntry>();
            ToBeRead = new ObservableCollection<V1LibraryEntry>();
            CurrentlyReading = new ObservableCollection<V1LibraryEntry>();

            SelectedDate = DateTime.Now;

            ReadingStatusValues = new List<ReadingStatus>
            {
                ReadingStatus.Completed,
                ReadingStatus.ToRead,
                ReadingStatus.Reading
            };

            Ratings = new ObservableCollection<int>();
            for (int i = 1; i <= 10; i++)
            {
                Ratings.Add(i);
            }
        }


        public async Task GetUserLibrary(V1User user)
        {
            try
            {

                string url = $"{_apiBaseUrl}/libraries/GetUserLibrary?userId={user.Id}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                V1LibraryCollection library = JsonConvert.DeserializeObject<V1LibraryCollection>(json);

                CompleteLibrary = library;

                foreach (V1LibraryEntry entry in library.Entries)
                {
                    if (entry.ReadingStatus == ReadingStatus.Completed)
                    {
                        ReadEntries.Add(entry);
                    } else if (entry.ReadingStatus == ReadingStatus.ToRead)
                    {
                        ToBeRead.Add(entry);
                    } else if (entry.ReadingStatus == ReadingStatus.Reading)
                    {
                        CurrentlyReading.Add(entry);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task<V1Book> GetBookWithEntryAsync(string isbn)
        {
            string url = $"{_apiBaseUrl}/books/GetByIsbn?isbn={isbn}";

            using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();
            var json = await responseMessage.Content.ReadAsStringAsync();
            V1BooksDto bookSearch = new V1BooksDto(json);

            foreach (V1Book book in bookSearch.Books)
            {
                if (book.ImageLinks != null && book.ImageLinks.ContainsKey("thumbnail"))
                {
                    return book;
                }
            }

            return null;
        }

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

        public async Task LoadAsync()
        {
            IsBusy = true;

            await GetUserLibrary(LoggedInUser);

            IsBusy = false;
        }
    }
}