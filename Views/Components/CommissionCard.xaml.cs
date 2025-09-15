using CommsManager.Models;
using CommsManager.ViewModels;

namespace CommsManager.Views.Components;

public partial class CommissionCard : ContentView
{
    public static readonly BindableProperty CommissionProperty =
        BindableProperty.Create(nameof(Commission), typeof(Commission), typeof(CommissionCard), null);

    public Commission Commission
    {
        get => (Commission)GetValue(CommissionProperty);
        set => SetValue(CommissionProperty, value);
    }

    public CommissionCard()
    {
        InitializeComponent();
    }
}
