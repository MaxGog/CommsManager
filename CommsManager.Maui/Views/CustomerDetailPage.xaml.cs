using CommsManager.Maui.ViewModels;

namespace CommsManager.Maui.Views;

public partial class CustomerDetailPage : ContentPage
{
    public CustomerDetailPage(CustomersViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CustomersViewModel vm)
        {
            vm.LoadCustomersCommand.Execute(null);
        }
    }
}