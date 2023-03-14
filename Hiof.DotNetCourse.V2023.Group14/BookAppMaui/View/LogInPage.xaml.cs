using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;

namespace BookAppMaui;

public partial class LogInPage : ContentPage
{
	public LogInPage(LoginViewModel loginViewModel)
	{
		InitializeComponent();
		this.BindingContext= loginViewModel;

	}
}