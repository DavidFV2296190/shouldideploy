using Avalonia.Controls;
using Avalonia.Threading;
using ShouldIDeployApp.ViewModels;

namespace ShouldIDeployApp.Views;

public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _viewModel;
    private bool _forceClose;

    /// <summary>
    /// Raised when the deployment status changes, so the App can update the tray icon color.
    /// </summary>
    public event Action<bool>? DeployStatusChanged;

    /// <summary>
    /// Gets the current deployment status from the view model.
    /// </summary>
    public bool CurrentDeployStatus => _viewModel.CanDeploy;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainWindowViewModel();
        DataContext = _viewModel;

        // Wire up full-screen toggle
        _viewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(MainWindowViewModel.IsFullScreen))
            {
                Dispatcher.UIThread.Post(() =>
                {
                    WindowState = _viewModel.IsFullScreen
                        ? WindowState.FullScreen
                        : WindowState.Normal;
                });
            }
        };

        // Ensure status updates happen on the UI thread and notify tray icon
        _viewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(MainWindowViewModel.StatusText)
                or nameof(MainWindowViewModel.Reason)
                or nameof(MainWindowViewModel.BackgroundColor)
                or nameof(MainWindowViewModel.IsToastVisible))
            {
                Dispatcher.UIThread.Post(() => InvalidateVisual());
            }

            if (e.PropertyName == nameof(MainWindowViewModel.CanDeploy))
            {
                DeployStatusChanged?.Invoke(_viewModel.CanDeploy);
            }
        };

        // Start the notification scheduler
        _viewModel.Initialize();
    }

    /// <summary>
    /// Allows the tray icon "Exit" command to truly close the window.
    /// </summary>
    public void ForceClose()
    {
        _forceClose = true;
        Close();
    }

    /// <summary>
    /// Refreshes the deployment status (used by the tray icon context menu).
    /// </summary>
    public void TrayRefresh()
    {
        _viewModel.Refresh();
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (!_forceClose)
        {
            // Minimize to tray instead of closing
            e.Cancel = true;
            Hide();
            return;
        }

        _viewModel.Dispose();
        base.OnClosing(e);
    }
}
