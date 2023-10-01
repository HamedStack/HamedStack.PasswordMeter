// ReSharper disable UnusedMember.Global

namespace HamedStack.PasswordMeter;

/// <summary>
/// Represents the strength levels of a password based on a scoring algorithm.
/// </summary>
public enum PasswordStrength
{
    /// <summary>
    /// Indicates that the password strength is invalid or could not be determined.
    /// </summary>
    Invalid,

    /// <summary>
    /// Indicates very weak password strength.
    /// </summary>
    VeryWeak,

    /// <summary>
    /// Indicates weak password strength.
    /// </summary>
    Weak,

    /// <summary>
    /// Indicates good password strength.
    /// </summary>
    Good,

    /// <summary>
    /// Indicates strong password strength.
    /// </summary>
    Strong,

    /// <summary>
    /// Indicates very strong password strength.
    /// </summary>
    VeryStrong,

    /// <summary>
    /// Indicates a perfect, highly secure password strength.
    /// </summary>
    Perfect
}
