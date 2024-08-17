using Microsoft.Extensions.Logging;
using WarehouseFun.App.Base;
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

            builder.Services.AddSingleton<GameHubClient>((provider) => new GameHubClient("http://192.168.1.45:5259/game"));
            builder.Services.AddSingleton<IHardwareHandling>((provider) => HardwareHandling.Instance);

            builder.Services.AddTransient<IAlerts>((provider) => (IAlerts)Application.Current!);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
