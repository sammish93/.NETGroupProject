using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View
{
    public partial class MainPage : ContentPage
    { 
        

        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = new MainPageViewModel();
        }

        protected override async void OnAppearing()
        {

            var model = BindingContext as ViewModel.MainPageViewModel;

            if (model != null)
            {
                await model.LoadAsync();
            }

            base.OnAppearing();
        }

    }
}