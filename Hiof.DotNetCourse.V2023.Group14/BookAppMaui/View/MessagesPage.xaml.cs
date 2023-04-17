using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;
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

        private void CollectionViewConversations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var model = BindingContext as ViewModel.MessagesViewModel;

            if (model != null)
            {
                if (!e.CurrentSelection.IsNullOrEmpty() && e.CurrentSelection.First() != null)
                {
                    V1ConversationModel conversation = ((V1ConversationModel)e.CurrentSelection.First());
                    model.SelectedConversation = conversation;

                    var messageEntry = this.FindByName<Entry>("messageEntry");
                    var messageButton = this.FindByName<Button>("messageButton");
                    messageEntry.IsVisible = true;
                    messageButton.IsVisible = true;
                }
            }
        }
    }
}