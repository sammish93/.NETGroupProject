using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LogInPage(new LoginViewModel());
        }
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            const int newWidth = 700;
            const int newHeight = 600;

           

            window.Width = newWidth;
            window.Height = newHeight;

            window.MinimumHeight = newHeight;
            window.MinimumWidth = newWidth;

            window.MinimumHeight = newHeight;
            window.MaximumWidth = newWidth;

            return window;
        }
    }
}