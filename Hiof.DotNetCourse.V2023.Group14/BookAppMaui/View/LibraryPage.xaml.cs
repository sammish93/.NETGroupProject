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
	/*
	protected override async void OnAppearing()
	{
		var model = BindingContext as LibraryPageViewModel;

		if(model != null )
		{
			await model.PopulateReadBooks();
		}
		base.OnAppearing();
	}
	*/
}