using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            // The login page is the first page that is shown when the app is started.
            MainPage.BindingContext = new LoginViewModel();
        }
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);
            // Creates a window with the resolution 1366x768 by default. Can be resized. UI is fully responsive down to 800x600 on desktop.
            window.Width = 1366;
            window.Height = 768;

            return window;
        }
    }
}