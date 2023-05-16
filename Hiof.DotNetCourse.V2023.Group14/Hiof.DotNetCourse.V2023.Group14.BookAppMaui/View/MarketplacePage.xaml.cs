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
    private string _searchQuery;

    public IDispatcherTimer UserTypingTimer
    {
        get => _userTypingTimer;
        set
        {
            _userTypingTimer = value;

        }
    }

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;

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

    private void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        var model = BindingContext as ViewModel.MarketplacePageViewModel;

        if (model != null)
        {
            if (!e.CurrentSelection.IsNullOrEmpty() && e.CurrentSelection.First() != null)
            {
                V1Book book = ((V1Book)e.CurrentSelection.First());

                model.SelectedBook = book;

                var absoluteBanner = this.FindByName<AbsoluteLayout>("absoluteBanner");
                var selectPromptLabel = this.FindByName<Label>("selectPromptLabel");
                var bookDisplayGrid = this.FindByName<Grid>("bookDisplayGrid");

                selectPromptLabel.IsVisible = false;
                absoluteBanner.IsVisible = true;
                bookDisplayGrid.IsVisible = true;

                model.IsSellGridVisible = false;
                model.IsBuyGridVisible = false;
                model.IsBuyAndSellButtonsVisible = true;
            }
        }
    }

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

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (UserTypingTimer == null)
        {
            UserTypingTimer = Application.Current.Dispatcher.CreateTimer();
            UserTypingTimer.Interval = TimeSpan.FromMilliseconds(1000);
            UserTypingTimer.Start();
        }

        UserTypingTimer.Tick += (s, e) =>
        {
            OnTypingTimerElapsed(s, e);
        };
    }

    private void OnTypingTimerElapsed(object sender, EventArgs e)
    {
        var searchBar = this.FindByName<SearchBar>("searchBar");

        PerformSearch(searchBar.Text);
    }

    private async void PerformSearch(string searchQuery)
    {
        var model = BindingContext as ViewModel.MarketplacePageViewModel;

        if (model != null)
        {
            if (searchQuery != null && !searchQuery.Equals(SearchQuery))
            {
                await model.GetBookSearchAsync(searchQuery);
                SearchQuery = searchQuery;
            }
        }
    }


}