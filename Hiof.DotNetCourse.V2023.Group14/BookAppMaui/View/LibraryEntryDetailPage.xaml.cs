using System.Diagnostics;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

public partial class LibraryEntryDetailPage : ContentPage
{
	public LibraryEntryDetailPage()
	{
		InitializeComponent();
		BindingContext = new LibraryEntryDetailViewModel(App.LoggedInUser, App.SelectedEntry);
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        BindingContext = new LibraryEntryDetailViewModel(App.LoggedInUser, App.SelectedEntry);
        int pageCount = Shell.Current.Navigation.NavigationStack.Count;

        Debug.WriteLine(pageCount);
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        dynamicColumn.WidthRequest = width;
        dynamicColumn.HeightRequest = height;
    }

    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {

    }
}