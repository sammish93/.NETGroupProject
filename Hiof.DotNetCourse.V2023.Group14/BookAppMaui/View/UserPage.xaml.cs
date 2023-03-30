using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

public partial class UserPage : ContentPage
{
	public UserPage()
	{
		InitializeComponent();
        BindingContext = new UserPageViewModel(App.LoggedInUser, App.SelectedUser, App.SelectedUserDisplayPicture);
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        BindingContext = new UserPageViewModel(App.LoggedInUser, App.SelectedUser, App.SelectedUserDisplayPicture);

        var model = BindingContext as ViewModel.UserPageViewModel;

        if (model != null)
        {
            await model.LoadAsync();
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        dynamicColumn.WidthRequest = width;
        dynamicColumn.HeightRequest = height;
    }
}