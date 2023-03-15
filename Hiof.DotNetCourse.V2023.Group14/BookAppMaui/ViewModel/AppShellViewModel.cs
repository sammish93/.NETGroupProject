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

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public class AppShellViewModel : BaseViewModel
    {
        public V1User User;
        public string Name { get; set; } = "defaultNameThingy";

        public AppShellViewModel()
        {

        }

        public AppShellViewModel(V1User user)
        {
            User = user;
            Name = user.FirstName;
        }

    }
}
