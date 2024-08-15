using Android.Media.TV;
using Microsoft.Extensions.Logging;
using WarehouseFun.App.Base;
using WarehouseFun.App.Services;
using WarehouseFun.Shared;

namespace WarehouseFun.App
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
                    fonts.AddFont("FontAwesomeSolid.otf", "FontAwesomeSolid");
                    fonts.AddFont("BebasNeue-Regular.ttf", "BebasNeue");
                });

            builder.Services.AddSingleton<GameHubClient>((provider) => new GameHubClient("https://localhost:7270/game"));
            builder.Services.AddSingleton<IHardwareHandling>(sp => new HardwareHandling());

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
