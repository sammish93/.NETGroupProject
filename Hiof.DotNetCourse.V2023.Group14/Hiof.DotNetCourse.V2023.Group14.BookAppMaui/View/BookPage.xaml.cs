using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

public partial class BookPage : ContentPage
{
	public BookPage()
	{
		InitializeComponent();
        BindingContext = new BookPageViewModel(Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().LoggedInUser, Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().SelectedBook);
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        BindingContext = new BookPageViewModel(Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().LoggedInUser, Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().SelectedBook);

        var addBookToLibraryButton = this.FindByName<Button>("addBookToLibraryButton");

        var model = BindingContext as ViewModel.BookPageViewModel;

        if (model != null)
        {
            // Button text changes based on whether the book exists in the library or not.
            if (await model.IsBookInLibraryAsync(Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().SelectedBook))
            {
                addBookToLibraryButton.Text = "Add a re-read to library";
            }
            else
            {
                addBookToLibraryButton.Text = "Add to library";
            }

            await model.LoadAsync();
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
            // Date picker behaves differently than entry forms, and thus requires this method for the date to be saved to a variable (and thus retrieved).
            model.UpdateDate(e.NewDate);
        }
    }
}