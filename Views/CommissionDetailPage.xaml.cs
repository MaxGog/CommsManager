using CommsManager.ViewModels;

namespace CommsManager.Views;

public partial class CommissionDetailPage : ContentPage
{
    private readonly CommissionDetailViewModel _viewModel;

    public CommissionDetailPage(CommissionDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}
