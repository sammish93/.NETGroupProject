using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

public partial class BookPage : ContentPage
{
	public BookPage()
	{
		InitializeComponent();
        BindingContext = new BookPageViewModel(App.LoggedInUser, App.SelectedBook);
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        BindingContext = new BookPageViewModel(App.LoggedInUser, App.SelectedBook);

        var addBookToLibraryButton = this.FindByName<Button>("addBookToLibraryButton");

        var model = BindingContext as ViewModel.BookPageViewModel;

        if (model != null)
        {
            if (await model.isBookInLibrary(App.SelectedBook))
            {
                addBookToLibraryButton.Text = "Add a re-read to library";
            }
            else
            {
                addBookToLibraryButton.Text = "Add to library";
            }
        }
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