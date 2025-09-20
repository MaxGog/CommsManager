using CommsManager.Interfaces;
using CommsManager.Services;
using CommsManager.Services.Database;
using CommsManager.ViewModels;
using CommsManager.Views;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

		RegisterServices(builder.Services);
        RegisterViewModels(builder.Services);
        RegisterPages(builder.Services);

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	private static void RegisterServices(IServiceCollection services)
    {
		services.AddSingleton<DatabaseContext>(serviceProvider =>
		{
			var dbPath = Path.Combine(FileSystem.AppDataDirectory, "commsmanager.db3");
			var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>()
				.UseSqlite($"Filename={dbPath}");
			
			return new DatabaseContext(optionsBuilder.Options);
		});

        services.AddSingleton<IRepository, Repository>();
    }

    private static void RegisterViewModels(IServiceCollection services)
    {
        services.AddTransient<CommissionsViewModel>();
        services.AddTransient<CommissionDetailViewModel>();
    }

    private static void RegisterPages(IServiceCollection services)
    {
        services.AddTransient<CommissionsPage>();
        services.AddTransient<CommissionDetailPage>();
    }
}
