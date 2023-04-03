using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.IdentityModel.Tokens;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

public partial class LibraryPage : ContentPage
{
    public LibraryPage()
    {
        InitializeComponent();
        this.BindingContext = new LibraryPageViewModel();
    }

    protected override async void OnAppearing()
    {
        var model = BindingContext as LibraryPageViewModel;

        if (model != null)
        {
            await model.LoadAsync();
        }
        base.OnAppearing();
    }
    private void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        // Check if an item is selected
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            // Get the selected item
            var model = BindingContext as ViewModel.MainPageViewModel;

            if (model != null)
            {
                if (!e.CurrentSelection.IsNullOrEmpty() && e.CurrentSelection.First() != null)
                {
                    V1LibraryEntryWithImage book = ((V1LibraryEntryWithImage)e.CurrentSelection.First());

                   
                }
            }
        }

    }
}