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

            Routing.RegisterRoute(nameof(LogInPage), typeof(LogInPage));
            //Routing.RegisterRoute("home", typeof(MainPage));
            //Routing.RegisterRoute(nameof(Page2), typeof(Page2));

            this.BindingContext = new AppShellViewModel();

            /*
            Routing.RegisterRoute(nameof(LogInPage), typeof(LogInPage));
            Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));

            var userSaveKey = Preferences.Get("UserIsLoggedIn", false);

            if(userSaveKey == true)
            {
                MyAppShell.CurrentItem = MyHomePage;
            }
            else
            {
                MyAppShell.CurrentItem = MyLogin;
            }
            */
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
                }
            }

            base.OnNavigated(args);
        }
    }
}