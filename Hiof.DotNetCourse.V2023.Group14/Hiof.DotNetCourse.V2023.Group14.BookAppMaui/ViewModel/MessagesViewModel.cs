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
using System.Drawing;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class MessagesViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private bool _isBusy;
        private string _message;
        private V1User _loggedInUser;
        private ObservableCollection<V1UserWithDisplayPicture> _conversationParticipants;
        private ObservableCollection<V1ConversationModel> _conversations;
        private V1ConversationModel _selectedConversation;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
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

        public ObservableCollection<V1UserWithDisplayPicture> ConversationParticipants
        {
            get => _conversationParticipants;
            set
            {
                _conversationParticipants = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<V1ConversationModel> Conversations
        {
            get => _conversations;
            set
            {
                _conversations = value;
                OnPropertyChanged();
            }
        }
        public V1ConversationModel SelectedConversation
        {
            get => _selectedConversation;
            set
            {
                _selectedConversation = value;
                OnPropertyChanged();
            }
        }

        public MessagesViewModel(V1User user)
        {
            ConversationParticipants = new ObservableCollection<V1UserWithDisplayPicture>();
            Conversations = new ObservableCollection<V1ConversationModel>();
            LoggedInUser = user;
        }

        public async Task PopulateUserResultsAsync(String username)
        {

            try
            {
                ConversationParticipants.Clear();

                string url = $"{_apiBaseUrl}/users/GetUsersByName?name={username}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();

                dynamic? jArrayUsers = JsonConvert.DeserializeObject(json);

                foreach (JObject userJson in jArrayUsers)
                {
                    V1User user = JsonConvert.DeserializeObject<V1User>(userJson.ToString());
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

                    ConversationParticipants.Add(userWithDisplayPicture);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task PopulateConversationsAsync(V1User user)
        {
            Conversations.Clear();

            string url = $"{_apiBaseUrl}/messages/GetByParticipant?name={user.Id}";

            using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();
            var json = await responseMessage.Content.ReadAsStringAsync();

            dynamic? jArrayUsers = JsonConvert.DeserializeObject(json);

            foreach (JObject conversationsJson in jArrayUsers)
            {
                V1ConversationModel conversation = JsonConvert.DeserializeObject<V1ConversationModel>(conversationsJson.ToString());

                if (!conversation.Messages.IsNullOrEmpty())
                {
                    conversation.LastMessage = conversation.Messages.Last();
                    conversation.Messages.Reverse();
                }
                
                conversation.ParticipantsAsObjects = new List<V1UserWithDisplayPicture>();
                
                foreach (V1Participant participant in conversation.Participants)
                {
                    if (!participant.Participant.Equals(user.Id.ToString()))
                    {
                        conversation.ParticipantsAsObjects.Add(await GetUserWithDisplayPictureAsync(participant.Participant));
                    }
                }

                Conversations.Add(conversation);
            }
        }

        public async Task PopulateConversationAsync(string conversationId, V1User user)
        {

            string url = $"{_apiBaseUrl}/messages/GetByConversationId?conversationId={conversationId}";

            using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();
            var json = await responseMessage.Content.ReadAsStringAsync();

            dynamic? jArrayUsers = JsonConvert.DeserializeObject(json);


            V1ConversationModel conversation = JsonConvert.DeserializeObject<V1ConversationModel>(json);

            if (!conversation.Messages.IsNullOrEmpty())
            {
                conversation.LastMessage = conversation.Messages.Last();
                conversation.Messages.Reverse();
            }

            conversation.ParticipantsAsObjects = new List<V1UserWithDisplayPicture>();

            foreach (V1Participant participant in conversation.Participants)
            {
                if (!participant.Participant.Equals(user.Id.ToString()))
                {
                    conversation.ParticipantsAsObjects.Add(await GetUserWithDisplayPictureAsync(participant.Participant));
                }
            }

            PopulateMessagesWithUserMetadataAsync(conversation);
            SelectedConversation = conversation;
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

        public void PopulateMessagesWithUserMetadataAsync(V1ConversationModel conversation)
        {
            var loggedInUser = new V1UserWithDisplayPicture(LoggedInUser, Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().UserDisplayPicture);

            if (!conversation.Messages.IsNullOrEmpty())
            {
                foreach (V1Messages message in conversation.Messages)
                {
                    foreach (V1UserWithDisplayPicture user in conversation.ParticipantsAsObjects)
                    {
                        if (message.Sender.Equals(loggedInUser.User.Id.ToString()))
                        {
                            message.SenderObject = loggedInUser;
                            break;
                        } else if (message.Sender.Equals(user.User.Id.ToString()))
                        {
                            message.SenderObject = user;
                            break;
                        }
                    }
                    
                }
            }
        }

        public ICommand SendMessageCommand => new Command(async () => await SendMessageAsync(SelectedConversation.ConversationId.ToString(), LoggedInUser.Id.ToString()));

        public async Task SendMessageAsync(string conversationId, string sender)
        {
            var url = $"{_apiBaseUrl}/messages/AddMessageToConversation/?conversationId={conversationId}&sender={sender}";

            var requestBodyJson = JsonConvert.SerializeObject(Message);
            var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, requestContent);
            if (response.IsSuccessStatusCode)
            {
                await PopulateConversationAsync(conversationId, LoggedInUser);
                await UpdateIsRead(Guid.Parse(conversationId), sender, true);
                foreach (V1UserWithDisplayPicture receiver in SelectedConversation.ParticipantsAsObjects)
                {
                    await UpdateIsRead(Guid.Parse(conversationId), receiver.User.Id.ToString(), false);
                }
                Message = "";

            } else
            {
                await Application.Current.MainPage.DisplayAlert("Uh oh!", "Something went wrong.", "OK");
            }
        }

        public async Task UpdateIsRead(Guid conversationId, string participantId, bool isRead)
        {
            string url = $"{_apiBaseUrl}/messages/UpdateIsRead?conversationId={conversationId}&participantId={participantId}&isRead={isRead}";

            var requestContent = new StringContent(isRead.ToString(), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, requestContent);
        }
        public async Task LoadAsync()
        {
            IsBusy = true;
            await PopulateConversationsAsync(LoggedInUser);
            await PopulateUserResultsAsync(LoggedInUser.UserName);
            IsBusy = false;
        }
    }
}
