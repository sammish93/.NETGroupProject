using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui
{
    public partial class AppShell : Shell
    {
        
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LogInPage), typeof(LogInPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));
            //Routing.RegisterRoute(nameof(Page2), typeof(Page2));

            this.BindingContext = new AppShellViewModel();

            
            //Routing.RegisterRoute(nameof(LogInPage), typeof(LogInPage));
            //Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));
            /*
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
    }
}