// ReSharper disable IdentifierTypo

// ReSharper disable CommentTypo
namespace HamedStack.PasswordMeter;

/// <summary>
/// A utility class for converting total seconds into a human-readable time format.
/// </summary>
internal static class TimeConverter
{
    /// <summary>
    /// Converts the given total seconds into various time units, such as years, months, days, etc.
    /// </summary>
    /// <param name="totalSeconds">The total number of seconds to convert.</param>
    /// <returns>An instance of <see cref="TimeUnits"/> representing the converted time units.</returns>
    internal static TimeUnits SecondsToTimeUnits(double totalSeconds)
    {
        const double secInMin = 60;
        const double secInHour = secInMin * 60;
        const double secInDay = secInHour * 24;
        const double secInMonth = secInDay * 30.44; // Average month length
        const double secInYear = secInDay * 365.25; // Average year length considering leap years
        const double secInDecade = secInYear * 10;
        const double secInCentury = secInYear * 100;
        const double secInMillennium = secInYear * 1000;

        var millennium = (int) (totalSeconds / secInMillennium);
        totalSeconds %= secInMillennium;

        var centuries = (int) (totalSeconds / secInCentury);
        totalSeconds %= secInCentury;

        var decades = (int) (totalSeconds / secInDecade);
        totalSeconds %= secInDecade;

        var years = (int) (totalSeconds / secInYear);
        totalSeconds %= secInYear;

        var months = (int) (totalSeconds / secInMonth);
        totalSeconds %= secInMonth;

        var days = (int) (totalSeconds / secInDay);
        totalSeconds %= secInDay;

        var hours = (int) (totalSeconds / secInHour);
        totalSeconds %= secInHour;

        var minutes = (int) (totalSeconds / secInMin);
        totalSeconds %= secInMin;

        var seconds = (int) Math.Floor(totalSeconds);

        return new TimeUnits
        {
            Millennium = millennium,
            Centuries = centuries,
            Decades = decades,
            Years = years,
            Months = months,
            Days = days,
            Hours = hours,
            Minutes = minutes,
            Seconds = seconds
        };
    }

    /// <summary>
    /// Formats the provided <see cref="TimeUnits"/> into a human-readable string.
    /// </summary>
    /// <param name="timeUnits">The instance of <see cref="TimeUnits"/> to format.</param>
    /// <returns>A formatted string representing the time units.</returns>
    internal static string FormatTimeUnits(TimeUnits timeUnits)
    {
        if (AreAllValuesZero(timeUnits))
        {
            return "instantly";
        }

        var nonZeroUnits = GetNonZeroUnits(timeUnits);

        return string.Join(", ", nonZeroUnits);
    }

    /// <summary>
    /// Singularizes the provided time unit based on its value.
    /// </summary>
    /// <param name="unit">The time unit to singularize.</param>
    /// <param name="value">The value of the time unit.</param>
    /// <returns>The singularized form of the time unit.</returns>
    private static string Singularize(string unit, int value)
    {
        if (value == 1)
        {
            return unit switch
            {
                "millenniums" => "millennium",
                "centuries" => "century",
                "decades" => "decade",
                "years" => "year",
                "months" => "month",
                "days" => "day",
                "hours" => "hour",
                "minutes" => "minute",
                "seconds" => "second",
                _ => unit
            };
        }

        return unit;
    }

    /// <summary>
    /// Checks if all values in the provided <see cref="TimeUnits"/> are zero.
    /// </summary>
    /// <param name="timeUnits">The instance of <see cref="TimeUnits"/> to check.</param>
    /// <returns>True if all values are zero, otherwise false.</returns>
    private static bool AreAllValuesZero(TimeUnits timeUnits)
    {
        return timeUnits is
            {Millennium: 0, Centuries: 0, Decades: 0, Years: 0, Months: 0, Days: 0, Hours: 0, Minutes: 0, Seconds: 0};
    }

    /// <summary>
    /// Gets an array of non-zero time units from the provided <see cref="TimeUnits"/>.
    /// </summary>
    /// <param name="timeUnits">The instance of <see cref="TimeUnits"/> to process.</param>
    /// <returns>An array of formatted strings representing non-zero time units.</returns>
    private static string[] GetNonZeroUnits(TimeUnits timeUnits)
    {
        var nonZeroUnits = new List<string>();

        if (timeUnits.Millennium > 0)
            nonZeroUnits.Add($"{timeUnits.Millennium} {Singularize("millenniums", timeUnits.Millennium)}");

        if (timeUnits.Centuries > 0)
            nonZeroUnits.Add($"{timeUnits.Centuries} {Singularize("centuries", timeUnits.Centuries)}");

        if (timeUnits.Decades > 0)
            nonZeroUnits.Add($"{timeUnits.Decades} {Singularize("decades", timeUnits.Decades)}");

        if (timeUnits.Years > 0)
            nonZeroUnits.Add($"{timeUnits.Years} {Singularize("years", timeUnits.Years)}");

        if (timeUnits.Months > 0)
            nonZeroUnits.Add($"{timeUnits.Months} {Singularize("months", timeUnits.Months)}");

        if (timeUnits.Days > 0)
            nonZeroUnits.Add($"{timeUnits.Days} {Singularize("days", timeUnits.Days)}");

        if (timeUnits.Hours > 0)
            nonZeroUnits.Add($"{timeUnits.Hours} {Singularize("hours", timeUnits.Hours)}");

        if (timeUnits.Minutes > 0)
            nonZeroUnits.Add($"{timeUnits.Minutes} {Singularize("minutes", timeUnits.Minutes)}");

        if (timeUnits.Seconds > 0)
            nonZeroUnits.Add($"{timeUnits.Seconds} {Singularize("seconds", timeUnits.Seconds)}");

        return nonZeroUnits.ToArray();
    }
}