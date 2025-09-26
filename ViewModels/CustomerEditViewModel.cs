using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using CommsManager.Interfaces;
using CommsManager.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CommsManager.ViewModels;

public partial class CustomerEditViewModel : BaseViewModel
{
    private readonly IRepository _repository;

    [ObservableProperty]
    private Customer _customer;

    [ObservableProperty]
    private bool _isNewCustomer;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _email;

    [ObservableProperty]
    private string _phone;

    [ObservableProperty]
    private string _socialMedia;

    [ObservableProperty]
    private string _notes;

    [ObservableProperty]
    private string _contactId;

    public CustomerEditViewModel(IRepository repository)
    {
        _repository = repository;
        Title = "Редактирование заказчика";
    }

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("Customer"))
        {
            Customer = query["Customer"] as Customer;
            IsNewCustomer = Customer?.Id == 0;

            if (Customer != null)
            {
                Name = Customer.Name;
                Email = Customer.Email;
                Phone = Customer.Phone;
                SocialMedia = Customer.SocialMedia;
                Notes = Customer.Notes;
                ContactId = Customer.ContactId;
            }

            Title = IsNewCustomer ? "Новый заказчик" : "Редактирование заказчика";
        }
    }

    [RelayCommand]
    private async Task SaveCustomerAsync()
    {
        if (!ValidateCustomer())
            return;

        await SafeExecuteAsync(async () =>
        {
            Customer.Name = Name;
            Customer.Email = Email;
            Customer.Phone = Phone;
            Customer.SocialMedia = SocialMedia;
            Customer.Notes = Notes;
            Customer.ContactId = ContactId;

            if (IsNewCustomer)
            {
                await _repository.AddCustomerAsync(Customer);
                await Shell.Current.DisplayAlert("Успех", "Заказчик создан", "OK");
            }
            else
            {
                await _repository.UpdateCustomerAsync(Customer);
                await Shell.Current.DisplayAlert("Успех", "Изменения сохранены", "OK");
            }

            await Shell.Current.GoToAsync("..");
        });
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    private bool ValidateCustomer()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(Name))
            errors.Add("Имя заказчика обязательно");

        if (!string.IsNullOrWhiteSpace(Email) && !new EmailAddressAttribute().IsValid(Email))
            errors.Add("Некорректный формат email");

        if (!string.IsNullOrWhiteSpace(Phone) && !new PhoneAttribute().IsValid(Phone))
            errors.Add("Некорректный формат телефона");

        if (errors.Any())
        {
            Application.Current.MainPage.DisplayAlert("Ошибка", string.Join("\n", errors), "OK");
            return false;
        }

        return true;
    }
}