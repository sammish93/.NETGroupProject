using CommunityToolkit.Maui;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.View;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Hiof.DotNetCourse.V2023.Group14.BookAppMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            // Includes variables that are saved during runtime (User that is logged in, which book/user etc is displayed after clicked-on event).
            builder.Services.AddSingleton<V1UserSingleton>();

            return builder.Build();
        }
    }
}