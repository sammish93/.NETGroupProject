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
        }
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);
            window.Width = 1366;
            window.Height = 768;


            // This code makes the GUI fixed to specific dimensions
            /*
            const int newWidth = 1024;
            const int newHeight = 768

            window.Width = newWidth;
            window.Height = newHeight;

            window.MinimumWidth = newWidth;
            window.MinimumHeight = newHeight;
            */

            return window;
        }
    }
}