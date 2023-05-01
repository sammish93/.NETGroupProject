namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;


// Remember to change class name here.
public partial class TemplatePage : ContentPage
{
    // And here.
	public TemplatePage()
	{
		InitializeComponent();
        //BindingContext = new ExampleViewModel();
	}

    protected override async void OnAppearing()
    {
        /*
        var model = BindingContext as ViewModel.ExampleViewModel;

        if (model != null)
        {

            // Here you can possibly initiate or load elements on your page that require API calls.
            await model.LoadAsync();
        }
        */

        base.OnAppearing();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        // 'dynamicColumn' is an x:Name that's manually set in the xaml.
        dynamicColumn.WidthRequest = width;
        dynamicColumn.HeightRequest = height;
    }
}