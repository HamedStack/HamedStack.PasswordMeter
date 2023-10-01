namespace HamedStack.PasswordMeter;

/// <summary>
/// Represents the result of a password cracking attempt, including the time required and a description.
/// </summary>
public class CrackTimeResult
{
    /// <summary>
    /// Gets or sets the time, in seconds, required to crack the password.
    /// </summary>
    public double Seconds { get; set; }

    /// <summary>
    /// Gets or sets a description providing additional information about the cracking attempt.
    /// </summary>
    public string Description { get; set; } = null!;
}
