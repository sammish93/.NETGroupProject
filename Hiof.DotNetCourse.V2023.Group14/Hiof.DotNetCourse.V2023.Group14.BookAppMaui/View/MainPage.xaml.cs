using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.IdentityModel.Tokens;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View
{
    public partial class MainPage : ContentPage
    { 
        

        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = new MainPageViewModel(Application.Current.MainPage.Handler.MauiContext.Services.GetService<UserSingleton>().LoggedInUser);
        }

        protected override async void OnAppearing()
        {

            var model = BindingContext as ViewModel.MainPageViewModel;

            if (model != null)
            {
                await model.LoadProgressBarAsync();
                var progressBar = this.FindByName<ProgressBar>("progressBar");
                if (model.LoggedInUserRecentReadingGoal != null)
                {
                    // Division expression to get percentage completed of the current reading target.
                    double progress = Convert.ToDouble(model.LoggedInUserRecentReadingGoal.GoalCurrent) / Convert.ToDouble(model.LoggedInUserRecentReadingGoal.GoalTarget);

                    // Animates the progress bar.
                    await progressBar.ProgressTo(progress, 750, Easing.Linear);
                } else
                {
                    var mostRecentReadingGoalLabel = this.FindByName<Label>("mostRecentReadingGoalLabel");
                    // If no reading goal is present then this is shown instead.
                    mostRecentReadingGoalLabel.Text = " You haven't created a reading goal yet.";
                    await progressBar.ProgressTo(0, 750, Easing.Linear);
                }

                    
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

        // Handles the behaviour when a user is selected from a collectionview.
        private async void CollectionViewUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var model = BindingContext as ViewModel.MainPageViewModel;

            if (model != null)
            {
                if (!e.CurrentSelection.IsNullOrEmpty() && e.CurrentSelection.First() != null)
                {
                    V1User user = ((V1UserWithDisplayPicture)e.CurrentSelection.First()).User;
                    await model.NavigateToUserPage(user);
                }
            }
        }

        // Handles the behaviour when a book is selected from a collectionview.
        private async void CollectionViewBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var model = BindingContext as ViewModel.MainPageViewModel;

            if (model != null)
            {
                if (!e.CurrentSelection.IsNullOrEmpty() && e.CurrentSelection.First() != null)
                {
                    V1Book book = ((V1Book)e.CurrentSelection.First());
                    await model.NavigateToBookPageAsync(book);
                }
            }
        }

        // Handles the behaviour when a comment is selected from a collectionview.
        private async void CollectionView_SelectionChangedComment(object sender, SelectionChangedEventArgs e)
        {
            var model = BindingContext as ViewModel.MainPageViewModel;

            if (model != null)
            {
                if (!e.CurrentSelection.IsNullOrEmpty() && e.CurrentSelection.First() != null)
                {
                    V1Comments comment = ((V1Comments)e.CurrentSelection.First());

                    if (comment.CommentType == ClassLibrary.Enums.V1.CommentType.User)
                    {
                        // If the comment was left on a user's page.
                        V1UserWithDisplayPicture userWithDisplayPicture = await model.GetUserWithDisplayPictureAsync(comment.UserId.ToString());
                        await model.NavigateToUserPage(userWithDisplayPicture.User);
                    } else if (comment.CommentType == ClassLibrary.Enums.V1.CommentType.Book)
                    {
                        // If the comment was left on a book's page.
                        string isbn = "";
                        if (!comment.ISBN13.IsNullOrEmpty())
                        {
                            isbn = comment.ISBN13;
                        }
                        else if (!comment.ISBN10.IsNullOrEmpty())
                        {
                            isbn = comment.ISBN10;
                        }

                        V1Book book = await model.GetBookAsync(isbn);
                        await model.NavigateToBookPageAsync(book);
                    } else if (comment.CommentType == ClassLibrary.Enums.V1.CommentType.Reply)
                    {
                        // If the comment was a reply.
                        if (comment.BookObject != null)
                        {
                            // If the reply was left on a book's page.
                            await model.NavigateToBookPageAsync(comment.BookObject);
                        } else if (comment.AuthorObject != null)
                        {
                            // If the reply was left on a user's page.
                            V1UserWithDisplayPicture userWithDisplayPicture = await model.GetUserWithDisplayPictureAsync(comment.AuthorObject.User.Id.ToString());
                            await model.NavigateToUserPage(userWithDisplayPicture.User);
                        }
                    }
                }
            }
        }
    }
}