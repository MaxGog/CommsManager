using Microsoft.Extensions.Logging;
using CommsManager.Shared.Services;
using CommunityToolkit.Maui;
using CommsManager.Maui.Services;
using CommsManager.Maui.ViewModels;
using CommsManager.Maui.Views;
using CommsManager.Core.Interfaces;
using CommsManager.Maui.Data.Repositories;
using CommsManager.Maui.Data;

namespace CommsManager.Maui;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        }).UseMauiCommunityToolkit();
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddSingleton<MockDataService>();

        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();
        builder.Services.AddSingleton<IDatabaseMigrator, DatabaseMigrator>();
        builder.Services.AddSingleton<IDatabaseBackupService, DatabaseBackupService>();

        builder.Services.AddSingleton<Data.IUnitOfWork, UnitOfWork>();
        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<IFileService, FileService>();

        builder.Services.AddTransient<CustomersViewModel>();
        builder.Services.AddTransient<OrdersViewModel>();
        builder.Services.AddTransient<ArtistsViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<OrderDetailViewModel>();

        builder.Services.AddTransient<AppShell>();
        builder.Services.AddTransient<CustomersPage>();
        builder.Services.AddTransient<OrdersPage>();
        builder.Services.AddTransient<ArtistsPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<CustomerDetailPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}