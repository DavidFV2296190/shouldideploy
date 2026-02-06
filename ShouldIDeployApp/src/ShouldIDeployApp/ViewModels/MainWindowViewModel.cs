using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShouldIDeployApp.Models;
using ShouldIDeployApp.Services;

namespace ShouldIDeployApp.ViewModels;

public partial class MainWindowViewModel : ObservableObject, IDisposable
{
    private readonly NotificationScheduler _scheduler;

    [ObservableProperty]
    private bool _canDeploy;

    [ObservableProperty]
    private string _statusText = "Checking...";

    [ObservableProperty]
    private string _reason = "";

    [ObservableProperty]
    private string _lastChecked = "";

    [ObservableProperty]
    private string _backgroundColor = "#FFFFFF";

    [ObservableProperty]
    private string _foregroundColor = "#111111";

    [ObservableProperty]
    private bool _isFullScreen;

    [ObservableProperty]
    private string _selectedTimezone;

    [ObservableProperty]
    private string _toastMessage = "";

    [ObservableProperty]
    private bool _isToastVisible;

    [ObservableProperty]
    private string _fullScreenButtonText = "⛶ Full Screen";

    public string[] AvailableTimezones { get; }

    public event Action<DeploymentResult>? OnToastRequested;

    public MainWindowViewModel()
    {
        AvailableTimezones = GetCommonTimezones();
        _selectedTimezone = TimeHelper.DefaultTimezone;

        _scheduler = new NotificationScheduler(_selectedTimezone);
        _scheduler.OnStatusUpdated += OnStatusUpdated;
        _scheduler.OnNotificationRequired += OnNotificationRequired;
    }

    public void Initialize()
    {
        _scheduler.Start();
    }

    partial void OnSelectedTimezoneChanged(string value)
    {
        _scheduler.UpdateTimezone(value);
    }

    partial void OnIsFullScreenChanged(bool value)
    {
        FullScreenButtonText = value ? "⊡ Exit Full Screen" : "⛶ Full Screen";
    }

    [RelayCommand]
    private void ToggleFullScreen()
    {
        IsFullScreen = !IsFullScreen;
    }

    [RelayCommand]
    private void Refresh()
    {
        var time = new TimeHelper(SelectedTimezone);
        var result = DeploymentChecker.GetResult(time);
        UpdateFromResult(result);
    }

    [RelayCommand]
    private void DismissToast()
    {
        IsToastVisible = false;
    }

    private void OnStatusUpdated(DeploymentResult result)
    {
        UpdateFromResult(result);
    }

    private void OnNotificationRequired(DeploymentResult result)
    {
        ToastMessage = result.CanDeploy
            ? $"✅ Safe to deploy! {result.Reason}"
            : $"⛔ Don't deploy! {result.Reason}";
        IsToastVisible = true;
        OnToastRequested?.Invoke(result);

        // Auto-dismiss toast after 8 seconds - long enough to read, short enough not to annoy
        _ = AutoDismissToastAsync();
    }

    private async Task AutoDismissToastAsync()
    {
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(8));
            IsToastVisible = false;
        }
        catch (TaskCanceledException)
        {
            // Expected when the app is closing
        }
    }

    private void UpdateFromResult(DeploymentResult result)
    {
        CanDeploy = result.CanDeploy;
        StatusText = result.CanDeploy ? "Yes!" : "No!";
        Reason = result.Reason;
        LastChecked = $"Last checked: {result.CheckedAt:HH:mm:ss} ({SelectedTimezone})";

        if (result.CanDeploy)
        {
            BackgroundColor = "#FFFFFF";
            ForegroundColor = "#111111";
        }
        else
        {
            BackgroundColor = "#FF4136";
            ForegroundColor = "#FFFFFF";
        }
    }

    private static string[] GetCommonTimezones()
    {
        return TimeZoneInfo.GetSystemTimeZones()
            .Select(tz => tz.Id)
            .ToArray();
    }

    public void Dispose()
    {
        _scheduler.Dispose();
        GC.SuppressFinalize(this);
    }
}
