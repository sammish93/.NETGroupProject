using System;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using System.Windows.Input;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class AppShellViewModel : BaseViewModel
    {
        private V1User _user;
        private string _titleCurrentPage = "defaultTitle";


        public V1User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public string TitleCurrentPage
        {
            get => _titleCurrentPage;
            set => SetProperty(ref _titleCurrentPage, value);
        }


        public AppShellViewModel()
        {

        }

        public AppShellViewModel(V1User user)
        {
            User = user;
        }

        public ICommand HomeButtonCommand => new Command(async () => await NavButtonAsync("///home"));
        public ICommand ProfileButtonCommand => new Command(async () => await NavButtonAsync("///profile"));
        public ICommand MessagesButtonCommand => new Command(async () => await NavButtonAsync("///messages"));

        private async Task NavButtonAsync(string root)
        {
            await Shell.Current.GoToAsync(root);
        }
    }
}

