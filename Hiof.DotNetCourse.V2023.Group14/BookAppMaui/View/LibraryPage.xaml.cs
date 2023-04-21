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

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        dynamicColumn.WidthRequest = width;
        dynamicColumn.HeightRequest = height;
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        var model = BindingContext as ViewModel.LibraryPageViewModel;

        if (model != null)
        {
            if (!e.CurrentSelection.IsNullOrEmpty() && e.CurrentSelection.First() != null)
            {
                V1LibraryEntry entry = ((V1LibraryEntry)e.CurrentSelection.First());

                string isbn = "";
                if (!entry.LibraryEntryISBN13.IsNullOrEmpty()) 
                {
                    isbn = entry.LibraryEntryISBN13;
                } else if (!entry.LibraryEntryISBN10.IsNullOrEmpty())
                {
                    isbn = entry.LibraryEntryISBN10;
                }

                string thumbnailUrl = "";

                V1Book book = await model.GetBookWithEntryAsync(isbn);
                thumbnailUrl = book.ImageLinks["thumbnail"];

                var entryWithImage = new V1LibraryEntryWithImage(entry, thumbnailUrl);
                model.SelectedEntry = entryWithImage;
            }
        }
    }

    private void RadioButtonAll_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var collectionLibraryAll = this.FindByName<CollectionView>("collectionLibraryAll");
        var collectionLibraryRead = this.FindByName<CollectionView>("collectionLibraryRead");
        var collectionLibraryReading = this.FindByName<CollectionView>("collectionLibraryReading");
        var collectionLibraryToRead = this.FindByName<CollectionView>("collectionLibraryToRead");

        collectionLibraryAll.IsVisible = true;
        collectionLibraryRead.IsVisible = false;
        collectionLibraryReading.IsVisible = false;
        collectionLibraryToRead.IsVisible = false;
    }

    private void RadioButtonRead_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var collectionLibraryAll = this.FindByName<CollectionView>("collectionLibraryAll");
        var collectionLibraryRead = this.FindByName<CollectionView>("collectionLibraryRead");
        var collectionLibraryReading = this.FindByName<CollectionView>("collectionLibraryReading");
        var collectionLibraryToRead = this.FindByName<CollectionView>("collectionLibraryToRead");

        collectionLibraryAll.IsVisible = false;
        collectionLibraryRead.IsVisible = true;
        collectionLibraryReading.IsVisible = false;
        collectionLibraryToRead.IsVisible = false;
    }

    private void RadioButtonReading_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var collectionLibraryAll = this.FindByName<CollectionView>("collectionLibraryAll");
        var collectionLibraryRead = this.FindByName<CollectionView>("collectionLibraryRead");
        var collectionLibraryReading = this.FindByName<CollectionView>("collectionLibraryReading");
        var collectionLibraryToRead = this.FindByName<CollectionView>("collectionLibraryToRead");

        collectionLibraryAll.IsVisible = false;
        collectionLibraryRead.IsVisible = false;
        collectionLibraryReading.IsVisible = true;
        collectionLibraryToRead.IsVisible = false;
    }

    private void RadioButtonToRead_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var collectionLibraryAll = this.FindByName<CollectionView>("collectionLibraryAll");
        var collectionLibraryRead = this.FindByName<CollectionView>("collectionLibraryRead");
        var collectionLibraryReading = this.FindByName<CollectionView>("collectionLibraryReading");
        var collectionLibraryToRead = this.FindByName<CollectionView>("collectionLibraryToRead");

        collectionLibraryAll.IsVisible = false;
        collectionLibraryRead.IsVisible = false;
        collectionLibraryReading.IsVisible = false;
        collectionLibraryToRead.IsVisible = true;
    }
}