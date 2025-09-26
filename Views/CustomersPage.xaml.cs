using CommsManager.ViewModels;

namespace CommsManager.Views;

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
        
        if (BindingContext is CustomersViewModel viewModel && !viewModel.IsInitialized)
        {
            await viewModel.InitializeAsync();
        }
    }
}