namespace ShouldIDeployApp.Models;

/// <summary>
/// Core deployment decision logic. Port of shouldIDeploy from constants.ts.
/// Determines whether it's safe to deploy based on day, time, and special dates.
/// </summary>
public class DeploymentChecker
{
    private static readonly Random Random = new();

    public static bool ShouldDeploy(TimeHelper time)
    {
        return !time.IsFriday() &&
               !time.IsWeekend() &&
               !time.IsHolidays() &&
               !time.IsAfternoon();
    }

    public static DeploymentResult GetResult(TimeHelper? time = null)
    {
        time ??= new TimeHelper();
        bool canDeploy = ShouldDeploy(time);
        string reason = GetReason(time);

        return new DeploymentResult(canDeploy, reason, time.GetDate());
    }

    public static string GetReason(TimeHelper time)
    {
        if (time.IsDayBeforeChristmas())
            return GetRandom(Reasons.DayBeforeChristmas);

        if (time.IsChristmas())
            return GetRandom(Reasons.Christmas);

        if (time.IsNewYear())
            return GetRandom(Reasons.NewYear);

        if (time.IsFriday13th())
            return GetRandom(Reasons.Friday13th);

        if (time.IsFridayAfternoon())
            return GetRandom(Reasons.FridayAfternoon);

        if (time.IsFriday())
            return GetRandom(Reasons.ToNotDeploy);

        if (time.IsThursdayAfternoon())
            return GetRandom(Reasons.ThursdayAfternoon);

        if (time.IsWeekend())
            return GetRandom(Reasons.Weekend);

        if (time.IsAfternoon())
            return GetRandom(Reasons.Afternoon);

        return GetRandom(Reasons.ToDeploy);
    }

    private static string GetRandom(string[] list)
    {
        return list[Random.Next(list.Length)];
    }
}

public record DeploymentResult(bool CanDeploy, string Reason, DateTime CheckedAt);
