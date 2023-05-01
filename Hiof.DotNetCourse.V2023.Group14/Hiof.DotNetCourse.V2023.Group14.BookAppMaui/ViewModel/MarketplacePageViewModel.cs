﻿using System;
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
        private V1User _loggedInUser;
        private V1Book _selectedBook;
        private ObservableCollection<V1Book> _bookSearch;
        private string _condition;
        private decimal _price;
        private ObservableCollection<V1Currency> _currencyValues;
        private V1Currency _selectedCurrency;
        private bool _isBusy;
        private bool _isBuyAndSellButtonsVisible;
        private bool _isSellGridVisible;
        private bool _isBuyGridVisible;


        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;

            }
        }

        public bool IsBuyAndSellButtonsVisible
        {
            get => _isBuyAndSellButtonsVisible;
            set
            {
                SetProperty(ref _isBuyAndSellButtonsVisible, value);

            }
        }

        public bool IsSellGridVisible
        {
            get => _isSellGridVisible;
            set
            {
                SetProperty(ref _isSellGridVisible, value);

            }
        }

        public bool IsBuyGridVisible
        {
            get => _isBuyGridVisible;
            set
            {
                SetProperty(ref _isBuyGridVisible, value);

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

        public string Condition { get => _condition; set => SetProperty(ref _condition, value); }
        public decimal Price { get => _price; set => SetProperty(ref _price, value); }

        public ObservableCollection<V1Currency> CurrencyValues
        {
            get => _currencyValues;
            set
            {
                _currencyValues = value;
                OnPropertyChanged();
            }
        }

        public V1Currency SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                _selectedCurrency = value;
                OnPropertyChanged();
            }
        }


        public MarketplacePageViewModel()
        {
            LoggedInUser = Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().LoggedInUser;
            IsBuyAndSellButtonsVisible = false;
            IsSellGridVisible = false;
            IsBuyGridVisible = false;
            BookSearch = new ObservableCollection<V1Book>();

            CurrencyValues = new ObservableCollection<V1Currency>();
            CurrencyValues.Add(V1Currency.EUR);
            CurrencyValues.Add(V1Currency.NOK);
            CurrencyValues.Add(V1Currency.SEK);
            CurrencyValues.Add(V1Currency.USD);
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
            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedBook = book;
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

        public ICommand SellCommand => new Command(async () => await Sell());


        public async Task Sell()
        {
            IsBuyAndSellButtonsVisible = false;
            IsSellGridVisible = true;
        }

        public ICommand BuyCommand => new Command(async () => await Buy());


        public async Task Buy()
        {
            IsBuyAndSellButtonsVisible = false;
            IsBuyGridVisible = true;
        }

        public ICommand CreateAdCommand => new Command(async () => await CreateAd());


        public async Task CreateAd()
        {

            await Application.Current.MainPage.DisplayAlert("Success!", "You have successfully created an ad.", "OK");

            IsSellGridVisible = false;
            IsBuyGridVisible = true;

        }

        public ICommand BackCommand => new Command(async () => await Back());


        public async Task Back()
        {
            IsBuyGridVisible = false;
            IsSellGridVisible = false;
            IsBuyAndSellButtonsVisible = true;
        }

        public async Task LoadAsync()
        {
            IsBusy = true;

            IsBusy = false;
        }
    }
}