using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

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

		if(model != null )
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
            var selectedItem = e.CurrentSelection[0];
           
              
                // Show or hide the details view based on whether a book is selected
                DetailsView.IsVisible = selectedItem != null;
            }

            // Do something with the selected item
            // For example, you can navigate to a new page and pass the selected item as a parameter
            // Navigation.PushAsync(new MyPage(selectedItem));
        }
    }

