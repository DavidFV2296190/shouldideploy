using Avalonia.Controls;
using Avalonia.Threading;
using ShouldIDeployApp.ViewModels;

namespace ShouldIDeployApp.Views;

public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _viewModel;

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

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        _viewModel.Dispose();
        base.OnClosing(e);
    }
}
