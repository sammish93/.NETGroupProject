using CommunityToolkit.Maui.Animations;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.Maui.Controls;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui
{
    public partial class AppShell : Shell
    {
        private IDispatcherTimer _userTypingTimer;
        private bool _isNewMessageReceived = false;
        private bool _isNewMessageReceivedAlertBeenShown = false;
        private readonly HttpClient _httpClient = new HttpClient();

        public IDispatcherTimer UserTypingTimer
        {
            get => _userTypingTimer;
            set
            {
                _userTypingTimer = value;

            }
        }

        public bool IsNewMessageReceived
        {
            get => _isNewMessageReceived;
            set
            {
                _isNewMessageReceived = value;
            }
        }

        public bool IsNewMessageReceivedAlertBeenShown
        {
            get => _isNewMessageReceivedAlertBeenShown;
            set
            {
                _isNewMessageReceivedAlertBeenShown = value;
            }
        }

        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("login", typeof(LogInPage));
            Routing.RegisterRoute("signup", typeof(SignupPage));
            Routing.RegisterRoute("home", typeof(MainPage));
            Routing.RegisterRoute("book", typeof(BookPage));
            Routing.RegisterRoute("search", typeof(SearchPage));
            Routing.RegisterRoute("user", typeof(UserPage));
            Routing.RegisterRoute("library", typeof(LibraryPage));
            Routing.RegisterRoute("marketplace", typeof(MarketplacePage));
            Routing.RegisterRoute("messages", typeof(MessagesPage));
            Routing.RegisterRoute("settings", typeof(SettingsPage));

            this.BindingContext = new AppShellViewModel();
        }

        // Sets the Shell TitleView title to the current page title when it has been nagivated to.
        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {

            var model = BindingContext as AppShellViewModel;

            if (model != null)
            {
                var currentPage = Shell.Current.CurrentItem.Title;
;
                if (currentPage != null)
                {
                    model.TitleCurrentPage = currentPage;
                    model.User = Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().LoggedInUser;
                    model.DisplayPicture = Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().UserDisplayPicture;
                }
            }

            if (UserTypingTimer == null)
            {
                UserTypingTimer = Application.Current.Dispatcher.CreateTimer();
                // The Gui waits 5 seconds (5000 milliseconds) until it checks for a new message.
                UserTypingTimer.Interval = TimeSpan.FromMilliseconds(5000);
                UserTypingTimer.Start();
            }

            UserTypingTimer.Tick += (s, e) =>
            {
                // This happens every 5 seconds.
                OnTypingTimerElapsed(s, e);
            };

            base.OnNavigated(args);
        }

        // This method is called every n seconds, based on the Timer object's interval.
        private async void OnTypingTimerElapsed(object sender, EventArgs e)
        {
            if (Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().LoggedInUser != null)
            {
                var messagesLogo = this.FindByName<ImageButton>("messagesLogo");
                var fade = new FadeAnimation();

                // Checks to see if there's a new message
                IsNewMessageReceived = await PerformNewMessageCheck(Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().LoggedInUser);

                // The (IsNewMessageReceivedAlertBeenShown == false) are there to ensure that the shell doesn't 'flicker' every 5 seconds, and only updates the image 
                // when it hasn't.
                if (IsNewMessageReceived)
                {
                    if (IsNewMessageReceivedAlertBeenShown == false)
                    {
                        // Updates the message icon if not already done before.
                        messagesLogo.Source = "messages_large_unread_message.png";
                        IsNewMessageReceivedAlertBeenShown = true;
                        // Animates the change from one icon to another.
                        await fade.Animate(messagesLogo);
                    }
                } else
                {
                    if (!messagesLogo.Source.Equals("messages_large.png"))
                    {
                        if (IsNewMessageReceivedAlertBeenShown == true)
                        {
                            // Changes the logo back to the original (without new message alert) if all messages are now read.
                            messagesLogo.Source = "messages_large.png";
                            IsNewMessageReceivedAlertBeenShown = false;

                            // Animates the change from one icon to another.
                            await fade.Animate(messagesLogo);
                        }
                    }
                }
            }
        }

        // Method called every n seconds based on a Timer object's interval.
        private async Task<bool> PerformNewMessageCheck(V1User user)
        {
            try
            {
                // Checks to see if the logged in user has any unread messages.
                string url = $"https://localhost:7125/api/BackgroundJob/MessageChecker/NewMessages/{user.Id}";

                using HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    // Returns true if so.
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }       
    }
}