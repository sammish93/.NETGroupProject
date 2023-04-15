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
        private static V1User _loggedInUser;
        private static V1User _selectedUser;
        private static string _searchQuery;
        private static V1Book _selectedBook;
        private static byte[] _userDisplayPicture;
        private static byte[] _defaultDisplayPicture;
        private static byte[] _selectedUserDisplayPicture;
        private static bool _isUserLibraryAltered = false;

        public static V1User LoggedInUser
        {
            get => _loggedInUser;
            set
            {
                _loggedInUser = value;
            }
        }

        public static V1User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
            }
        }

        public static string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
            }
        }

        public static V1Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
            }
        }

        public static byte[] UserDisplayPicture
        {
            get => _userDisplayPicture;
            set
            {
                _userDisplayPicture = value;
            }
        }

        public static byte[] DefaultDisplayPicture
        {
            get => _defaultDisplayPicture;
            set
            {
                _defaultDisplayPicture = value;
            }
        }

        public static byte[] SelectedUserDisplayPicture
        {
            get => _selectedUserDisplayPicture;
            set
            {
                _userDisplayPicture = value;
            }
        }

        public static bool IsUserLibraryAltered
        {
            get => _isUserLibraryAltered;
            set
            {
                _isUserLibraryAltered = value;
            }
        }


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