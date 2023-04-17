using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class LibraryEntryDetailViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private bool _isBusy;
        private V1User _user;
        private V1LibraryEntryWithImage _selectedEntry;

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

        public V1User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        public V1LibraryEntryWithImage SelectedEntry
        {
            get => _selectedEntry;
            set
            {
                _selectedEntry = value;
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
            get=> _readingStatusValues;
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

        public LibraryEntryDetailViewModel(V1User user, V1LibraryEntryWithImage selectedEntry)
        {
            User = user;
            SelectedEntry = selectedEntry;
            SelectedDate = DateTime.Now;

            ReadingStatusValues = new List<ReadingStatus>
            { 
                ReadingStatus.Completed,
                ReadingStatus.ToRead,
                ReadingStatus.Reading
            };
            SelectedReadingStatus = selectedEntry.ReadingStatus;
            Ratings = new ObservableCollection<int>();
            for (int i = 1; i <= 10; i++)
            {
                Ratings.Add(i);
            }
            if (selectedEntry.Rating.HasValue) 
            {
                SelectedRating = (int)selectedEntry.Rating; 
            }
        }
       

        private async Task<V1Book> GetBookImageUrlAsync(string isbn)
        {
            string loginUrlTwo = $"{_apiBaseUrl}/books/GetByIsbn?isbn={isbn}";

            using HttpResponseMessage responseMessageTwo = await _httpClient.GetAsync(loginUrlTwo);
            responseMessageTwo.EnsureSuccessStatusCode();
            var jsonTwo = await responseMessageTwo.Content.ReadAsStringAsync();
            V1BooksDto bookSearch = new V1BooksDto(jsonTwo);

            foreach (V1Book book in bookSearch.Books)
            {
                if (book.ImageLinks != null && book.ImageLinks.ContainsKey("thumbnail"))
                {
                    return book;
                }
            }

            return null;
        }

        public ICommand SaveChangesCommand => new Command(async () => await SaveChangesAsync());
        public ICommand DeleteEntryCommand => new Command(async () => await DeleteEntryAsync());
        public async Task SaveChangesAsync()
        {

            try
            {
                int pageCount = Shell.Current.Navigation.NavigationStack.Count;

                Debug.WriteLine(pageCount);

                string readingStatusUrl = $"{_apiBaseUrl}/libraries/ChangeReadingStatus?entryId={SelectedEntry.Id}&readingStatus={SelectedReadingStatus}";

                Debug.WriteLine(readingStatusUrl);
                string ratingUrl = $"{_apiBaseUrl}/libraries/ChangeRating?entryId={SelectedEntry.Id}&rating={SelectedRating}";

               
                string dateUrl = $"{_apiBaseUrl}/libraries/ChangeDateRead?entryId={SelectedEntry.Id}&dateTime={SelectedDate}";

                HttpResponseMessage response = null;

                
                if (SelectedReadingStatus != null)
                {
                    var jsonString = JsonConvert.SerializeObject(SelectedReadingStatus);
                    var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    response = await _httpClient.PutAsync(readingStatusUrl, httpContent);
                }
                
                else if (SelectedRating != 0)
                {
                    var jsonString = JsonConvert.SerializeObject(SelectedRating);
                    var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    response = await _httpClient.PutAsync(ratingUrl, httpContent);
                }
                
                else if (SelectedDate != null)
                {
                    var jsonString = JsonConvert.SerializeObject(SelectedDate);
                    var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    response = await _httpClient.PutAsync(dateUrl, httpContent);
                }

                if (response != null && response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Success!", "Update successful.", "OK");
                    await Shell.Current.Navigation.PopAsync();
                   
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to update.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }


        }

        public async Task DeleteEntryAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_apiBaseUrl))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "API base URL is not set.", "OK");
                    return;
                }

                if (SelectedEntry == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Selected entry is null.", "OK");
                    return;
                }

                string deleteEntryUrl = $"{_apiBaseUrl}/libraries/DeleteEntry?entry={SelectedEntry.Id}";
                var response = await _httpClient.DeleteAsync(deleteEntryUrl);
                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Success!", "Deleted successfully.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to update.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task BackAsync()
        {
            await Shell.Current.GoToAsync("///");
        }

        public async Task LoadAsync()
        {
            IsBusy = true;
            
            IsBusy = false;
        }


    }
}
