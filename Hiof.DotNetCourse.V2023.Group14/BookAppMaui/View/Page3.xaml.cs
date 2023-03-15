using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

public partial class Page3 : ContentPage
{
	public Page3()
	{
		InitializeComponent();
        this.BindingContext = new Page3ViewModel();
    }
}