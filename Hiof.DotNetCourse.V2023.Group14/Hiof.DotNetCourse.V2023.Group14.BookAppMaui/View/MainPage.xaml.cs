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

                double progress = Convert.ToDouble(model.LoggedInUserRecentReadingGoal.GoalCurrent) / Convert.ToDouble(model.LoggedInUserRecentReadingGoal.GoalTarget);

                await progressBar.ProgressTo(progress, 750, Easing.Linear);
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

        private async void CollectionViewBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var model = BindingContext as ViewModel.MainPageViewModel;

            if (model != null)
            {
                if (!e.CurrentSelection.IsNullOrEmpty() && e.CurrentSelection.First() != null)
                {
                    V1Book book = ((V1Book)e.CurrentSelection.First());
                    await model.NavigateToBookPage(book);
                }
            }
        }

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
                        V1UserWithDisplayPicture userWithDisplayPicture = await model.GetUserWithDisplayPictureAsync(comment.UserId.ToString());
                        await model.NavigateToUserPage(userWithDisplayPicture.User);
                    } else if (comment.CommentType == ClassLibrary.Enums.V1.CommentType.Book)
                    {
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
                        await model.NavigateToBookPage(book);
                    } else if (comment.CommentType == ClassLibrary.Enums.V1.CommentType.Reply)
                    {
                        
                        if (comment.BookObject != null)
                        {
                            await model.NavigateToBookPage(comment.BookObject);
                        } else if (comment.AuthorObject != null)
                        {
                            V1UserWithDisplayPicture userWithDisplayPicture = await model.GetUserWithDisplayPictureAsync(comment.AuthorObject.User.Id.ToString());
                            await model.NavigateToUserPage(userWithDisplayPicture.User);
                        }
                    }
                    
                }
            }
        }

    }
}