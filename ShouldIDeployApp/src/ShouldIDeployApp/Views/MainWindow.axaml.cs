using Avalonia.Controls;
using Avalonia.Threading;
using ShouldIDeployApp.ViewModels;

namespace ShouldIDeployApp.Views;

public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _viewModel;
    private bool _forceClose;

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

        // Ensure status updates happen on the UI thread
        _viewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(MainWindowViewModel.StatusText)
                or nameof(MainWindowViewModel.Reason)
                or nameof(MainWindowViewModel.BackgroundColor)
                or nameof(MainWindowViewModel.IsToastVisible))
            {
                Dispatcher.UIThread.Post(() => InvalidateVisual());
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
