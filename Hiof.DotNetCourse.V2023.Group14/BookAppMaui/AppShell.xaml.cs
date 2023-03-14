namespace BookAppMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LogInPage), typeof(LogInPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
    }
}