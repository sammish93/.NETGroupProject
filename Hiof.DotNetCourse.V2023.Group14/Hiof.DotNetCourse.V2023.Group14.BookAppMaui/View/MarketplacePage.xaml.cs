using CommunityToolkit.Maui.Behaviors;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Maui.Controls;
using System.Timers;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

public partial class MarketplacePage : ContentPage
{

    private IDispatcherTimer _userTypingTimer;

    public IDispatcherTimer UserTypingTimer
    {
        get => _userTypingTimer;
        set
        {
            _userTypingTimer = value;

        }
    }

    public MarketplacePage()
    {
        InitializeComponent();
        this.BindingContext = new MarketplacePageViewModel();
        
    }


    protected override async void OnAppearing()
    {
        var model = BindingContext as MarketplacePageViewModel;

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

    // When a book is selected from a search result collectionview then the book is saved to the 'SelectedBook' variable, and a nested page with the 'buy' and 'sell'
    // buttons appear, in which a user can then navigate to said nested page.
    private void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        var model = BindingContext as ViewModel.MarketplacePageViewModel;

        if (model != null)
        {
            if (!e.CurrentSelection.IsNullOrEmpty() && e.CurrentSelection.First() != null)
            {
                // Saves the selected book to a variable.
                V1Book book = ((V1Book)e.CurrentSelection.First());
                model.SelectedBook = book;

                var absoluteBanner = this.FindByName<AbsoluteLayout>("absoluteBanner");
                var selectPromptLabel = this.FindByName<Label>("selectPromptLabel");
                var bookDisplayGrid = this.FindByName<Grid>("bookDisplayGrid");

                // Shows the nested page in which a user can either buy or sell a book.
                selectPromptLabel.IsVisible = false;
                absoluteBanner.IsVisible = true;
                bookDisplayGrid.IsVisible = true;

                model.IsSellGridVisible = false;
                model.IsBuyGridVisible = false;
                model.IsBuyAndSellButtonsVisible = true;
            }
        }
    }

    // Saves the user selected in the 'sell' nested page to a variable called 'SelectedUser'.
    private void OnSellerSelected(object sender, SelectionChangedEventArgs e)
    {
        var model = BindingContext as ViewModel.MarketplacePageViewModel;

        if (model != null)
        {
            if (!e.CurrentSelection.IsNullOrEmpty() && e.CurrentSelection.First() != null)
            {
                V1MarketplaceBookResponse post = ((V1MarketplaceBookResponse)e.CurrentSelection.First());

                model.SelectedUser = post.OwnerObject;
            }
        }
    }

    // Records how long the Gui should wait until calling a method to search for results based on a search query.
    // Note: Maui's current implementation of 'isUserTyping'-esque functionality is very poor in the current version. This is a clunky solution that could possibly 
    // be improved in future builds.
    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (UserTypingTimer == null)
        {
            UserTypingTimer = Application.Current.Dispatcher.CreateTimer();
            // The Gui waits 1 second (1000 milliseconds) until it fetches the search query entered, and searches for books based on said query.
            UserTypingTimer.Interval = TimeSpan.FromMilliseconds(1000);
            UserTypingTimer.Start();
        }

        UserTypingTimer.Tick += (s, e) =>
        {
            // This happens every 1 second.
            OnTypingTimerElapsed(s, e);
        };
    }

    // This method is called every n seconds, based on the Timer object's interval.
    private void OnTypingTimerElapsed(object sender, EventArgs e)
    {
        var searchBar = this.FindByName<SearchBar>("searchBar");

        // Retrieves string search query entered by the user in the search bar and forwards it to a method as a parameter.
        PerformSearch(searchBar.Text);
    }

    // Method called every n seconds based on a Timer object's interval.
    private async void PerformSearch(string searchQuery)
    {
        var model = BindingContext as ViewModel.MarketplacePageViewModel;

        if (model != null)
        {
            // Important if test which is -only- called if the search query is different than the search query in the previous tick/interval.
            // If this if test didn't exist then the Gui would constantly refresh search results every single n seconds.
            if (searchQuery != null && !searchQuery.Equals(model.SearchQuery))
            {
                await model.GetBookSearchAsync(searchQuery);
                model.SearchQuery = searchQuery;
            }
        }
    }

    // Filters search results between all search results, and search results that have at least one book currently for sale.
    private void checkBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var model = BindingContext as ViewModel.MarketplacePageViewModel;

        if (model != null)
        {
            model.IsCheckboxChecked = ((bool)e.Value);
            if (model.IsCheckboxChecked)
            {
                // Only search results with at least one book currently for sale are shown.
                model.IsSearchResultsVisible = false;
                model.IsSearchResultsForSaleVisible = true;
            }
            else
            {
                // All search results are shown.
                model.IsSearchResultsVisible = true;
                model.IsSearchResultsForSaleVisible = false;
            }
        }
    }
}