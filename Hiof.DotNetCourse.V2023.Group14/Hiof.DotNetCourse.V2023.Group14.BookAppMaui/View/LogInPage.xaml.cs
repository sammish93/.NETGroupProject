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