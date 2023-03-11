using Android.App;
using Android.Runtime;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui;

namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui.Platforms.Android
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}