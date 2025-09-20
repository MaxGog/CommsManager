using System.Collections.ObjectModel;
using System.Windows.Input;
using CommsManager.Models;
using CommsManager.Models.Enums;
using CommsManager.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CommsManager.ViewModels;
public partial class CommissionDetailViewModel : BaseViewModel, Interfaces.IQueryAttributable
{
    private readonly IRepository _repository;

    [ObservableProperty]
    private Commission _commission;

    [ObservableProperty]
    private Customer _selectedCustomer;

    [ObservableProperty]
    private string _newTag = string.Empty;

    [ObservableProperty]
    private ImageSource _sketchImage;

    [ObservableProperty]
    private ImageSource _finalArtImage;

    public ObservableCollection<Customer> Customers { get; } = new();
    public ObservableCollection<CommissionStatus> StatusOptions { get; } = new();
    public ObservableCollection<ArtType> ArtTypeOptions { get; } = new();

    public bool IsExistingCommission => Commission?.Id > 0;
    public bool HasSketch => !string.IsNullOrEmpty(Commission?.SketchPath);
    public bool HasFinalArt => !string.IsNullOrEmpty(Commission?.FinalArtPath);

    public int? DaysUntilDeadline => Commission?.Deadline.HasValue == true 
        ? (int?)(Commission.Deadline.Value - DateTime.Today).TotalDays 
        : null;

    public Color DeadlineColor
    {
        get
        {
            if (Commission?.Deadline.HasValue != true) return Colors.Gray;
            
            var daysLeft = (Commission.Deadline.Value - DateTime.Today).TotalDays;
            return daysLeft switch
            {
                < 0 => Color.FromArgb("#FF5252"),
                < 1 => Color.FromArgb("#FFB74D"),
                < 3 => Color.FromArgb("#FFD54F"),
                _ => Color.FromArgb("#66BB6A")
            };
        }
    }

    public ICommand SaveCommissionCommand { get; }
    public ICommand DeleteCommissionCommand { get; }
    public ICommand UploadSketchCommand { get; }
    public ICommand UploadFinalArtCommand { get; }
    public ICommand ViewSketchCommand { get; }
    public ICommand ViewFinalArtCommand { get; }
    public ICommand AddTagCommand { get; }
    public ICommand RemoveTagCommand { get; }
    public ICommand ClearDeadlineCommand { get; }

    public CommissionDetailViewModel(IRepository repository)
    {
        _repository = repository;

        SaveCommissionCommand = new AsyncRelayCommand(SaveCommission);
        DeleteCommissionCommand = new AsyncRelayCommand(DeleteCommission);
        UploadSketchCommand = new AsyncRelayCommand(UploadSketch);
        UploadFinalArtCommand = new AsyncRelayCommand(UploadFinalArt);
        ViewSketchCommand = new AsyncRelayCommand(ViewSketch);
        ViewFinalArtCommand = new AsyncRelayCommand(ViewFinalArt);
        AddTagCommand = new AsyncRelayCommand(AddTag);
        RemoveTagCommand = new AsyncRelayCommand<CommissionTag>(RemoveTag);
        ClearDeadlineCommand = new RelayCommand(ClearDeadline);

        InitializeEnums();
    }

    private void InitializeEnums()
    {
        foreach (CommissionStatus status in Enum.GetValues(typeof(CommissionStatus)))
            StatusOptions.Add(status);

        foreach (ArtType artType in Enum.GetValues(typeof(ArtType)))
            ArtTypeOptions.Add(artType);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Commission", out var commissionObj) && commissionObj is Commission commission)
        {
            Commission = commission;
            
            Commission.Tags ??= new List<CommissionTag>();
            Commission.Artworks ??= new List<Artwork>();
            
            Title = Commission.Id > 0 ? "Редактирование комиссии" : "Новая комиссия";
            
            OnPropertyChanged(nameof(IsExistingCommission));
            OnPropertyChanged(nameof(HasSketch));
            OnPropertyChanged(nameof(HasFinalArt));
            OnPropertyChanged(nameof(DaysUntilDeadline));
            OnPropertyChanged(nameof(DeadlineColor));
        }
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await LoadCustomers();
        LoadImages();
    }

    private async Task LoadCustomers()
    {
        try
        {
            var customers = await _repository.GetCustomersAsync();
            Customers.Clear();
            foreach (var customer in customers)
            {
                Customers.Add(customer);
            }

            if (Commission?.CustomerId > 0)
            {
                SelectedCustomer = Customers.FirstOrDefault(c => c.Id == Commission.CustomerId);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить заказчиков: {ex.Message}", "OK");
        }
    }

    private void LoadImages()
    {
        if (Commission != null)
        {
            if (!string.IsNullOrEmpty(Commission.SketchPath))
            {
                SketchImage = ImageSource.FromFile(Commission.SketchPath);
            }

            if (!string.IsNullOrEmpty(Commission.FinalArtPath))
            {
                FinalArtImage = ImageSource.FromFile(Commission.FinalArtPath);
            }
        }
    }

    private async Task SaveCommission()
    {
        if (IsBusy) return;

        if (string.IsNullOrWhiteSpace(Commission.Title))
        {
            await Shell.Current.DisplayAlert("Ошибка", "Название комиссии обязательно", "OK");
            return;
        }

        if (SelectedCustomer == null)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Выберите заказчика", "OK");
            return;
        }

        try
        {
            IsBusy = true;

            Commission.CustomerId = SelectedCustomer.Id;
            Commission.Customer = SelectedCustomer;
            
            Commission.Title ??= "Новая комиссия";
            Commission.Description ??= string.Empty;
            Commission.Notes ??= string.Empty;

            if (Commission.Id == 0)
            {
                var savedCommission = await _repository.AddCommissionAsync(Commission);
                Commission = savedCommission;
                
                await Shell.Current.DisplayAlert("Успех", "Комиссия создана", "OK");
            }
            else
            {
                await _repository.UpdateCommissionAsync(Commission);
                await Shell.Current.DisplayAlert("Успех", "Комиссия обновлена", "OK");
            }

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось сохранить комиссию: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task DeleteCommission()
    {
        if (Commission?.Id == 0) return;

        var confirm = await Shell.Current.DisplayAlert(
            "Подтверждение",
            "Вы уверены, что хотите удалить эту комиссию?",
            "Удалить",
            "Отмена");

        if (confirm)
        {
            try
            {
                await _repository.DeleteCommissionAsync(Commission.Id);
                await Shell.Current.DisplayAlert("Успех", "Комиссия удалена", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"Не удалось удалить комиссию: {ex.Message}", "OK");
            }
        }
    }

    private async Task UploadSketch()
    {
        await Shell.Current.DisplayAlert("Инфо", "Функция загрузки будет реализована", "OK");
    }

    private async Task UploadFinalArt()
    {
        await Shell.Current.DisplayAlert("Инфо", "Функция загрузки будет реализована", "OK");
    }

    private async Task ViewSketch()
    {
        if (HasSketch)
        {
            await Shell.Current.DisplayAlert("Просмотр", "Просмотр эскиза", "OK");
        }
    }

    private async Task ViewFinalArt()
    {
        if (HasFinalArt)
        {
            await Shell.Current.DisplayAlert("Просмотр", "Просмотр финальной работы", "OK");
        }
    }

    private async Task AddTag()
    {
        if (string.IsNullOrWhiteSpace(NewTag) || Commission == null) return;

        try
        {
            var tag = await _repository.GetTagByNameAsync(NewTag.Trim());
            if (tag == null)
            {
                tag = new Tag { Name = NewTag.Trim() };
                await _repository.AddTagAsync(tag);
            }

            Commission.Tags ??= new List<CommissionTag>();

            if (Commission.Tags.All(t => t.TagId != tag.Id))
            {
                Commission.Tags.Add(new CommissionTag { TagId = tag.Id, Tag = tag });
                NewTag = string.Empty;
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось добавить тег: {ex.Message}", "OK");
        }
    }

    private async Task RemoveTag(CommissionTag commissionTag)
    {
        if (commissionTag != null && Commission?.Tags != null)
        {
            Commission.Tags.Remove(commissionTag);
        }
    }

    private void ClearDeadline()
    {
        Commission.Deadline = null;
        OnPropertyChanged(nameof(DaysUntilDeadline));
        OnPropertyChanged(nameof(DeadlineColor));
    }

    partial void OnCommissionChanged(Commission value)
    {
        OnPropertyChanged(nameof(IsExistingCommission));
        OnPropertyChanged(nameof(HasSketch));
        OnPropertyChanged(nameof(HasFinalArt));
        OnPropertyChanged(nameof(DaysUntilDeadline));
        OnPropertyChanged(nameof(DeadlineColor));
    }

    partial void OnSelectedCustomerChanged(Customer value)
    {
        if (value != null && Commission != null)
        {
            Commission.CustomerId = value.Id;
            Commission.Customer = value;
        }
    }
}
