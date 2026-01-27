namespace CommsManager.Maui.Services;

public interface IDialogService
{
    Task ShowAlertAsync(string title, string message, string cancel = "OK");
    Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Да", string cancel = "Нет");
    Task<string?> ShowActionSheetAsync(string title, string cancel, string destruction, params string[] buttons);
    Task ShowToastAsync(string message, int duration = 3000);
}

public class DialogService : IDialogService
{
    public async Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        await Shell.Current.DisplayAlert(title, message, cancel);
    }

    public async Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Да", string cancel = "Нет")
    {
        return await Shell.Current.DisplayAlert(title, message, accept, cancel);
    }

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