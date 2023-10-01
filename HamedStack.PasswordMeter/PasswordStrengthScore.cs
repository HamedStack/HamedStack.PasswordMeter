// ReSharper disable ClassNeverInstantiated.Global

namespace HamedStack.PasswordMeter;

/// <summary>
/// Represents the result of scoring a password, including its strength level and the maximum achievable score.
/// </summary>
public class PasswordStrengthScore
{
    /// <summary>
    /// Gets the strength level of the scored password.
    /// </summary>
    /// <remarks>
    /// This property indicates the overall strength of the password based on a scoring algorithm.
    /// </remarks>
    public PasswordStrength PasswordStrength { get; init; }

    /// <summary>
    /// Gets the maximum achievable score for a password.
    /// </summary>
    /// <remarks>
    /// The maximum score represents the highest possible strength level for a password based on the scoring algorithm.
    /// </remarks>
    public int MaxScore { get; init; }
}
