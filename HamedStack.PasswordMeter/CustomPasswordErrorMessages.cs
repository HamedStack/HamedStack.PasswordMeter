namespace HamedStack.PasswordMeter;

/// <summary>
/// Represents a class that holds custom error messages for various password validation scenarios.
/// </summary>
public class CustomPasswordErrorMessages
{
    /// <summary>
    /// Gets or sets the error message for an empty password.
    /// </summary>
    public string? Empty { get; set; }

    /// <summary>
    /// Gets or sets the error message for a password that is too short.
    /// </summary>
    public string? TooShort { get; set; }

    /// <summary>
    /// Gets or sets the error message for a password that is too long.
    /// </summary>
    public string? TooLong { get; set; }

    /// <summary>
    /// Gets or sets the error message for a password that does not contain enough uppercase characters.
    /// </summary>
    public string? NotEnoughUppercase { get; set; }

    /// <summary>
    /// Gets or sets the error message for a password that does not contain enough lowercase characters.
    /// </summary>
    public string? NotEnoughLowercase { get; set; }

    /// <summary>
    /// Gets or sets the error message for a password that does not contain enough numbers.
    /// </summary>
    public string? NotEnoughNumbers { get; set; }

    /// <summary>
    /// Gets or sets the error message for a password that does not contain enough symbols.
    /// </summary>
    public string? NotEnoughSymbols { get; set; }

    /// <summary>
    /// Gets or sets the error message for a password that does not include all specified character types.
    /// </summary>
    public string? DoesNotIncludeAll { get; set; }

    /// <summary>
    /// Gets or sets the error message for a password that contains excluded characters.
    /// </summary>
    public string? ContainsExcluded { get; set; }

    /// <summary>
    /// Gets or sets the error message for a password that does not start with a specified pattern.
    /// </summary>
    public string? DoesNotStartWith { get; set; }

    /// <summary>
    /// Gets or sets the error message for a password that does not end with a specified pattern.
    /// </summary>
    public string? DoesNotEndWith { get; set; }

    /// <summary>
    /// Gets or sets the error message for a password that does not include at least one of the specified patterns.
    /// </summary>
    public string? DoesNotIncludeOneOf { get; set; }
}
