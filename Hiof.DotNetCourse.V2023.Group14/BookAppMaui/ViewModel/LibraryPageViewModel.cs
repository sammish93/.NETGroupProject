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


namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class LibraryPageViewModel : BaseViewModel
    {

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";

        public V1LibraryCollection userLibrary { get; set; }

        public V1User LoggedInUser { get; set; }

        public ObservableCollection<V1Book> ReadBooks { get; set; }
        public ObservableCollection<V1Book> CurrentlyReadingBooks { get; set; }

        public ObservableCollection<V1Book> FutureReads { get; set; }
        


        public V1Book Book { get; set; }    

        public ObservableCollection<V1LibraryEntry> ReadEntries { get; set; }
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                
            }
        }

        public LibraryPageViewModel()
        {
            LoggedInUser = App.LoggedInUser;
            ReadBooks = new ObservableCollection<V1Book>();
            FutureReads = new ObservableCollection<V1Book>();
            CurrentlyReadingBooks = new ObservableCollection<V1Book>();


            ReadEntries = new ObservableCollection<V1LibraryEntry>();


        }


        public async Task PopulateBooks()
        {
            try
            {
               ReadEntries.Clear();

                string loginUrl = $"{_apiBaseUrl}/libraries/GetUserLibrary?userId={LoggedInUser.Id}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                V1LibraryCollection library = JsonConvert.DeserializeObject<V1LibraryCollection>(json);

                

                foreach (V1LibraryEntry entry in library.Entries)
                {
                    ReadEntries.Add(entry);
               

                    foreach(V1LibraryEntry e in ReadEntries)
                    {
                        string Isbn;
                        if (e.LibraryEntryISBN10 != null)
                        {
                            Isbn = e.LibraryEntryISBN10;
                        }
                        else if (e.LibraryEntryISBN13 != null)
                        {
                            Isbn = e.LibraryEntryISBN13;
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
                            
                            

                            



                        }


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
        public async Task LoadAsync()
        {
            IsBusy = true;
            await PopulateBooks();
            
            IsBusy = false;
        }
    }
}
       
    

