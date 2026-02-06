using ShouldIDeployApp.Models;

namespace ShouldIDeployApp.Tests;

public class DeploymentCheckerTests
{
    [Fact]
    public void ShouldDeploy_TuesdayMorning_ReturnsTrue()
    {
        // Tuesday 2024-01-02 at 10:00 AM - safe to deploy
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 2, 10, 0, 0));
        Assert.True(DeploymentChecker.ShouldDeploy(time));
    }

    [Fact]
    public void ShouldDeploy_WednesdayMorning_ReturnsTrue()
    {
        // Wednesday 2024-01-03 at 9:00 AM - safe to deploy
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 3, 9, 0, 0));
        Assert.True(DeploymentChecker.ShouldDeploy(time));
    }

    [Fact]
    public void ShouldDeploy_Friday_ReturnsFalse()
    {
        // Friday 2024-01-05 at 10:00 AM
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 5, 10, 0, 0));
        Assert.False(DeploymentChecker.ShouldDeploy(time));
    }

    [Fact]
    public void ShouldDeploy_Saturday_ReturnsFalse()
    {
        // Saturday 2024-01-06
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 6, 10, 0, 0));
        Assert.False(DeploymentChecker.ShouldDeploy(time));
    }

    [Fact]
    public void ShouldDeploy_Sunday_ReturnsFalse()
    {
        // Sunday 2024-01-07
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 7, 10, 0, 0));
        Assert.False(DeploymentChecker.ShouldDeploy(time));
    }

    [Fact]
    public void ShouldDeploy_After4PM_ReturnsFalse()
    {
        // Tuesday 2024-01-02 at 4:00 PM
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 2, 16, 0, 0));
        Assert.False(DeploymentChecker.ShouldDeploy(time));
    }

    [Fact]
    public void ShouldDeploy_At359PM_ReturnsTrue()
    {
        // Tuesday 2024-01-02 at 3:59 PM - still safe
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 2, 15, 59, 0));
        Assert.True(DeploymentChecker.ShouldDeploy(time));
    }

    [Fact]
    public void ShouldDeploy_Christmas_ReturnsFalse()
    {
        // Wednesday Dec 25 2024
        var time = new TimeHelper(customDate: new DateTime(2024, 12, 25, 10, 0, 0));
        Assert.False(DeploymentChecker.ShouldDeploy(time));
    }

    [Fact]
    public void ShouldDeploy_NewYearsDay_ReturnsFalse()
    {
        // Monday Jan 1 2024
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 1, 10, 0, 0));
        Assert.False(DeploymentChecker.ShouldDeploy(time));
    }

    [Fact]
    public void GetResult_ReturnsCorrectResult()
    {
        // Tuesday morning - safe to deploy
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 2, 10, 0, 0));
        var result = DeploymentChecker.GetResult(time);

        Assert.True(result.CanDeploy);
        Assert.NotNull(result.Reason);
        Assert.NotEmpty(result.Reason);
        Assert.Contains(result.Reason, Reasons.ToDeploy);
    }

    [Fact]
    public void GetResult_FridayMorning_ReturnsNotDeploy()
    {
        // Friday 2024-01-05 at 10:00 AM
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 5, 10, 0, 0));
        var result = DeploymentChecker.GetResult(time);

        Assert.False(result.CanDeploy);
        Assert.NotNull(result.Reason);
        Assert.Contains(result.Reason, Reasons.ToNotDeploy);
    }

    [Fact]
    public void GetResult_FridayAfternoon_ReturnsFridayAfternoonReason()
    {
        // Friday 2024-01-05 at 5:00 PM
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 5, 17, 0, 0));
        var result = DeploymentChecker.GetResult(time);

        Assert.False(result.CanDeploy);
        Assert.Contains(result.Reason, Reasons.FridayAfternoon);
    }

    [Fact]
    public void GetResult_ThursdayAfternoon_ReturnsThursdayAfternoonReason()
    {
        // Thursday 2024-01-04 at 5:00 PM
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 4, 17, 0, 0));
        var result = DeploymentChecker.GetResult(time);

        Assert.False(result.CanDeploy);
        Assert.Contains(result.Reason, Reasons.ThursdayAfternoon);
    }

    [Fact]
    public void GetResult_Weekend_ReturnsWeekendReason()
    {
        // Saturday 2024-01-06 at 10:00 AM
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 6, 10, 0, 0));
        var result = DeploymentChecker.GetResult(time);

        Assert.False(result.CanDeploy);
        Assert.Contains(result.Reason, Reasons.Weekend);
    }

    [Fact]
    public void GetResult_WeekdayAfternoon_ReturnsAfternoonReason()
    {
        // Tuesday 2024-01-02 at 5:00 PM
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 2, 17, 0, 0));
        var result = DeploymentChecker.GetResult(time);

        Assert.False(result.CanDeploy);
        Assert.Contains(result.Reason, Reasons.Afternoon);
    }

    [Fact]
    public void GetResult_Christmas_ReturnsChristmasReason()
    {
        // Wednesday Dec 25 2024 at 10:00 AM
        var time = new TimeHelper(customDate: new DateTime(2024, 12, 25, 10, 0, 0));
        var result = DeploymentChecker.GetResult(time);

        Assert.False(result.CanDeploy);
        Assert.Contains(result.Reason, Reasons.Christmas);
    }

    [Fact]
    public void GetResult_Friday13th_ReturnsFriday13thReason()
    {
        // Friday Sep 13 2024 at 10:00 AM
        var time = new TimeHelper(customDate: new DateTime(2024, 9, 13, 10, 0, 0));
        var result = DeploymentChecker.GetResult(time);

        Assert.False(result.CanDeploy);
        Assert.Contains(result.Reason, Reasons.Friday13th);
    }

    [Fact]
    public void GetResult_DayBeforeChristmasAfternoon_ReturnsChristmasEveReason()
    {
        // Tuesday Dec 24 2024 at 4:00 PM
        var time = new TimeHelper(customDate: new DateTime(2024, 12, 24, 16, 0, 0));
        var result = DeploymentChecker.GetResult(time);

        Assert.False(result.CanDeploy);
        Assert.Contains(result.Reason, Reasons.DayBeforeChristmas);
    }

    [Fact]
    public void GetResult_NewYearsEveAfternoon_ReturnsNewYearReason()
    {
        // Tuesday Dec 31 2024 at 4:00 PM
        var time = new TimeHelper(customDate: new DateTime(2024, 12, 31, 16, 0, 0));
        var result = DeploymentChecker.GetResult(time);

        Assert.False(result.CanDeploy);
        Assert.Contains(result.Reason, Reasons.NewYear);
    }

    [Fact]
    public void GetResult_CheckedAt_MatchesTimeHelperDate()
    {
        var customDate = new DateTime(2024, 1, 2, 10, 0, 0);
        var time = new TimeHelper(customDate: customDate);
        var result = DeploymentChecker.GetResult(time);

        Assert.Equal(customDate, result.CheckedAt);
    }
}
