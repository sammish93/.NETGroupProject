using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;

public partial class UserPage : ContentPage
{
    public UserPage()
    {
        InitializeComponent();
        BindingContext = new UserPageViewModel(Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().LoggedInUser, Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().SelectedUser, Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().SelectedUserDisplayPicture);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        BindingContext = new UserPageViewModel(Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().LoggedInUser, Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().SelectedUser, Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().SelectedUserDisplayPicture);

        var model = BindingContext as ViewModel.UserPageViewModel;

        if (model != null)
        {
            var messageButton = this.FindByName<Button>("messageButton");
            var readingGoalButton = this.FindByName<Button>("readingGoalButton");
            var pickerStartDate = this.FindByName<DatePicker>("pickerStartDate");
            var pickerEndDate = this.FindByName<DatePicker>("pickerEndDate");
            var entryGoalTarget = this.FindByName<Entry>("entryGoalTarget");
            var labelStartDate = this.FindByName<Label>("labelStartDate");
            var labelEndDate = this.FindByName<Label>("labelEndDate");
            var labelGoalTarget = this.FindByName<Label>("labelGoalTarget");

            // If the user is visiting their own page then elements pertaining to the creation of a new reading goal is shown.
            if (Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().LoggedInUser.Id == Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().SelectedUser.Id)
            {
                messageButton.IsVisible = false;
                readingGoalButton.IsVisible = true;
                pickerStartDate.IsVisible = true;
                pickerEndDate.IsVisible = true;
                entryGoalTarget.IsVisible = true;
                labelStartDate.IsVisible = true;
                labelEndDate.IsVisible = true;
                labelGoalTarget.IsVisible = true;
            } else
            // If the user is visiting another user's page then these elements are hidden and a button to send a message to another user is shown instead.
            {
                readingGoalButton.IsVisible = false;
                pickerStartDate.IsVisible = false;
                pickerEndDate.IsVisible = false;
                entryGoalTarget.IsVisible = false;
                labelStartDate.IsVisible = false;
                labelEndDate.IsVisible = false;
                labelGoalTarget.IsVisible = false;
                messageButton.IsVisible = true;
            }

            await model.LoadProgressBarAsync();
            var progressBar = this.FindByName<ProgressBar>("progressBar");

            if (model.SelectedUserRecentReadingGoal != null) 
            {
                // Division expression to calculate the percentage of reading goal completion.
                double progress = Convert.ToDouble(model.SelectedUserRecentReadingGoal.GoalCurrent) / Convert.ToDouble(model.SelectedUserRecentReadingGoal.GoalTarget);
                // Animates the progress bar.
                await progressBar.ProgressTo(progress, 750, Easing.Linear);
            } else
            {
                var mostRecentReadingGoalLabel = this.FindByName<Label>("mostRecentReadingGoalLabel");
                // If no reading goal is present then this is shown instead.
                mostRecentReadingGoalLabel.Text = Application.Current.MainPage.Handler.MauiContext.Services.GetService<V1UserSingleton>().SelectedUser.UserName + " hasn't created a reading goal yet.";
                await progressBar.ProgressTo(0, 750, Easing.Linear);
            }
            
            await model.LoadAsync();
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        dynamicColumn.WidthRequest = width;
        dynamicColumn.HeightRequest = height;
    }

    // Handles the behaviour when a book is selected.
    private async void CollectionViewBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var model = BindingContext as ViewModel.UserPageViewModel;

        if (model != null)
        {
            if (!e.CurrentSelection.IsNullOrEmpty() && e.CurrentSelection.First() != null)
            {
                V1Book book = ((V1Book)e.CurrentSelection.First());
                await model.NavigateToBookPageAsync(book);
            }
        }
    }

    private void DatePicker_StartDateSelected(object sender, DateChangedEventArgs e)
    {
        var model = BindingContext as ViewModel.UserPageViewModel;

        if (model != null)
        {
            // Date picker behaves differently than entry forms, and thus requires this method for the date to be saved to a variable (and thus retrieved).
            model.UpdateStartDate(e.NewDate);
        }
    }

    private void DatePicker_EndDateSelected(object sender, DateChangedEventArgs e)
    {
        var model = BindingContext as ViewModel.UserPageViewModel;

        if (model != null)
        {
            // Date picker behaves differently than entry forms, and thus requires this method for the date to be saved to a variable (and thus retrieved).
            model.UpdateEndDate(e.NewDate);
        }
    }
}