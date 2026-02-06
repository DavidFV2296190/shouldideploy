using ShouldIDeployApp.Models;

namespace ShouldIDeployApp.Tests;

public class TimeHelperTests
{
    [Fact]
    public void IsFriday_WhenFriday_ReturnsTrue()
    {
        // 2024-01-05 is a Friday
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 5, 10, 0, 0));
        Assert.True(time.IsFriday());
    }

    [Fact]
    public void IsFriday_WhenMonday_ReturnsFalse()
    {
        // 2024-01-01 is a Monday
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 1, 10, 0, 0));
        Assert.False(time.IsFriday());
    }

    [Fact]
    public void IsWeekend_WhenSaturday_ReturnsTrue()
    {
        // 2024-01-06 is a Saturday
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 6, 10, 0, 0));
        Assert.True(time.IsWeekend());
    }

    [Fact]
    public void IsWeekend_WhenSunday_ReturnsTrue()
    {
        // 2024-01-07 is a Sunday
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 7, 10, 0, 0));
        Assert.True(time.IsWeekend());
    }

    [Fact]
    public void IsWeekend_WhenTuesday_ReturnsFalse()
    {
        // 2024-01-02 is a Tuesday
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 2, 10, 0, 0));
        Assert.False(time.IsWeekend());
    }

    [Fact]
    public void IsAfternoon_WhenBefore4PM_ReturnsFalse()
    {
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 2, 15, 59, 0));
        Assert.False(time.IsAfternoon());
    }

    [Fact]
    public void IsAfternoon_WhenAt4PM_ReturnsTrue()
    {
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 2, 16, 0, 0));
        Assert.True(time.IsAfternoon());
    }

    [Fact]
    public void IsAfternoon_WhenAfter4PM_ReturnsTrue()
    {
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 2, 18, 0, 0));
        Assert.True(time.IsAfternoon());
    }

    [Fact]
    public void IsThursday_WhenThursday_ReturnsTrue()
    {
        // 2024-01-04 is a Thursday
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 4, 10, 0, 0));
        Assert.True(time.IsThursday());
    }

    [Fact]
    public void IsThursdayAfternoon_WhenThursdayAt5PM_ReturnsTrue()
    {
        // 2024-01-04 is a Thursday
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 4, 17, 0, 0));
        Assert.True(time.IsThursdayAfternoon());
    }

    [Fact]
    public void IsFridayAfternoon_WhenFridayAt5PM_ReturnsTrue()
    {
        // 2024-01-05 is a Friday
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 5, 17, 0, 0));
        Assert.True(time.IsFridayAfternoon());
    }

    [Fact]
    public void IsFriday13th_WhenFriday13th_ReturnsTrue()
    {
        // 2024-09-13 is a Friday
        var time = new TimeHelper(customDate: new DateTime(2024, 9, 13, 10, 0, 0));
        Assert.True(time.IsFriday13th());
    }

    [Fact]
    public void IsFriday13th_WhenFridayNot13th_ReturnsFalse()
    {
        // 2024-01-05 is a Friday but not the 13th
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 5, 10, 0, 0));
        Assert.False(time.IsFriday13th());
    }

    [Fact]
    public void Is13th_WhenDayIs13_ReturnsTrue()
    {
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 13, 10, 0, 0));
        Assert.True(time.Is13th());
    }

    [Fact]
    public void IsChristmas_WhenDec25_ReturnsTrue()
    {
        var time = new TimeHelper(customDate: new DateTime(2024, 12, 25, 10, 0, 0));
        Assert.True(time.IsChristmas());
    }

    [Fact]
    public void IsChristmas_WhenDec24_ReturnsFalse()
    {
        var time = new TimeHelper(customDate: new DateTime(2024, 12, 24, 10, 0, 0));
        Assert.False(time.IsChristmas());
    }

    [Fact]
    public void IsDayBeforeChristmas_WhenDec24After4PM_ReturnsTrue()
    {
        var time = new TimeHelper(customDate: new DateTime(2024, 12, 24, 16, 0, 0));
        Assert.True(time.IsDayBeforeChristmas());
    }

    [Fact]
    public void IsDayBeforeChristmas_WhenDec24Before4PM_ReturnsFalse()
    {
        var time = new TimeHelper(customDate: new DateTime(2024, 12, 24, 15, 0, 0));
        Assert.False(time.IsDayBeforeChristmas());
    }

    [Fact]
    public void IsNewYear_WhenDec31After4PM_ReturnsTrue()
    {
        var time = new TimeHelper(customDate: new DateTime(2024, 12, 31, 16, 0, 0));
        Assert.True(time.IsNewYear());
    }

    [Fact]
    public void IsNewYear_WhenJan1_ReturnsTrue()
    {
        var time = new TimeHelper(customDate: new DateTime(2024, 1, 1, 10, 0, 0));
        Assert.True(time.IsNewYear());
    }

    [Fact]
    public void IsNewYear_WhenDec31Before4PM_ReturnsFalse()
    {
        var time = new TimeHelper(customDate: new DateTime(2024, 12, 31, 15, 0, 0));
        Assert.False(time.IsNewYear());
    }

    [Fact]
    public void IsHolidays_WhenChristmas_ReturnsTrue()
    {
        var time = new TimeHelper(customDate: new DateTime(2024, 12, 25, 10, 0, 0));
        Assert.True(time.IsHolidays());
    }

    [Fact]
    public void IsHolidays_WhenNormalDay_ReturnsFalse()
    {
        var time = new TimeHelper(customDate: new DateTime(2024, 3, 5, 10, 0, 0));
        Assert.False(time.IsHolidays());
    }

    [Fact]
    public void GetDate_WithCustomDate_ReturnsCustomDate()
    {
        var customDate = new DateTime(2024, 6, 15, 14, 30, 0);
        var time = new TimeHelper(customDate: customDate);
        Assert.Equal(customDate, time.GetDate());
    }

    [Fact]
    public void Constructor_WithInvalidTimezone_FallsBackToUtc()
    {
        var time = new TimeHelper("Invalid/Timezone");
        Assert.Equal("UTC", time.TimezoneName);
    }

    [Fact]
    public void Constructor_WithNullTimezone_UsesDefault()
    {
        var time = new TimeHelper(null);
        Assert.Equal("UTC", time.TimezoneName);
    }
}
