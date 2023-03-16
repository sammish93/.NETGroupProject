using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;

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
            LoginViewModel loginView = (LoginViewModel)this.BindingContext;
            loginView.Username = "JinkxMonsoon";
            loginView.Password = "Itismonsoonseason1!";
        }
		
			
        
		
    }
}