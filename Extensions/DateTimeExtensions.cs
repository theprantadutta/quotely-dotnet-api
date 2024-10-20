namespace quotely_dotnet_api.Extensions;

public static class DateTimeExtensions
{
    public static string ToCustomLogDateFormat(this DateTime dateTime)
    {
        return dateTime.ToString("dd MMMM, yyyy h:mm:sstt");
    }
}