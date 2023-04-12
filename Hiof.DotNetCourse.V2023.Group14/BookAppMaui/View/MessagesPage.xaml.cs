using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.IdentityModel.Tokens;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View
{
    public partial class MessagesPage : ContentPage
    { 
        

        public MessagesPage()
        {
            InitializeComponent();

            this.BindingContext = new MessagesViewModel(App.LoggedInUser);
        }

        protected override async void OnAppearing()
        {

            var model = BindingContext as ViewModel.MessagesViewModel;

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
    }
}