using CommsManager.Maui.Interfaces;

namespace CommsManager.Maui.Services;

public class DialogService : IDialogService
{
    [Obsolete]
    public async Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        await Shell.Current.DisplayAlert(title, message, cancel);
    }

    [Obsolete]
    public async Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Да", string cancel = "Нет")
    {
        return await Shell.Current.DisplayAlert(title, message, accept, cancel);
    }

    [Obsolete]
    public async Task<string?> ShowActionSheetAsync(string title, string cancel, string destruction, params string[] buttons)
    {
        return await Shell.Current.DisplayActionSheet(title, cancel, destruction, buttons);
    }

    public async Task ShowToastAsync(string message, int duration = 3000)
    {
        var toast = CommunityToolkit.Maui.Alerts.Toast.Make(message, (CommunityToolkit.Maui.Core.ToastDuration)duration);
        await toast.Show();
    }
}