namespace ShouldIDeployApp.Models;

/// <summary>
/// Port of the TypeScript Time helper class.
/// Provides timezone-aware date/time checks for deployment decisions.
/// </summary>
public class TimeHelper
{
    public static string DefaultTimezone => TimeZoneInfo.Local.Id;

    private readonly TimeZoneInfo _timeZone;
    private readonly DateTime? _customDate;

    public TimeHelper(string? timezone = null, DateTime? customDate = null)
    {
        timezone ??= DefaultTimezone;
        _timeZone = GetTimeZoneOrDefault(timezone);
        _customDate = customDate;
    }

    public string TimezoneName => _timeZone.Id;

    private static TimeZoneInfo GetTimeZoneOrDefault(string timezone)
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timezone);
        }
        catch (TimeZoneNotFoundException)
        {
            return TimeZoneInfo.Utc;
        }
    }

    public DateTime GetDate()
    {
        if (_customDate.HasValue)
            return _customDate.Value;

        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);
    }

    public bool IsThursday() => GetDate().DayOfWeek == DayOfWeek.Thursday;

    public bool IsFriday() => GetDate().DayOfWeek == DayOfWeek.Friday;

    public bool Is13th() => GetDate().Day == 13;

    public bool IsAfternoon() => GetDate().Hour >= 16;

    public bool IsThursdayAfternoon() => IsThursday() && IsAfternoon();

    public bool IsFridayAfternoon() => IsFriday() && IsAfternoon();

    public bool IsFriday13th() => IsFriday() && Is13th();

    public bool IsWeekend() => GetDate().DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    public bool IsDayBeforeChristmas()
    {
        var date = GetDate();
        return date.Month == 12 && date.Day == 24 && date.Hour >= 16;
    }

    public bool IsChristmas()
    {
        var date = GetDate();
        return date.Month == 12 && date.Day == 25;
    }

    public bool IsNewYear()
    {
        var date = GetDate();
        return (date.Month == 12 && date.Day == 31 && date.Hour >= 16) ||
               (date.Month == 1 && date.Day == 1);
    }

    public bool IsHolidays() => IsDayBeforeChristmas() || IsChristmas() || IsNewYear();
}
