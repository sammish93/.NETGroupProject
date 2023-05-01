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
using System.Globalization;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class BookPageViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private bool _isBusy;
        private V1User _user;
        private V1Book _selectedBook;
        private ObservableCollection<ReadingStatus> _readingStatuses;
        private ObservableCollection<V1Comments> _commentsOnUserPage;
        private ReadingStatus _selectedReadingStatus;
        private ObservableCollection<int> _ratings;
        private int _selectedRating;
        private DateTime _selectedDate;
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

        public V1Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
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

        public ObservableCollection<int> Ratings
        {
            get => _ratings;
            set
            {
                _ratings = value;
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


        public BookPageViewModel(V1User user, V1Book book)
        {
            User = user;
            SelectedBook = book;

            IsCommentButtonVisible = true;
            IsReplyButtonVisible = false;
            MessagePlaceholder = "Enter a comment...";

            CommentsOnUserPage = new ObservableCollection<V1Comments>();
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

                int? rating = null; 
                if (SelectedReadingStatus == ReadingStatus.Completed)
                {
                    if (SelectedRating == 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Oops!", "You have forgotten to choose a rating.", "OK");
                        return; 
                    }
                    rating = SelectedRating; 

                }

                bool isBookInLibraryResult = await isBookInLibrary(SelectedBook);
                bool answer = false;

                if (isBookInLibraryResult)
                {
                    answer = await Application.Current.MainPage.DisplayAlert("This book is already in your library", "Would you like to add it an additional time?", "Yes", "No");
                }

                if (answer || !isBookInLibraryResult)
                {
                    var requestBody = new V1LibraryEntry(User, SelectedBook, rating, SelectedDate, SelectedReadingStatus);

                    var requestBodyJson = JsonConvert.SerializeObject(requestBody);
                    var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _httpClient.PostAsync(createLibEntryUrl, requestContent);
                    if (response.IsSuccessStatusCode)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success!", "You have added this book to your library.", "OK");
                        Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().IsUserLibraryAltered = true;
                        if (SelectedReadingStatus == ReadingStatus.Completed)
                        {
                            await UpdateReadingLibrary(User.Id, SelectedDate);
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Uh oh!", "Something went wrong.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task<bool> isBookInLibrary(V1Book book)
        {
            string isbn = "";
            if (book.IndustryIdentifiers["ISBN_13"] != null)
            {
                isbn = book.IndustryIdentifiers["ISBN_13"];
            } else if (book.IndustryIdentifiers["ISBN_10"] != null)
            {
                isbn = book.IndustryIdentifiers["ISBN_10"];
            }
            
            string url = $"{_apiBaseUrl}/libraries/GetEntryFromSpecificUser?userId={User.Id}&isbn={isbn}";

            HttpResponseMessage result = await _httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var libraryEntry = await result.Content.ReadAsStringAsync();

                if (!libraryEntry.IsNullOrEmpty()) 
                { 
                    return true; 
                }
                
            }

            return false;
        }

        public void UpdateDate(DateTime dateTime)
        {
            SelectedDate = dateTime;
        }

        public async Task PopulateComments(V1Book selectedBook)
        {

            try
            {
                CommentsOnUserPage.Clear();

                string Isbn = "";
                if (selectedBook.IndustryIdentifiers["ISBN_10"] != null)
                {
                    Isbn = selectedBook.IndustryIdentifiers["ISBN_10"];
                }
                else if (selectedBook.IndustryIdentifiers["ISBN_13"] != null)
                {
                    Isbn = selectedBook.IndustryIdentifiers["ISBN_13"];
                }

                string url = $"{_apiBaseUrl}/comments/GetCommentsByISBN?isbn={Isbn}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                dynamic? jArrayReadingGoals = JsonConvert.DeserializeObject(json);

                foreach (JObject commentsJson in jArrayReadingGoals["response"])
                {
                    V1Comments comment = JsonConvert.DeserializeObject<V1Comments>(commentsJson.ToString());
                    comment = await PopulateCommentReplies(comment);
                    comment.AuthorObject = await GetUserWithDisplayPictureAsync(comment.AuthorId.ToString());
                    CommentsOnUserPage.Add(comment);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<V1Comments> PopulateCommentReplies(V1Comments selectedComments)
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
                    reply.AuthorObject = await GetUserWithDisplayPictureAsync(reply.AuthorId.ToString());
                }

                return comment;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

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

        public async Task UpdateReadingLibrary(Guid userId, DateTime dateTime)
        {
            string url = $"{_apiBaseUrl}/goals/GetGoalId?userId={userId}&GoalDate={dateTime.ToString("yyyy/MM/dd")}";

            HttpResponseMessage result = await _httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var libraryId = await result.Content.ReadAsStringAsync();

                Guid libraryIdGuid = JsonConvert.DeserializeObject<Guid>(libraryId);

                int amount = 1;
                url = $"{_apiBaseUrl}/goals/IncrementReadingGoal?id={libraryIdGuid}&amount={amount}";
                
            }
        }

        public ICommand ReplyCommand => new Command(async (e) => await Reply((V1Comments)e));

        public async Task Reply(V1Comments comment)
        {
            IsCommentButtonVisible = false;
            IsReplyButtonVisible = true;
            MessagePlaceholder = $"Enter a reply to {comment.AuthorObject.User.UserName}...";
            ReplyId = comment.Id;
        }

        public ICommand UpvoteCommand => new Command(async (e) => await Upvote((V1Comments)e));

        public async Task Upvote(V1Comments comment)
        {
            string url = $"{_apiBaseUrl}/comments/UpdateCommentUpvotes?id={comment.Id}&upvotes=1";

            var jsonString = JsonConvert.SerializeObject(new { id = comment.Id, upvotes = 1 });
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, httpContent);
            if (response.IsSuccessStatusCode)
            {
                await LoadAsync();
            }
        }

        public ICommand SendCommentCommand => new Command(async () => await SendComment(CommentEntry));

        public async Task SendComment(string message)
        {
            string url = $"{_apiBaseUrl}/comments/CreateBookComment";

            if (message != null)
            {
                var requestBody = new V1Comments
                {
                    Id = Guid.NewGuid(),
                    Body = message,
                    CreatedAt = DateTime.Now,
                    AuthorId = Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().LoggedInUser.Id,
                    Upvotes = 0,
                    CommentType = ClassLibrary.Enums.V1.CommentType.Book,
                    ISBN10 = SelectedBook.IndustryIdentifiers["ISBN_10"],
                    ISBN13 = SelectedBook.IndustryIdentifiers["ISBN_13"]
                };

                var requestBodyJson = JsonConvert.SerializeObject(requestBody);
                var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    await LoadAsync();
                }
            }
        }

        public async Task LoadAsync()
        {
            IsBusy = true;
            await PopulateComments(SelectedBook);
            IsBusy = false;
        }
    }
}
