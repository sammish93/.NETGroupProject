using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

public partial class SignupPage : ContentPage
{
	public SignupPage()
	{
		InitializeComponent();
		BindingContext = new SignupPageViewModel();
	}
}