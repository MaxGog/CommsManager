using CommsManager.ViewModels;

namespace CommsManager.Views;

public partial class CommissionsPage : ContentPage
{
    private readonly CommissionsViewModel _viewModel;

    public CommissionsPage(CommissionsViewModel viewModel)
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
