// ReSharper disable ClassNeverInstantiated.Global

namespace HamedStack.PasswordMeter;

/// <summary>
/// Represents the options for generating or validating passwords based on various criteria.
/// </summary>
public class PasswordOptions
{
    /// <summary>
    /// Gets or sets the minimum length requirement for the password.
    /// </summary>
    public int? MinLength { get; set; }

    /// <summary>
    /// Gets or sets the maximum length requirement for the password.
    /// </summary>
    public int? MaxLength { get; set; }

    /// <summary>
    /// Gets or sets the minimum number of uppercase letters required in the password.
    /// </summary>
    public int? UppercaseLettersMinLength { get; set; }

    /// <summary>
    /// Gets or sets the minimum number of lowercase letters required in the password.
    /// </summary>
    public int? LowercaseLettersMinLength { get; set; }

    /// <summary>
    /// Gets or sets the minimum number of numeric digits required in the password.
    /// </summary>
    public int? NumbersMinLength { get; set; }

    /// <summary>
    /// Gets or sets the minimum number of symbols required in the password.
    /// </summary>
    public int? SymbolsMinLength { get; set; }

    /// <summary>
    /// Gets or sets an array of characters that must be included in the password.
    /// </summary>
    public string[]? Include { get; set; }

    /// <summary>
    /// Gets or sets an array of characters that must be excluded from the password.
    /// </summary>
    public string[]? Exclude { get; set; }

    /// <summary>
    /// Gets or sets a string that the password must start with.
    /// </summary>
    public string? StartsWith { get; set; }

    /// <summary>
    /// Gets or sets a string that the password must end with.
    /// </summary>
    public string? EndsWith { get; set; }

    /// <summary>
    /// Gets or sets an array of strings, of which at least one must be included in the password.
    /// </summary>
    public string[]? IncludeOne { get; set; }

    /// <summary>
    /// Gets or sets custom error messages for various password validation scenarios.
    /// </summary>
    public CustomPasswordErrorMessages? CustomErrorMessages { get; set; }
}
