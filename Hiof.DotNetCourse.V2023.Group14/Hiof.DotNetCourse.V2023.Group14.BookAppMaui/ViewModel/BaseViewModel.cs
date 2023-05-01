using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel
{
    public partial class BaseViewModel : ObservableObject

    {
        public BaseViewModel()
        {

        }

       
        [ObservableProperty]
        string title;
        

        
    }
}
