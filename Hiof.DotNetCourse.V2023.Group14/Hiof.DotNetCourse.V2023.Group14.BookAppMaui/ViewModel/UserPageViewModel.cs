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
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class UserPageViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private bool _isBusy;
        private V1User _user;
        private V1User _selectedUser;
        private byte[] _selectedUserDisplayPicture;
        private V1ReadingGoals _selectedUserRecentReadingGoal;
        private V1LibraryCollection _userLibrary;
        private ObservableCollection<V1Book> _userBooks;
        private ObservableCollection<V1ReadingGoals> _userReadingGoals;
        private ObservableCollection<V1Comments> _commentsOnUserPage;
        private DateTime _selectedStartDate;
        private DateTime _selectedEndDate;
        private string _goalTarget;
        private bool _isCommentButtonVisible;
        private bool _isReplyButtonVisible;
        private string _messagePlaceholder;
        private Guid _replyId;
        private string _commentEntry;


        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public bool IsCommentButtonVisible
        {
            get => _isCommentButtonVisible;
            set
            {
                _isCommentButtonVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsReplyButtonVisible
        {
            get => _isReplyButtonVisible;
            set
            {
                _isReplyButtonVisible = value;
                OnPropertyChanged();
            }
        }

        public V1LibraryCollection UserLibrary
        {
            get => _userLibrary;
            set
            {
                _userLibrary = value;
                OnPropertyChanged();
            }
        }

        public byte[] SelectedUserDisplayPicture
        {
            get => _selectedUserDisplayPicture;
            set
            {
                _selectedUserDisplayPicture = value;
                OnPropertyChanged();
            }
        }

        public V1ReadingGoals SelectedUserRecentReadingGoal
        {
            get => _selectedUserRecentReadingGoal;
            set
            {
                _selectedUserRecentReadingGoal = value;
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

        public V1User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<V1Book> UserBooks
        {
            get => _userBooks;
            set
            {
                _userBooks = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<V1ReadingGoals> UserReadingGoals
        {
            get => _userReadingGoals;
            set
            {
                _userReadingGoals = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<V1Comments> CommentsOnUserPage
        {
            get => _commentsOnUserPage;
            set
            {
                _commentsOnUserPage = value;
                OnPropertyChanged();
            }
        }

        public DateTime SelectedStartDate
        {
            get => _selectedStartDate;
            set
            {
                _selectedStartDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime SelectedEndDate
        {
            get => _selectedEndDate;
            set
            {
                _selectedEndDate = value;
                OnPropertyChanged();
            }
        }


        public string MessagePlaceholder
        {
            get => _messagePlaceholder;
            set
            {
                _messagePlaceholder = value;
                OnPropertyChanged();
            }
        }

        public Guid ReplyId
        {
            get => _replyId;
            set
            {
                _replyId = value;
                OnPropertyChanged();
            }
        }

        public string CommentEntry
        {
            get => _commentEntry;
            set => SetProperty(ref _commentEntry, value);
        }

        public string GoalTarget { get => _goalTarget; set => SetProperty(ref _goalTarget, value); }

        public UserPageViewModel(V1User loggedInUser, V1User selectedUser, byte[] selectedUserDisplayPicture)
        {
            User = loggedInUser;
            SelectedUser = selectedUser;
            IsCommentButtonVisible = true;
            IsReplyButtonVisible = false;
            MessagePlaceholder = "Enter a comment...";
            CommentsOnUserPage = new ObservableCollection<V1Comments>();
            UserBooks = new ObservableCollection<V1Book>();
            UserReadingGoals = new ObservableCollection<V1ReadingGoals>();
            CommentsOnUserPage = new ObservableCollection<V1Comments>();
            SelectedUserDisplayPicture = selectedUserDisplayPicture;

            SelectedStartDate = DateTime.Now;
            SelectedEndDate = DateTime.Now;
            GoalTarget = "0";
        }

        // Retrieves n amount of the highest rated books from a specific user's library. The GUI is responsively designed to support a maximum of 5 book items
        // being shown at the same time (though more can be shown using a horizontal scroll bar).
        // Also retrieves statistics based on the amount of books in a user's library, as well as amount of books completed.
        public async Task PopulateBooksAsync(V1User user, int nrOfResults)
        {

            try
            {
                UserBooks.Clear();

                string url = $"{_apiBaseUrl}/libraries/GetUserHighestRatedBooks?userId={user.Id}&numberOfResults={nrOfResults}";

                string libraryUrl = $"{_apiBaseUrl}/libraries/GetUserLibrary?userId={user.Id}";

                // API call to retrieve a maximum of 5 books that are rated highest in relation to the rest of a user's library.
                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                V1LibraryCollection library = JsonConvert.DeserializeObject<V1LibraryCollection>(json);

                // API call to retrieve amount of books read and other statistics.
                using HttpResponseMessage responseMessageLibrary = await _httpClient.GetAsync(libraryUrl);
                responseMessageLibrary.EnsureSuccessStatusCode();
                var jsonLibrary = await responseMessageLibrary.Content.ReadAsStringAsync();
                V1LibraryCollection libraryComplete = JsonConvert.DeserializeObject<V1LibraryCollection>(jsonLibrary);
                UserLibrary = libraryComplete;


                foreach (V1LibraryEntry entry in library.Entries)
                {
                    string Isbn;
                    if (entry.LibraryEntryISBN10 != null)
                    {
                        Isbn = entry.LibraryEntryISBN10;
                    }
                    else if (entry.LibraryEntryISBN13 != null)
                    {
                        Isbn = entry.LibraryEntryISBN13;
                    }
                    else
                    {
                        continue;
                    }

                    // API call to Google Books to retrieve thumbnail of a book's cover.
                    var loginUrlTwo = $"{_apiBaseUrl}/books/GetByIsbn?isbn={Isbn}";

                    using HttpResponseMessage responseMessageTwo = await _httpClient.GetAsync(loginUrlTwo);
                    responseMessageTwo.EnsureSuccessStatusCode();
                    var jsonTwo = await responseMessageTwo.Content.ReadAsStringAsync();
                    V1BooksDto bookSearch = new V1BooksDto(jsonTwo);

                    foreach (V1Book book in bookSearch.Books)
                    {
                        // Plaintext URL swaps symbols for html encoding.
                        book.ImageLinks["smallThumbnail"].Replace("&", "&amp;");
                        book.ImageLinks["thumbnail"].Replace("&", "&amp;");
                        UserBooks.Add(book);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        // Retrieves comments for a specific user's page.
        // Note: future versions of the Gui could include 'blog'-like posts that could be displayed separately from other comments, while also following similar
        public async Task PopulateCommentsAsync(V1User selectedUser)
        {

            try
            {
                CommentsOnUserPage.Clear();

                string url = $"{_apiBaseUrl}/comments/GetCommentsByUserId?id={selectedUser.Id}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                dynamic? jArrayReadingGoals = JsonConvert.DeserializeObject(json);

                foreach (JObject commentsJson in jArrayReadingGoals["response"])
                {
                    V1Comments comment = JsonConvert.DeserializeObject<V1Comments>(commentsJson.ToString());
                    // Retrieves replies from a specific comment.
                    comment = await PopulateCommentRepliesAsync(comment);
                    // Retrieves data relating to the author of the reply (username, display picture etc.)
                    comment.AuthorObject = await GetUserWithDisplayPictureAsync(comment.AuthorId.ToString());
                    CommentsOnUserPage.Add(comment);
                }
            }
            catch (Exception ex)
            {

            }
        }

        // Fetches replies linked to a single specific comment, as well as author object (username etc) and display picture.
        public async Task<V1Comments> PopulateCommentRepliesAsync(V1Comments selectedComments)
        {

            try
            {
                string url = $"{_apiBaseUrl}/comments/GetCommentById?id={selectedComments.Id}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                V1Comments comment = JsonConvert.DeserializeObject<V1Comments>(json);

                foreach (V1Comments reply in comment.Replies)
                {
                    // Retrieves data relating to the author of the reply (username, display picture etc.)
                    // Note: though the database supports multi-level replies (a reply to a reply), the GUI hasn't integrated it.
                    reply.AuthorObject = await GetUserWithDisplayPictureAsync(reply.AuthorId.ToString());
                }

                return comment;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        // Retrieves display picture of a specific user. If no display picture exists then a default display picture is provided.
        public async Task<V1UserWithDisplayPicture> GetUserWithDisplayPictureAsync(String guidString)
        {
            try
            {

                var guid = Guid.Parse(guidString);
                string loginUrl = $"{_apiBaseUrl}/users/GetById?id={guid}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();

                V1User user = JsonConvert.DeserializeObject<V1User>(json.ToString());

                V1UserWithDisplayPicture userWithDisplayPicture;


                string displayPictureUrl = $"{_apiBaseUrl}/icons/GetIconByName?username={user.UserName}";
                HttpResponseMessage resultDisplayPicture = await _httpClient.GetAsync(displayPictureUrl);

                if (resultDisplayPicture.IsSuccessStatusCode)
                {
                    var responseStringDisplayPicture = await resultDisplayPicture.Content.ReadAsStringAsync();

                    V1UserIcon displayPicture = JsonConvert.DeserializeObject<V1UserIcon>(responseStringDisplayPicture);

                    userWithDisplayPicture = new V1UserWithDisplayPicture(user, displayPicture.DisplayPicture);
                }
                else
                {
                    userWithDisplayPicture = new V1UserWithDisplayPicture(user, Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().DefaultDisplayPicture);
                }

                return userWithDisplayPicture;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        // Retrieves all reading goals sorted by date descending (newest goal first), and places them in a collection to be displayed via collectionview in the Gui,
        public async Task PopulateReadingGoalsAsync(V1User user)
        {

            try
            {
                UserReadingGoals.Clear();

                string loginUrl = $"{_apiBaseUrl}/goals/GetAllGoals?userId={user.Id}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(loginUrl);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();

                dynamic? jArrayReadingGoals = JsonConvert.DeserializeObject(json);

                foreach (JObject userJson in jArrayReadingGoals)
                {
                    V1ReadingGoals readingGoal = JsonConvert.DeserializeObject<V1ReadingGoals>(userJson.ToString());

                    if (readingGoal.Id != SelectedUserRecentReadingGoal.Id)
                    {
                        UserReadingGoals.Add(readingGoal);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        // Retrieves a display picture of a specific user. If no display picture exists then a default one is provided.
        public async Task GetSelectedUserDisplayPictureAsync(string username)
        {
            string displayPictureUrl = $"{_apiBaseUrl}/icons/GetIconByName?username={username}";
            HttpResponseMessage resultDisplayPicture = await _httpClient.GetAsync(displayPictureUrl);

            if (resultDisplayPicture.IsSuccessStatusCode)
            {
                var responseStringDisplayPicture = await resultDisplayPicture.Content.ReadAsStringAsync();

                V1UserIcon displayPicture = JsonConvert.DeserializeObject<V1UserIcon>(responseStringDisplayPicture);

                SelectedUserDisplayPicture = displayPicture.DisplayPicture;
            }
            else
            {
                SelectedUserDisplayPicture = Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().DefaultDisplayPicture;
            }
        }

        // Navigates to the book page when a book is selected (from a user's highest rated books). QueryProperty isn't implemented properly in the current 
        // version of Maui.
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

        // Retrieves the most recent (by date) reading goal. This is displayed in the GUI slightly larger than the other previous reading goals.
        public async Task GetMostRecentReadingGoalAsync(V1User user)
        {
            string url = $"{_apiBaseUrl}/goals/GetRecentGoal?userId={user.Id}";
            HttpResponseMessage result = await _httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var responseString = await result.Content.ReadAsStringAsync();

                V1ReadingGoals readingGoal = JsonConvert.DeserializeObject<V1ReadingGoals>(responseString);

                SelectedUserRecentReadingGoal = readingGoal;
            }
        }

        public void UpdateStartDate(DateTime dateTime)
        {
            SelectedStartDate = dateTime;
        }

        public void UpdateEndDate(DateTime dateTime)
        {
            SelectedEndDate = dateTime;
        }

        public ICommand AddNewReadingGoalCommand => new Command(async () => await AddNewReadingGoalAsync());

        // Retrieves data entered by a user in the entry forms, data selector etc, and creates a new reading goal.
        private async Task AddNewReadingGoalAsync()
        {
            try
            {
                string createReadingGoalUrl = $"{_apiBaseUrl}/goals/CreateReadingGoal";

                int goalTargetInt;

                // Checks that the data entered is an integer.
                bool isParsed = int.TryParse(GoalTarget, out goalTargetInt);

                if (isParsed)
                {
                    goalTargetInt = int.Parse(GoalTarget);

                    if (GoalTarget.Equals("0"))
                    {
                        // Displays a prompt if no reading goal target has been set.
                        await Application.Current.MainPage.DisplayAlert("Oops!", "You have forgotten to choose a reading goal target.", "OK");
                        return;
                    } else
                    {
                        var requestBody = new V1ReadingGoals(User, SelectedStartDate, SelectedEndDate, goalTargetInt, 0);

                        var requestBodyJson = JsonConvert.SerializeObject(requestBody);
                        var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await _httpClient.PostAsync(createReadingGoalUrl, requestContent);
                        if (response.IsSuccessStatusCode)
                        {
                            await Application.Current.MainPage.DisplayAlert("Success!", "You have added a new reading goal.", "OK");
                            // Prompts main page to do a full reload.
                            Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().IsUserLibraryAltered = true;
                            // Progress bar animation is run.
                            await LoadProgressBarAsync();
                            // Reading goals are refreshed.
                            await PopulateReadingGoalsAsync(SelectedUser);
                            IsBusy = false;
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Uh oh!", "You already have a reading goal during this time period.", "OK");
                        }
                    }
                } else
                {
                    await Application.Current.MainPage.DisplayAlert("Oops!", "Only integers are accepted as valid reading goal targets.", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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
                } else
                {
                    await Shell.Current.GoToAsync("messages");
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
                        } else if (participant.Participant.Equals(userReceiver.Id.ToString()))
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

        public ICommand ReplyCommand => new Command(async (e) => await ReplyAsync((V1Comments)e));

        // Hides all elements relating to sending a comment and replaces them with the reply equivalent (as well as changing the placeholder text to refer to a 
        // specific user).
        public async Task ReplyAsync(V1Comments comment)
        {
            IsCommentButtonVisible = false;
            IsReplyButtonVisible = true;
            MessagePlaceholder = $"Enter a reply to {comment.AuthorObject.User.UserName}...";
            ReplyId = comment.Id;
        }

        public ICommand UpvoteCommand => new Command(async (e) => await UpvoteAsync((V1Comments)e));

        // Allows a user to incrementally increase an upvote by 1.
        // Note: Functionality is currently very basic, and there exists no restrictions on how many times a single user can upvote.
        public async Task UpvoteAsync(V1Comments comment)
        {
            string url = $"{_apiBaseUrl}/comments/UpdateCommentUpvotes?id={comment.Id}&upvotes=1";

            var jsonString = JsonConvert.SerializeObject(new { id = comment.Id, upvotes = 1 });
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, httpContent);
            if (response.IsSuccessStatusCode)
            {
                await PopulateCommentsAsync(SelectedUser);
            }
        }

        public ICommand SendCommentCommand => new Command(async () => await SendCommentAsync(CommentEntry));

        // Allows the user to send a specific comment linked to a specific user.
        public async Task SendCommentAsync(string message)
        {
            string url = $"{_apiBaseUrl}/comments/CreateComment";

            if (message != null)
            {

                var requestBodyJson = JsonConvert.SerializeObject(new
                {
                    body = message,
                    createdAt = DateTime.UtcNow,
                    upvotes = 0,
                    authorId = Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().LoggedInUser.Id,
                    userId = SelectedUser.Id
                });
                var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    await PopulateCommentsAsync(SelectedUser);
                }
            }
        }

        public ICommand SendReplyCommand => new Command(async () => await SendReplyAsync(CommentEntry));

        // Allows a user to send a specific reply linked to a specific comment.
        public async Task SendReplyAsync(string message)
        {
            string url = $"{_apiBaseUrl}/comments/CreateReplyComment";

            if (message != null)
            {

                var requestBodyJson = JsonConvert.SerializeObject(new
                {
                    body = message,
                    createdAt = DateTime.UtcNow,
                    upvotes = 0,
                    authorId = Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().LoggedInUser.Id,
                    parentCommentId = ReplyId,
                    userId = SelectedUser.Id
                });
                var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    // Reloads the comments.
                    await PopulateCommentsAsync(SelectedUser);
                }
            }
        }

        public async Task LoadAsync()
        {
            IsBusy = true;
            await GetSelectedUserDisplayPictureAsync(SelectedUser.UserName);
            await PopulateCommentsAsync(SelectedUser);
            // 5 results are retrieved.
            await PopulateBooksAsync(SelectedUser, 5);
            await PopulateReadingGoalsAsync(SelectedUser);
            IsBusy = false;
        }

        public async Task LoadProgressBarAsync()
        {
            IsBusy = true;
            await GetMostRecentReadingGoalAsync(SelectedUser);
        }
    }
}
