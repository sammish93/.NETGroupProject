using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

public partial class Page3 : ContentPage
{
	public Page3()
	{
		InitializeComponent();
        this.BindingContext = new Page3ViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var model = BindingContext as ViewModel.Page3ViewModel;

        if (model != null)
        { 
            string bookId = "";

            if (App.SelectedBook.IndustryIdentifiers["ISBN_13"] != null)
            {
                bookId = App.SelectedBook.IndustryIdentifiers["ISBN_13"];
            }
            else if (App.SelectedBook.IndustryIdentifiers["ISBN_10"] != null)
            {
                bookId = App.SelectedBook.IndustryIdentifiers["ISBN_10"];
            }

            await Shell.Current.GoToAsync($"///book?bookid={bookId}");
        }
    }
}