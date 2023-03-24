using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

public partial class BookPage : ContentPage
{
	public BookPage()
	{
		InitializeComponent();
        BindingContext = new BookPageViewModel();
	}

    protected override async void OnAppearing()
    {
        BindingContext = new BookPageViewModel();

        base.OnAppearing();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        dynamicColumn.WidthRequest = width;
        dynamicColumn.HeightRequest = height;
    }
}