using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommsManager.Maui.Services;

namespace CommsManager.Maui.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private int _customerCount;

    [ObservableProperty]
    private int _orderCount;

    [ObservableProperty]
    private int _artistCount;

    [ObservableProperty]
    private decimal _totalRevenue;

    [ObservableProperty]
    private bool _isLoading;

    public ProfileViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        LoadStatsCommand = new AsyncRelayCommand(LoadStatisticsAsync);
        ExportDataCommand = new AsyncRelayCommand(ExportDataAsync);
        ImportDataCommand = new AsyncRelayCommand(ImportDataAsync);
        SyncDataCommand = new AsyncRelayCommand(SyncDataAsync);
        ClearCacheCommand = new AsyncRelayCommand(ClearCacheAsync);
    }

    public IAsyncRelayCommand LoadStatsCommand { get; }
    public IAsyncRelayCommand ExportDataCommand { get; }
    public IAsyncRelayCommand ImportDataCommand { get; }
    public IAsyncRelayCommand SyncDataCommand { get; }
    public IAsyncRelayCommand ClearCacheCommand { get; }

    private async Task LoadStatisticsAsync()
    {
        IsLoading = true;

        try
        {
            var customers = await _databaseService.GetCustomersAsync();
            var orders = await _databaseService.GetOrdersAsync();
            var artists = await _databaseService.GetArtistProfilesAsync();

            CustomerCount = customers.Count;
            OrderCount = orders.Count;
            ArtistCount = artists.Count;
            TotalRevenue = orders.Where(o => o.Status == "Completed").Sum(o => o.PriceAmount);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task ExportDataAsync()
    {
        // TODO: Реализовать экспорт данных
        await Shell.Current.DisplayAlert("Экспорт", "Функция экспорта в разработке", "OK");
    }

    private async Task ImportDataAsync()
    {
        // TODO: Реализовать импорт данных
        await Shell.Current.DisplayAlert("Импорт", "Функция импорта в разработке", "OK");
    }

    private async Task SyncDataAsync()
    {
        // TODO: Реализовать синхронизацию с облаком
        await Shell.Current.DisplayAlert("Синхронизация", "Функция синхронизации в разработке", "OK");
    }

    private async Task ClearCacheAsync()
    {
        bool confirmed = await Shell.Current.DisplayAlert(
            "Очистка кеша",
            "Вы уверены, что хотите очистить кеш приложения?",
            "Очистить",
            "Отмена");

        if (confirmed)
        {
            // TODO: Реализовать очистку кеша
            await Shell.Current.DisplayAlert("Готово", "Кеш очищен", "OK");
        }
    }
}