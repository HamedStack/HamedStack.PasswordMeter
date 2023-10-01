namespace HamedStack.PasswordMeter;

/// <summary>
/// Represents the result of comparing two passwords, including the difference in scores and percentages.
/// </summary>
public class PasswordComparison
{
    /// <summary>
    /// Gets or sets the absolute difference in scores between the old and new passwords.
    /// </summary>
    /// <remarks>
    /// A positive value indicates an improvement in the new password, while a negative value indicates a decline.
    /// </remarks>
    public double Difference { get; set; }

    /// <summary>
    /// Gets or sets the percentage difference in scores between the old and new passwords.
    /// </summary>
    /// <remarks>
    /// The percentage is calculated based on the old password score.
    /// A positive percentage indicates an improvement, while a negative percentage indicates a decline.
    /// </remarks>
    public double DifferencePercentage { get; set; }

    /// <summary>
    /// Gets or sets the score of the old password based on a scoring algorithm.
    /// </summary>
    /// <remarks>
    /// The score represents the strength or quality of the old password.
    /// </remarks>
    public int OldPasswordScore { get; set; }

    /// <summary>
    /// Gets or sets the score of the new password based on a scoring algorithm.
    /// </summary>
    /// <remarks>
    /// The score represents the strength or quality of the new password.
    /// </remarks>
    public int NewPasswordScore { get; set; }
}
