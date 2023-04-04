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
    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var model = BindingContext as ViewModel.LibraryPageViewModel;

        if (model != null && e.SelectedItem != null)
        {
            model.IsItemSelected = true;
        }
    }


}