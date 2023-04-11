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

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class MessagesViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiBaseUrl = "https://localhost:7268/proxy/1.0";
        private bool _isBusy;
        private V1User _loggedInUser;
        private ObservableCollection<V1UserWithDisplayPicture> _conversationParticipants;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
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

        public ObservableCollection<V1UserWithDisplayPicture> ConversationParticipants
        {
            get => _conversationParticipants;
            set
            {
                _conversationParticipants = value;
                OnPropertyChanged();
            }
        }

        public MessagesViewModel(V1User user)
        {
            ConversationParticipants = new ObservableCollection<V1UserWithDisplayPicture>();
            LoggedInUser = user;
        }

        public async Task PopulateUserResults(String username)
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
                        userWithDisplayPicture = new V1UserWithDisplayPicture(user, App.DefaultDisplayPicture);
                    }

                    ConversationParticipants.Add(userWithDisplayPicture);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task LoadAsync()
        {
            IsBusy = true;
            await PopulateUserResults(LoggedInUser.UserName);
            IsBusy = false;
        }
    }
}
