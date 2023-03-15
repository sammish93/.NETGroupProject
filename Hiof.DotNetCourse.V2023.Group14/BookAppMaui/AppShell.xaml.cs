using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui
{
    public partial class AppShell : Shell
    {
        public V1User User;
        public string Name { get; set; } = "defaultNameThingy";
        public AppShell(V1User user)
        {
            User = user;
            Name = user.FirstName;

            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

            
            BindingContext = this;

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
            //Routing.RegisterRoute(nameof(Page2), typeof(Page2));
            */
        }
    }
}