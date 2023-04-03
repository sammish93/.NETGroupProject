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

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class BookPageViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private bool _isBusy;
        public V1User User { get; set; }
        private V1Book _selectedBook;
        public ObservableCollection<ReadingStatus> ReadingStatuses { get; set; }
        public ReadingStatus SelectedReadingStatus { get; set; }
        public ObservableCollection<int> Ratings { get; set; }
        public int SelectedRating { get; set; }
        public DateTime SelectedDate { get; set; }

        public V1Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public BookPageViewModel( V1Book book)
        {
            User = UserSingleton.Instance.GetUser(true);
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

                
                if (SelectedReadingStatus == ReadingStatus.Completed)
                {
                    
                    if (SelectedRating == 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Oops!", "You have forgotten to choose a rating.", "OK");
                        return;
                    }
                }

                var requestBody = new V1LibraryEntry(User, SelectedBook, SelectedRating, SelectedDate, SelectedReadingStatus);

                var requestBodyJson = JsonConvert.SerializeObject(requestBody);
                var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(createLibEntryUrl, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Success!", "You have added this book to your library.", "OK");
                    App.IsUserLibraryAltered = true;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Uh oh!", "Something went wrong.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void UpdateDate(DateTime dateTime)
        {
            SelectedDate = dateTime;
        }

        public async Task LoadAsync()
        {
            IsBusy = true;

            IsBusy = false;
        }
    }
}
