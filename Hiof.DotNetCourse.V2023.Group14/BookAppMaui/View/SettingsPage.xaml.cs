using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
		this.BindingContext = new SettingsPageViewModel(App.LoggedInUser, App.UserDisplayPicture);
	}

    protected override async void OnAppearing()
    {

        var model = BindingContext as ViewModel.SettingsPageViewModel;

        if (model != null)
        {
            await model.LoadAsync(App.LoggedInUser);
        }

        base.OnAppearing();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        dynamicColumn.WidthRequest = width;
        dynamicColumn.HeightRequest = height;
    }
}