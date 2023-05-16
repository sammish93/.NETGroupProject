using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

public partial class LogInPage : ContentPage
{
	public LogInPage()
	{
		InitializeComponent();
		this.BindingContext = new LoginViewModel();
    }

    protected override async void OnAppearing()
    {
        var model = BindingContext as LoginViewModel;

        if (model != null)
        {
            // Resets the username and password entry fields.
            model.Username = null;
            model.Password = null;

            var usernameEntry = this.FindByName<Entry>("usernameEntry");
            var passwordEntry = this.FindByName<Entry>("passwordEntry");

            usernameEntry.Text = null;
            passwordEntry.Text = null;
        }
        base.OnAppearing();
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
		if(e.Value == true)
		{
            // On checkbox selected the username and password entry forms are filled with a test account.
            LoginViewModel loginView = (LoginViewModel)this.BindingContext;
            // Test account username and password:
            loginView.Username = "JinkxMonsoon";
            loginView.Password = "Itismonsoonseason1!";
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        dynamicColumn.WidthRequest = width;
        dynamicColumn.HeightRequest = height;
    }
}