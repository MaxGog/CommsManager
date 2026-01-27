using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommsManager.Maui.Data.Models;
using CommsManager.Maui.Services;
using CommsManager.Maui.Views;

namespace CommsManager.Maui.ViewModels;

public partial class CustomersViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private List<LocalCustomer> _customers = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [Obsolete]
    public CustomersViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        LoadCustomersCommand = new AsyncRelayCommand(LoadCustomersAsync);
        AddCustomerCommand = new AsyncRelayCommand(AddCustomerAsync);
        DeleteCustomerCommand = new AsyncRelayCommand<LocalCustomer>(DeleteCustomerAsync);
        EditCustomerCommand = new AsyncRelayCommand<LocalCustomer>(EditCustomerAsync);
    }

    public IAsyncRelayCommand LoadCustomersCommand { get; }
    public IAsyncRelayCommand AddCustomerCommand { get; }
    public IAsyncRelayCommand<LocalCustomer> DeleteCustomerCommand { get; }
    public IAsyncRelayCommand<LocalCustomer> EditCustomerCommand { get; }

    private async Task LoadCustomersAsync()
    {
        IsLoading = true;

        try
        {
            var allCustomers = await _databaseService.GetCustomersAsync();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Customers = allCustomers;
            }
            else
            {
                Customers = [.. allCustomers.Where(c => c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))];
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task AddCustomerAsync()
    {
        await Shell.Current.GoToAsync(nameof(CustomerDetailPage));
    }

    private async Task EditCustomerAsync(LocalCustomer? customer)
    {
        if (customer == null) return;

        var parameters = new Dictionary<string, object>
        {
            ["Customer"] = customer
        };

        await Shell.Current.GoToAsync(nameof(CustomerDetailPage), parameters);
    }

    [Obsolete]
    private async Task DeleteCustomerAsync(LocalCustomer? customer)
    {
        if (customer == null) return;

        bool confirmed = await Shell.Current.DisplayAlert(
            "Удаление клиента",
            $"Вы уверены, что хотите удалить клиента {customer.Name}?",
            "Удалить",
            "Отмена");

        if (confirmed)
        {
            await _databaseService.DeleteCustomerAsync(customer);
            await LoadCustomersAsync();
        }
    }

    [RelayCommand]
    private void Search()
    {
        LoadCustomersCommand.ExecuteAsync(null);
    }
}