using System.Collections.ObjectModel;
using System.Windows.Input;
using CommsManager.Interfaces;
using CommsManager.Models;
using CommsManager.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CommsManager.ViewModels;

public partial class CustomersViewModel : BaseViewModel
{
    private readonly IRepository _repository;

    [ObservableProperty]
    private ObservableCollection<Customer> _customers = new();

    [ObservableProperty]
    private string _searchText;

    [ObservableProperty]
    private Customer _selectedCustomer;

    public CustomersViewModel(IRepository repository)
    {
        _repository = repository;
        Title = "Заказчики";
    }

    public override async Task InitializeAsync()
    {
        await LoadCustomersAsync();
        await base.InitializeAsync();
    }

    [RelayCommand]
    private async Task LoadCustomersAsync()
    {
        await LoadDataAsync(async () =>
        {
            var customers = await _repository.GetCustomersAsync();
            Customers = new ObservableCollection<Customer>(customers);
        });
    }

    [RelayCommand]
    private async Task AddCustomerAsync()
    {
        await Shell.Current.GoToAsync(nameof(CustomerEditPage), new Dictionary<string, object>
        {
            ["Customer"] = new Customer()
        });
    }

    [RelayCommand]
    private async Task EditCustomerAsync(Customer customer)
    {
        if (customer == null) return;

        await Shell.Current.GoToAsync(nameof(CustomerEditPage), new Dictionary<string, object>
        {
            ["Customer"] = customer
        });
    }

    [RelayCommand]
    private async Task DeleteCustomerAsync(Customer customer)
    {
        if (customer == null) return;

        bool result = await Application.Current.MainPage.DisplayAlert(
            "Удаление заказчика",
            $"Вы уверены, что хотите удалить заказчика {customer.Name}?",
            "Удалить", "Отмена");

        if (result)
        {
            await SafeExecuteAsync(async () =>
            {
                await _repository.DeleteCustomerAsync(customer.Id);
                Customers.Remove(customer);
                
                await Shell.Current.DisplayAlert("Успех", "Заказчик удален", "OK");
            });
        }
    }

    [RelayCommand]
    private async Task SearchCustomersAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            await LoadCustomersAsync();
            return;
        }

        await LoadDataAsync(async () =>
        {
            var allCustomers = await _repository.GetCustomersAsync();
            var filteredCustomers = allCustomers
                .Where(c => c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                           (c.Email?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                           (c.Phone?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false))
                .ToList();

            Customers = new ObservableCollection<Customer>(filteredCustomers);
        });
    }

    [RelayCommand]
    private async Task ViewCustomerCommissionsAsync(Customer customer)
    {
        if (customer == null) return;

        await Shell.Current.GoToAsync("//CommissionsPage", new Dictionary<string, object>
        {
            ["CustomerFilter"] = customer.Id
        });
    }

    partial void OnSearchTextChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            LoadCustomersCommand.Execute(null);
        }
    }
}