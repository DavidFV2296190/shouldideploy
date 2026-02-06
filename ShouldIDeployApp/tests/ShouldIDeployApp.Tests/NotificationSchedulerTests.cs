using ShouldIDeployApp.Models;
using ShouldIDeployApp.Services;

namespace ShouldIDeployApp.Tests;

public class NotificationSchedulerTests
{
    [Fact]
    public void Start_TriggersImmediateStatusUpdate()
    {
        using var scheduler = new NotificationScheduler("UTC");
        DeploymentResult? receivedResult = null;

        scheduler.OnStatusUpdated += result => receivedResult = result;
        scheduler.Start();

        // Give it a moment to process
        Thread.Sleep(100);
        scheduler.Stop();

        Assert.NotNull(receivedResult);
    }

    [Fact]
    public void Start_TriggersImmediateNotification()
    {
        using var scheduler = new NotificationScheduler("UTC");
        DeploymentResult? notificationResult = null;

        scheduler.OnNotificationRequired += result => notificationResult = result;
        scheduler.Start();

        Thread.Sleep(100);
        scheduler.Stop();

        Assert.NotNull(notificationResult);
    }

    [Fact]
    public void UpdateTimezone_TriggersNotification()
    {
        using var scheduler = new NotificationScheduler("UTC");
        DeploymentResult? notificationResult = null;

        scheduler.OnNotificationRequired += result => notificationResult = result;
        scheduler.UpdateTimezone("US/Eastern");

        Assert.NotNull(notificationResult);
    }

    [Fact]
    public void WorkHoursInterval_Is45Minutes()
    {
        Assert.Equal(TimeSpan.FromMinutes(45), NotificationScheduler.WorkHoursInterval);
    }

    [Fact]
    public void OffHoursInterval_Is2Hours()
    {
        Assert.Equal(TimeSpan.FromHours(2), NotificationScheduler.OffHoursInterval);
    }

    [Fact]
    public void CheckInterval_Is5Minutes()
    {
        Assert.Equal(TimeSpan.FromMinutes(5), NotificationScheduler.CheckInterval);
    }

    [Fact]
    public void Dispose_StopsScheduler()
    {
        var scheduler = new NotificationScheduler("UTC");
        scheduler.Start();
        scheduler.Dispose();

        // No exception should be thrown
        Assert.True(true);
    }
}
