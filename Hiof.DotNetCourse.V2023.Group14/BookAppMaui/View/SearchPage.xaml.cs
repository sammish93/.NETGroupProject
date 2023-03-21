using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
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
        this.BindingContext = new SearchPageViewModel(App.LoggedInUser);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var model = BindingContext as ViewModel.SearchPageViewModel;

        if (model != null)
        {
            await model.LoadAsync(App.SearchQuery);
        }
    }
}