using CommsManager.ViewModels;

namespace CommsManager.Views;

public partial class CustomerEditPage : ContentPage
{
    public CustomerEditPage(CustomerEditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}