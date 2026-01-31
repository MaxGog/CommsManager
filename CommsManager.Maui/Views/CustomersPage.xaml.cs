using CommsManager.Maui.ViewModels;

namespace CommsManager.Maui.Views;

public partial class CustomersPage : ContentPage
{
    public CustomersPage(CustomersViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CustomersViewModel viewModel)
        {
            await viewModel.LoadCustomersCommand.ExecuteAsync(null);
        }
    }
}
