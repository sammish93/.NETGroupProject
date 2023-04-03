using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui
{
    public partial class App : Application
    {
        // Static variables used to pass data to ViewModels. 
        // Use these to when you need to use a value in your onPageAppearing().
        // MAUI doesn't have a way of retrieving a query (e.g. example?query=queryexample) from a route before calling a ContentPage constructor yet. 
      //  public static V1User LoggedInUser { get; set; }
        public static V1User SelectedUser { get; set; }
        public static string SearchQuery { get; set; }
        public static V1Book SelectedBook { get; set; }
        public static V1LibraryEntryWithImage SelectedV1Library { get; set; } 
        public static byte[] UserDisplayPicture { get; set; }
        public static byte[] DefaultDisplayPicture { get; set; }
        public static byte[] SelectedUserDisplayPicture { get; set; }
        public static bool IsUserLibraryAltered { get; set; } = false;


        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);


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