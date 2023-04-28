using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.IdentityModel.Tokens;
using System.Web;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

[QueryProperty(nameof(Query), "query")]
public partial class SearchPage : ContentPage
{
    private static string _query;
    public static string Query
    {
        get => _query;
        set
        {
            _query = value;
        }
    }

    public SearchPage()
	{
		InitializeComponent();
        this.BindingContext = new SearchPageViewModel(Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().LoggedInUser);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var model = BindingContext as ViewModel.SearchPageViewModel;

        if (model != null)
        {
            await model.LoadAsync(Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().SearchQuery);
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        dynamicColumn.WidthRequest = width;
        dynamicColumn.HeightRequest = height;
    }

    private async void CollectionViewUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var model = BindingContext as ViewModel.SearchPageViewModel;

        if (model != null)
        {
            if (!e.CurrentSelection.IsNullOrEmpty() && e.CurrentSelection.First() != null)
            {
                V1User user = ((V1UserWithDisplayPicture)e.CurrentSelection.First()).User;
                await model.NavigateToUserPage(user);
            }
        }
    }

    private async void CollectionViewBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var model = BindingContext as ViewModel.SearchPageViewModel;

        if (model != null)
        {
            if (!e.CurrentSelection.IsNullOrEmpty() && e.CurrentSelection.First() != null)
            {
                V1Book book = ((V1Book)e.CurrentSelection.First());
                await model.NavigateToBookPage(book);
            }
        }
    }
}