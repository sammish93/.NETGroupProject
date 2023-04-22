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
    public class MarketplacePageViewModel : BaseViewModel
    {

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private V1User _loggedInUser { get; set; }
        private V1Book _selectedBook;
        private ObservableCollection<V1Book> _bookSearch { get; set; }
        private bool _isBusy;


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

        public V1Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                OnPropertyChanged();

            }
        }

        public ObservableCollection<V1Book> BookSearch
        {
            get => _bookSearch;
            set
            {
                _bookSearch = value;
                OnPropertyChanged();

            }
        }


        public MarketplacePageViewModel()
        {
            LoggedInUser = App.LoggedInUser;

            BookSearch = new ObservableCollection<V1Book>();
        }


        public async Task GetBookSearch(string searchQuery)
        {
            try
            {
                BookSearch.Clear();

                await PopulateBookIsbnResults(searchQuery);
                await PopulateBookTitleResults(searchQuery);
                await PopulateBookAuthorResults(searchQuery);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task PopulateBookIsbnResults(string query)
        {

            try
            {
                var queryReplaced = query;
                queryReplaced.Replace(" ", "%20");
                string url = $"{_apiBaseUrl}/books/GetByIsbn?isbn={queryReplaced}&maxResults=40";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var json = await responseMessage.Content.ReadAsStringAsync();
                    var bookSearch = new V1BooksDto(json);

                    foreach (V1Book book in bookSearch.Books)
                    {
                        if (book.IndustryIdentifiers == null
                            || (!book.IndustryIdentifiers.ContainsKey("ISBN_10") && !book.IndustryIdentifiers.ContainsKey("ISBN_13")))
                        {
                            continue;
                        }

                        book.ImageLinks["smallThumbnail"].Replace("&", "&amp;");
                        book.ImageLinks["thumbnail"].Replace("&", "&amp;");
                        BookSearch.Add(book);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        public async Task PopulateBookTitleResults(string query)
        {

            try
            {
                var queryReplaced = query;
                queryReplaced.Replace(" ", "%20");
                string url = $"{_apiBaseUrl}/books/GetByTitle?title={queryReplaced}&maxResults=40";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                var bookSearch = new V1BooksDto(json);

                foreach (V1Book book in bookSearch.Books)
                {
                    if (book.IndustryIdentifiers == null
                        || (!book.IndustryIdentifiers.ContainsKey("ISBN_10") && !book.IndustryIdentifiers.ContainsKey("ISBN_13")))
                    {
                        continue;
                    }

                    book.ImageLinks["smallThumbnail"].Replace("&", "&amp;");
                    book.ImageLinks["thumbnail"].Replace("&", "&amp;");
                    BookSearch.Add(book);
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task PopulateBookAuthorResults(string query)
        {

            try
            {
                var queryReplaced = query;
                queryReplaced.Replace(" ", "%20");
                string url = $"{_apiBaseUrl}/books/GetByAuthor?name={queryReplaced}&maxResults=40";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                var bookSearch = new V1BooksDto(json);

                foreach (V1Book book in bookSearch.Books)
                {
                    if (book.IndustryIdentifiers == null
                        || (!book.IndustryIdentifiers.ContainsKey("ISBN_10") && !book.IndustryIdentifiers.ContainsKey("ISBN_13")))
                    {
                        continue;
                    }

                    book.ImageLinks["smallThumbnail"].Replace("&", "&amp;");
                    book.ImageLinks["thumbnail"].Replace("&", "&amp;");
                    BookSearch.Add(book);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public ICommand NavigateToBookPageCommand => new Command(async () => await NavigateToBookPage(SelectedBook));


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

        public async Task LoadAsync()
        {
            IsBusy = true;

            IsBusy = false;
        }
    }
}