using BookAppMaui;
using Hiof.DotNetCourse.V2023.Group14.BookAppMaui.ViewModel;
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
            builder.Services.AddSingleton<LoginViewModel>();

            builder.Services.AddSingleton<LogInPage>();
#endif

            return builder.Build();
        }
    }
}