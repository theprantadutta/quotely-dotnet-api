namespace quotely_dotnet_api.Generators;

public class RandomDateTimeGenerator
{
    // Returns Three Randomly Generated DateTimes
    public static List<DateTime> GenerateThreeRandomDateTimes()
    {
        var random = new Random();
        var currentDate = DateTime.Now;

        // Generate times for the next 7 days
        var days = Enumerable
            .Range(0, 7)
            .Select(offset => currentDate.Date.AddDays(offset))
            .ToList();

        var result = (
            from day in days.OrderBy(_ => random.Next()).Take(3)
            let randomHour = random.Next(0, 24)
            let randomMinute = random.Next(0, 60)
            let randomSecond = random.Next(0, 60)
            select new DateTime(
                day.Year,
                day.Month,
                day.Day,
                randomHour,
                randomMinute,
                randomSecond
            )
        ).ToList();

        return result.OrderBy(dt => dt).ToList(); // Return sorted by date/time
    }
}
