using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommsManager.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CommsManager.ViewModels;

public class BaseViewModel :  ObservableObject, INotifyPropertyChanged, Interfaces.IQueryAttributable, IAsyncInitialize
{
    private bool _isBusy;
    private string _title = string.Empty;
    private bool _isInitialized;

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public bool IsInitialized
    {
        get => _isInitialized;
        set => SetProperty(ref _isInitialized, value);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName] string propertyName = "",
        Action onChanged = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        onChanged?.Invoke();
        OnPropertyChanged(propertyName);
        return true;
    }

    public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // Базовая реализация, может быть переопределена в производных классах
    }

    public virtual Task InitializeAsync()
    {
        IsInitialized = true;
        return Task.CompletedTask;
    }

    protected async Task LoadDataAsync(Func<Task> loadAction)
    {
        if (IsBusy)
            return;

        IsBusy = true;
        try
        {
            await loadAction();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected async Task SafeExecuteAsync(Func<Task> action, Action onError = null)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            onError?.Invoke();
        }
    }
}
