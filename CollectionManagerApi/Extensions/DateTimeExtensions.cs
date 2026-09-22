namespace CollectionManagerApi.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ConvertDateTimeFromUtcToLocal(this DateTime serverDateTime)
    {
        string? timeZone = Environment.GetEnvironmentVariable("Time_Zone");
        if (timeZone == null) timeZone = "Central Standard Time";
        TimeZoneInfo localTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
        var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(serverDateTime, localTimeZoneInfo);
        return localDateTime;
    }
}