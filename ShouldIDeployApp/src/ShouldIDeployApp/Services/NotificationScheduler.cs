using System.Timers;
using ShouldIDeployApp.Models;
using Timer = System.Timers.Timer;

namespace ShouldIDeployApp.Services;

/// <summary>
/// Manages toast notification scheduling with a smart frequency strategy:
/// - During work hours (9 AM - 6 PM weekdays): notify every 45 minutes
/// - Outside work hours: notify every 2 hours
/// - Immediate notification on status change (safe → unsafe or vice versa)
/// - Status check runs every 5 minutes internally
///
/// This keeps junior developers informed without being intrusive enough
/// that they're tempted to close the app.
/// </summary>
public class NotificationScheduler : IDisposable
{
    private readonly Timer _checkTimer;
    private readonly string _timezone;
    private bool? _lastDeployStatus;
    private DateTime _lastNotificationTime = DateTime.MinValue;

    /// <summary>
    /// Interval between toast notifications during work hours (45 minutes).
    /// Frequent enough to keep developers informed, but not so frequent
    /// that it becomes annoying.
    /// </summary>
    public static readonly TimeSpan WorkHoursInterval = TimeSpan.FromMinutes(45);

    /// <summary>
    /// Interval between toast notifications outside work hours (2 hours).
    /// Very light touch for off-hours.
    /// </summary>
    public static readonly TimeSpan OffHoursInterval = TimeSpan.FromHours(2);

    /// <summary>
    /// How often we internally check the deployment status (5 minutes).
    /// This ensures we catch status changes promptly.
    /// </summary>
    public static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(5);

    public event Action<DeploymentResult>? OnNotificationRequired;
    public event Action<DeploymentResult>? OnStatusUpdated;

    public NotificationScheduler(string? timezone = null)
    {
        _timezone = timezone ?? TimeHelper.DefaultTimezone;
        _checkTimer = new Timer(CheckInterval.TotalMilliseconds);
        _checkTimer.Elapsed += OnCheckTimerElapsed;
        _checkTimer.AutoReset = true;
    }

    public void Start()
    {
        // Perform an immediate check and notification on start
        PerformCheck(forceNotification: true);
        _checkTimer.Start();
    }

    public void Stop()
    {
        _checkTimer.Stop();
    }

    public void UpdateTimezone(string timezone)
    {
        // Re-check immediately with new timezone by triggering a forced check
        var time = new TimeHelper(timezone);
        var result = DeploymentChecker.GetResult(time);
        _lastDeployStatus = result.CanDeploy;
        OnStatusUpdated?.Invoke(result);
        OnNotificationRequired?.Invoke(result);
        _lastNotificationTime = DateTime.UtcNow;
    }

    private void OnCheckTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        PerformCheck(forceNotification: false);
    }

    private void PerformCheck(bool forceNotification)
    {
        var time = new TimeHelper(_timezone);
        var result = DeploymentChecker.GetResult(time);

        // Always update the UI
        OnStatusUpdated?.Invoke(result);

        bool statusChanged = _lastDeployStatus.HasValue && _lastDeployStatus.Value != result.CanDeploy;
        _lastDeployStatus = result.CanDeploy;

        // Determine if we should show a toast notification
        if (forceNotification || statusChanged || ShouldShowPeriodicNotification(time))
        {
            OnNotificationRequired?.Invoke(result);
            _lastNotificationTime = DateTime.UtcNow;
        }
    }

    private bool ShouldShowPeriodicNotification(TimeHelper time)
    {
        var timeSinceLastNotification = DateTime.UtcNow - _lastNotificationTime;
        var interval = IsWorkHours(time) ? WorkHoursInterval : OffHoursInterval;
        return timeSinceLastNotification >= interval;
    }

    private static bool IsWorkHours(TimeHelper time)
    {
        var date = time.GetDate();
        return date.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday) &&
               date.Hour >= 9 && date.Hour < 18;
    }

    public void Dispose()
    {
        _checkTimer.Stop();
        _checkTimer.Dispose();
        GC.SuppressFinalize(this);
    }
}
