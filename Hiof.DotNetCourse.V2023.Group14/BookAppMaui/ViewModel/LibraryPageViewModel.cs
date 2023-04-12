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

        public V1User LoggedInUser { get; set; } 
        public V1Book Book { get; set; }    
        public ReadingStatus readingStatus { get; set; }

        private Button SaveButton;

        public ObservableCollection<V1LibraryEntryWithImage> ReadEntries { get; set; }
        public ObservableCollection<V1LibraryEntryWithImage> ToBeRead { get; set; }
        public ObservableCollection<V1LibraryEntryWithImage> CurrentlyReading { get; set; }
        private bool _isBusy;
        private bool _isVisible;

        public bool IsVisible
        {
            get => _isVisible;
            set => _isVisible = value;
        }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                
            }
        }

    

        private V1LibraryEntryWithImage _selectedEntry;
        public V1LibraryEntryWithImage SelectedEntry
        {
            get { return _selectedEntry; }
            set
            {
                _selectedEntry = value;
                OnPropertyChanged();
                
            }
        }
     



        public LibraryPageViewModel()
        {
            LoggedInUser = App.LoggedInUser;
            ReadEntries = new ObservableCollection<V1LibraryEntryWithImage>();
            ToBeRead = new ObservableCollection<V1LibraryEntryWithImage>();
            CurrentlyReading = new ObservableCollection<V1LibraryEntryWithImage>();

        }
        public async Task PopulateBooks()
        {
            try
            {
                ReadEntries.Clear();
                ToBeRead.Clear();
                CurrentlyReading.Clear();

                string loginUrl = $"{_apiBaseUrl}/libraries/GetUserLibrary?userId={LoggedInUser.Id}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                V1LibraryCollection library = JsonConvert.DeserializeObject<V1LibraryCollection>(json);

                foreach (V1LibraryEntry entry in library.Entries)
                {
                    string isbn = entry.LibraryEntryISBN10 ?? entry.LibraryEntryISBN13;
                    if (string.IsNullOrEmpty(isbn))
                    {
                        continue;
                    }

                    var book = await GetBookImageUrlAsync(isbn);
                    if (book == null)
                    {
                        continue;
                    }

                    var imageUrl = book.ImageLinks["thumbnail"];
                    
                    var entryWithImage = new V1LibraryEntryWithImage(
                        entry.Id,
                        entry.Title,
                        entry.MainAuthor,
                        entry.Rating,
                        entry.ReadingStatus,
                        
                        imageUrl
                    );
                    CurrentlyReading.Add(entryWithImage);
                    /*
                    
                    if(entryWithImage.ReadingStatus == ReadingStatus.Completed)
                        ReadEntries.Add(entryWithImage);
                    else if(entryWithImage.ReadingStatus == ReadingStatus.ToRead)
                    {
                        ToBeRead.Add(entryWithImage);
                    }
                    else if(entryWithImage.ReadingStatus == ReadingStatus.Reading)
                    {
                        CurrentlyReading.Add(entryWithImage);
                    }
                   */

                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
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

        public async Task NavigateToBookPage(V1Book book)
        {
            App.SelectedBook= book;

            
        }

        public async Task LoadAsync()
        {
            IsBusy = true;
            
            IsVisible = false;
           
            await PopulateBooks();
            
            IsBusy = false;
        }
    }
}
       
    

