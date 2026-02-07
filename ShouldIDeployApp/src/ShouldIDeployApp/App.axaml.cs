using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ShouldIDeployApp.Services;
using ShouldIDeployApp.Views;

namespace ShouldIDeployApp;

public class App : Application
{
    private MainWindow? _mainWindow;

    public ICommand OpenCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ExitCommand { get; }

    public App()
    {
        OpenCommand = new SimpleCommand(ShowMainWindow);
        RefreshCommand = new SimpleCommand(RefreshStatus);
        ExitCommand = new SimpleCommand(ExitApplication);
        DataContext = this;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _mainWindow = new MainWindow();
            desktop.MainWindow = _mainWindow;

            // Prevent app from shutting down when the window is hidden (minimized to tray)
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // Update tray icon color when deployment status changes
            _mainWindow.DeployStatusChanged += OnDeployStatusChanged;

            // Set initial tray icon based on actual deployment status
            UpdateTrayIcon(_mainWindow.CurrentDeployStatus);
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void OnDeployStatusChanged(bool canDeploy)
    {
        Dispatcher.UIThread.Post(() => UpdateTrayIcon(canDeploy));
    }

    private void UpdateTrayIcon(bool canDeploy)
    {
        var icons = TrayIcon.GetIcons(this);
        if (icons is { Count: > 0 })
        {
            icons[0].Icon = TrayIconHelper.CreateTrayIcon(canDeploy);
            icons[0].ToolTipText = canDeploy
                ? "Should I Deploy? ✅ Yes!"
                : "Should I Deploy? ⛔ No!";
        }
    }

    private void ShowMainWindow()
    {
        if (_mainWindow is null) return;
        _mainWindow.Show();
        _mainWindow.WindowState = WindowState.Normal;
        _mainWindow.Activate();
    }

    private void RefreshStatus()
    {
        _mainWindow?.TrayRefresh();
    }

    private void ExitApplication()
    {
        _mainWindow?.ForceClose();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}

/// <summary>
/// Minimal ICommand implementation for tray icon menu commands.
/// </summary>
internal sealed class SimpleCommand : ICommand
{
    private readonly Action _execute;

    public SimpleCommand(Action execute)
    {
        _execute = execute;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) => _execute();

    public event EventHandler? CanExecuteChanged
    {
        add { }
        remove { }
    }
}
