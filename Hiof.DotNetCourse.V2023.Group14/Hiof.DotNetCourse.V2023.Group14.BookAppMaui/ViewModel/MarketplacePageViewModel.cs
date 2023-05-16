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
using Newtonsoft.Json.Linq;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class MarketplacePageViewModel : BaseViewModel
    {

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private V1User _loggedInUser;
        private V1Book _selectedBook;
        private V1User _selectedUser;
        private ObservableCollection<V1Book> _bookSearch;
        private ObservableCollection<V1MarketplaceBookResponse> _bookPosts;
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

        public V1User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
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

        public ObservableCollection<V1MarketplaceBookResponse> BookPosts
        {
            get => _bookPosts;
            set
            {
                _bookPosts = value;
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
            // Hides the 'nested' page until a book is selected.
            IsBuyAndSellButtonsVisible = false;
            IsSellGridVisible = false;
            IsBuyGridVisible = false;
            BookSearch = new ObservableCollection<V1Book>();
            BookPosts = new ObservableCollection<V1MarketplaceBookResponse>();

            // Adds supported currencies. Additional currencies can easily be added in future versions.
            CurrencyValues = new ObservableCollection<V1Currency>();
            CurrencyValues.Add(V1Currency.EUR);
            CurrencyValues.Add(V1Currency.NOK);
            CurrencyValues.Add(V1Currency.SEK);
            CurrencyValues.Add(V1Currency.USD);
        }


        public async Task GetBookSearchAsync(string searchQuery)
        {
            try
            {
                BookSearch.Clear();

                await PopulateBookIsbnResultsAsync(searchQuery);
                await PopulateBookTitleResultsAsync(searchQuery);
                await PopulateBookAuthorResultsAsync(searchQuery);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task PopulateBookIsbnResultsAsync(string query)
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

        public async Task PopulateBookPostsAsync()
        {
            try
            {
                string isbn = "";
                if (SelectedBook.IndustryIdentifiers["ISBN_13"] != null)
                {
                    isbn = SelectedBook.IndustryIdentifiers["ISBN_13"];
                }
                else if (SelectedBook.IndustryIdentifiers["ISBN_10"] != null)
                {
                    isbn = SelectedBook.IndustryIdentifiers["ISBN_10"];
                }

                string url = $"{_apiBaseUrl}/marketplace/GetPostByIsbn?isbn={isbn}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var json = await responseMessage.Content.ReadAsStringAsync();
                    dynamic? jArrayBookPosts = JsonConvert.DeserializeObject(json);

                    foreach (JObject postObject in jArrayBookPosts)
                    {
                        V1MarketplaceBookResponse post = JsonConvert.DeserializeObject<V1MarketplaceBookResponse>(postObject.ToString());
                        post.OwnerObject = await GetUserAsync(post.OwnerId.ToString());
                        BookPosts.Add(post);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<V1User> GetUserAsync(String guidString)
        {
            try
            {

                var guid = Guid.Parse(guidString);
                string loginUrl = $"{_apiBaseUrl}/users/GetById?id={guid}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var json = await responseMessage.Content.ReadAsStringAsync();
                    V1User user = JsonConvert.DeserializeObject<V1User>(json.ToString());

                    return user;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }


        public async Task PopulateBookTitleResultsAsync(string query)
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

        public async Task PopulateBookAuthorResultsAsync(string query)
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

        public ICommand SendMessageCommand => new Command(async () => await SendMessageAsync(Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().LoggedInUser, SelectedUser));

        // A user can start a conversation with another user by clicking on a 'send message' button. This method is called as a response to that button press.
        // In the event that the user hasn't previously had a conversation with another, a conversation is created and saved to the database, and the user is then 
        // redirected to the messages page.
        // In the event that the user has previously had contact with another user then the user is redirected to the messages page.
        private async Task SendMessageAsync(V1User userSender, V1User userRecipient)
        {
            try
            {
                if (userSender.Id == userRecipient.Id)
                {
                    await Application.Current.MainPage.DisplayAlert("Uh oh!", "You can't send a message to yourself.", "OK");
                } else
                {
                    bool isConversationExist = await GetExistingConversationAsync(userSender, userRecipient);

                    // Creates a new conversation and saves it to the database.
                    if (!isConversationExist)
                    {
                        var conversationId = Guid.NewGuid();

                        string url = $"{_apiBaseUrl}/messages/CreateNewConversation?conversationId={conversationId}";

                        List<string> participants = new List<string>
                        {
                            userSender.Id.ToString(),
                            userRecipient.Id.ToString()
                        };

                        var requestBodyJson = JsonConvert.SerializeObject(participants);
                        var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await _httpClient.PostAsync(url, requestContent);
                        if (response.IsSuccessStatusCode)
                        {
                            await Shell.Current.GoToAsync("messages");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Uh oh!", "Something went wrong.", "OK");
                        }
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("messages");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        // Checks to see if a user has previously had contact with another user. This is used by the SendMessageAsync function.
        private async Task<bool> GetExistingConversationAsync(V1User userSender, V1User userReceiver)
        {
            try
            {
                string url = $"{_apiBaseUrl}/messages/GetByParticipant?name={userSender.Id}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();

                dynamic? jArrayConversations = JsonConvert.DeserializeObject(json);

                foreach (JObject conversationsJson in jArrayConversations)
                {
                    V1ConversationModel conversation = JsonConvert.DeserializeObject<V1ConversationModel>(conversationsJson.ToString());

                    bool includesSender = false;
                    bool includesReceiver = false;

                    foreach (V1Participant participant in conversation.Participants)
                    {
                        if (participant.Participant.Equals(userSender.Id.ToString()))
                        {
                            includesSender = true;
                        }
                        else if (participant.Participant.Equals(userReceiver.Id.ToString()))
                        {
                            includesReceiver = true;
                        }
                    }

                    if (includesSender && includesReceiver)
                    {
                        // Breaks foreach iteration if result is found.
                        return true;
                    }

                }

                // If no conversation is found then the method returns a false boolean value.
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return false;
        }

        public ICommand NavigateToBookPageCommand => new Command(async () => await NavigateToBookPageAsync(SelectedBook));


        public async Task NavigateToBookPageAsync(V1Book book)
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

            await Shell.Current.GoToAsync($"book?bookid={bookId}");
        }

        public ICommand SellCommand => new Command(async () => await SellAsync());


        public async Task SellAsync()
        {
            IsBuyAndSellButtonsVisible = false;
            IsSellGridVisible = true;
        }

        public ICommand BuyCommand => new Command(async () => await BuyAsync());


        public async Task BuyAsync()
        {
            await PopulateBookPostsAsync();
            IsBuyAndSellButtonsVisible = false;
            IsBuyGridVisible = true;
        }

        public ICommand CreateAdCommand => new Command(async () => await CreateAdAsync());


        public async Task CreateAdAsync()
        {
            var url = $"{_apiBaseUrl}/marketplace/CreateNewPost?ownerId={LoggedInUser.Id}&currency={SelectedCurrency}&status={V1BookStatus.UNSOLD}";

            var requestBodyJson = JsonConvert.SerializeObject(new { 
                condition = Condition, 
                price = Price, 
                isbN10 = SelectedBook.IndustryIdentifiers["ISBN_10"] ?? null, 
                isbN13 = SelectedBook.IndustryIdentifiers["ISBN_13"] ?? null});

            var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, requestContent);
            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Success!", "You have successfully created an ad.", "OK");

            } else
            {
                await Application.Current.MainPage.DisplayAlert("Uh oh!", "Something went wrong.", "OK");
            }

            IsSellGridVisible = false;
            IsBuyGridVisible = true;
        }

        public ICommand BackCommand => new Command(async () => await BackAsync());


        public async Task BackAsync()
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