using CommunityToolkit.Maui.Animations;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Controls;
using System.ComponentModel;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui
{
    public partial class AppShell : Shell
    {
        private bool _isNewMessageReceived = false;
        private bool _isNewMessageReceivedAlertBeenShown = false;
        private readonly HttpClient _httpClient = new HttpClient();
        private HubConnection _hubConnection;
        private IDispatcherTimer _messageCheckerTimer;

        public IDispatcherTimer MessageCheckerTimer
        {
            get => _messageCheckerTimer;
            set
            {
                _messageCheckerTimer = value;

            }
        }

        public HubConnection HubConnection
        {
            get => _hubConnection;
            set
            {
                _hubConnection = value;

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
        protected override async void OnNavigated(ShellNavigatedEventArgs args)
        {

            var model = BindingContext as AppShellViewModel;

            if (model != null)
            {
                var currentPage = Shell.Current.CurrentItem.Title;
                ;
                if (currentPage != null)
                {
                    model.TitleCurrentPage = currentPage;
                    model.User = Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().LoggedInUser;
                    model.DisplayPicture = Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().UserDisplayPicture;
                }
            }
            // Connects to a HubConnection and subscribes to receive a SignalR message from the background tasker relating to whether or not the user has 
            // any unread messages from others.
            this.HubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5144/MessageHub")
                .Build();

            this.HubConnection.On<bool>("ReceiveMessage", (hasNewMessages) =>
            {
                if (!hasNewMessages.Equals(IsNewMessageReceived))
                {
                    IsNewMessageReceived = hasNewMessages;
                }
            });

            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Dispatch(async () =>
                await this.HubConnection.StartAsync());
            });

            // Creates a timer on the same thread as the GUI which checks to see if the IsNewMessageReceived value has been updated by a SignalR message.
            if (MessageCheckerTimer == null)
            {
                MessageCheckerTimer = Application.Current.Dispatcher.CreateTimer();
                // The Gui waits 1 second (1000 milliseconds) until it checks for a new message.
                MessageCheckerTimer.Interval = TimeSpan.FromMilliseconds(1000);
                MessageCheckerTimer.Start();
            }

            MessageCheckerTimer.Tick += (s, e) =>
            {
                // This happens every 1 second.
                OnReceivingNewMessageSignal(s, e);
            };

            base.OnNavigated(args);
        }

        // This method is called every n seconds, based on the Timer object's interval.
        private void OnReceivingNewMessageSignal(object sender, EventArgs e)
        {
            if (Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().LoggedInUser != null)
            {
                // The (IsNewMessageReceivedAlertBeenShown == false) are there to ensure that the shell doesn't 'flicker' every 5 seconds, and only updates the image 
                // when it hasn't.
                if (IsNewMessageReceived)
                {
                    if (IsNewMessageReceivedAlertBeenShown == false)
                    {
                        // Updates the message icon if not already done before.
                        IsNewMessageReceivedAlertBeenShown = true;
                        // Animates the change from one icon to another.
                        ChangeMessageIcon("messages_large_unread_message.png");
                    }
                } else
                {
                    if (!messagesLogo.Source.Equals("messages_large.png"))
                    {
                        if (IsNewMessageReceivedAlertBeenShown == true)
                        {
                            // Changes the logo back to the original (without new message alert) if all messages are now read.
                            IsNewMessageReceivedAlertBeenShown = false;

                            // Animates the change from one icon to another.
                            ChangeMessageIcon("messages_large.png");
                        }
                    }
                }
            }
        }

        // Changes the message icon to the specified path given as a param.
        private void ChangeMessageIcon(string iconPath)
        {
            var messagesLogo = this.FindByName<ImageButton>("messagesLogo");
            var fade = new FadeAnimation();

            messagesLogo.Source = iconPath;
            // The icon appears with an animation where it fades into view.
            fade.Animate(messagesLogo);
        }
    }
}