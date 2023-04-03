using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

public partial class BookPage : ContentPage
{
	public BookPage()
	{
		InitializeComponent();
        BindingContext = new BookPageViewModel(App.SelectedBook);
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        BindingContext = new BookPageViewModel( App.SelectedBook);
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        dynamicColumn.WidthRequest = width;
        dynamicColumn.HeightRequest = height;
    }

    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        var model = BindingContext as ViewModel.BookPageViewModel;

        if (model != null)
        {
            model.UpdateDate(e.NewDate);
        }
    }
}