using Microsoft.Extensions.Logging;
using CommsManager.Shared.Services;
using CommsManager.Services;

namespace CommsManager;

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
            });

        // Add device-specific services used by the CommsManager.Shared project
        builder.Services.AddSingleton<IFormFactor, FormFactor>();

        builder.Services.AddMauiBlazorWebView();

        builder.Services.AddSingleton<MockDataService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
