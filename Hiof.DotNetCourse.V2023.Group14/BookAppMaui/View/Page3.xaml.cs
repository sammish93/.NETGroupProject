using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

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

            if (Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedBook.IndustryIdentifiers["ISBN_13"] != null)
            {
                bookId = Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedBook.IndustryIdentifiers["ISBN_13"];
            }
            else if (Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedBook.IndustryIdentifiers["ISBN_10"] != null)
            {
                bookId = Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SelectedBook.IndustryIdentifiers["ISBN_10"];
            }

            await Shell.Current.GoToAsync($"///book?bookid={bookId}");
        }
    }
}