using System.Collections.ObjectModel;
using System.Windows.Input;
using CommsManager.Interfaces;
using CommsManager.Models;
using CommsManager.Models.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CommsManager.ViewModels;

public partial class CommissionsViewModel : BaseViewModel
{
    private readonly IRepository _repository;
    private Commission _selectedCommission;

    [ObservableProperty]
    private ObservableCollection<Commission> _commissions = new();

    [ObservableProperty]
    private ObservableCollection<Commission> _filteredCommissions = new();

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private CommissionStatus? _selectedStatusFilter;

    [ObservableProperty]
    private string _selectedSortOption = "По дате создания";

    [ObservableProperty]
    private bool _isRefreshing;

    public ObservableCollection<CommissionStatus?> StatusFilters { get; } = new()
    {
        null,
        CommissionStatus.Queued,
        CommissionStatus.InProgress,
        CommissionStatus.UnderReview,
        CommissionStatus.Completed
    };

    public ObservableCollection<string> SortOptions { get; } = new()
    {
        "По дате создания",
        "По дедлайну",
        "По приоритету",
        "По статусу"
    };

    public int CommissionsCount => FilteredCommissions.Count;

    public ICommand LoadCommissionsCommand { get; }
    public ICommand SelectCommissionCommand { get; }
    public ICommand CreateCommissionCommand { get; }
    public ICommand RefreshCommand { get; }

    public CommissionsViewModel(IRepository repository)
    {
        _repository = repository;

        LoadCommissionsCommand = new AsyncRelayCommand(LoadCommissions);
        SelectCommissionCommand = new AsyncRelayCommand<Commission>(SelectCommission);
        CreateCommissionCommand = new AsyncRelayCommand(CreateCommission);
        RefreshCommand = new AsyncRelayCommand(Refresh);

        PropertyChanged += OnPropertyChanged;
    }

    private async void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SearchText) || 
            e.PropertyName == nameof(SelectedStatusFilter) ||
            e.PropertyName == nameof(SelectedSortOption))
        {
            await FilterAndSortCommissions();
        }
    }

    private async Task LoadCommissions()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            IsRefreshing = true;
            var commissions = await _repository.GetCommissionsAsync();

            Commissions.Clear();
            foreach (var commission in commissions)
            {
                Commissions.Add(commission);
            }

            await FilterAndSortCommissions();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить комиссии: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    private async Task FilterAndSortCommissions()
    {
        var filtered = Commissions.Where(commission =>
        {
            var matchesSearch = string.IsNullOrEmpty(SearchText) ||
                                commission.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                commission.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                commission.Customer.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

            var matchesStatus = !SelectedStatusFilter.HasValue || commission.Status == SelectedStatusFilter.Value;

            return matchesSearch && matchesStatus;
        });

        var sorted = SelectedSortOption switch
        {
            "По дедлайну" => filtered.OrderBy(c => c.Deadline ?? DateTime.MaxValue),
            "По приоритету" => filtered.OrderByDescending(c => c.Priority),
            "По статусу" => filtered.OrderBy(c => c.Status),
            _ => filtered.OrderByDescending(c => c.CreatedDate)
        };

        FilteredCommissions.Clear();
        foreach (var commission in sorted)
        {
            FilteredCommissions.Add(commission);
        }

        OnPropertyChanged(nameof(CommissionsCount));
    }

    private async Task SelectCommission(Commission commission)
    {
        if (commission == null) return;

        var parameters = new Dictionary<string, object>
        {
            ["Commission"] = commission
        };

        await Shell.Current.GoToAsync("CommissionDetailPage", parameters);
    }

    private async Task CreateCommission()
    {
        var newCommission = new Commission
        {
            CreatedDate = DateTime.UtcNow,
            Title = "Новая комиссия",
            Status = CommissionStatus.Queued,
            Price = 0,
            ArtType = ArtType.Other,
            Priority = 1,
            IsPaid = false,
            Tags = new List<CommissionTag>(),
            Artworks = new List<Artwork>(),
        };

        var parameters = new Dictionary<string, object>
        {
            ["Commission"] = newCommission
        };

        await Shell.Current.GoToAsync("CommissionDetailPage", parameters);
    }

    private async Task Refresh()
    {
        await LoadCommissions();
        IsRefreshing = false;
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await LoadCommissions();
    }
}
