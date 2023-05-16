using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.Maui.Controls;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui
{
    public partial class AppShell : Shell
    {

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

            base.OnNavigated(args);
        }
    }
}