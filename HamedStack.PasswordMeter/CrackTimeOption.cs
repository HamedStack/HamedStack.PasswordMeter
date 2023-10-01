namespace HamedStack.PasswordMeter;

/// <summary>
/// Represents options for cracking time calculations.
/// </summary>
public class CrackTimeOption
{
    /// <summary>
    /// Gets or sets the estimated number of guesses per second during a cracking attempt.
    /// </summary>
    /// <remarks>
    /// The default value is null, and it's recommended to set a value for more accurate calculations.
    /// For example, you may set it to Math.Pow(10, 11) * 5 based on your system's capabilities.
    /// </remarks>
    public double? GuessesPerSecond { get; set; }

    /// <summary>
    /// Gets or sets the number of possible characters used in the password.
    /// </summary>
    /// <remarks>
    /// The default value is null, and it's recommended to set a value for more accurate calculations.
    /// For example, you may set it to 95 if the password includes all printable ASCII characters.
    /// </remarks>
    public int? PossibleCharacters { get; set; }
}