// ReSharper disable PossibleLossOfFraction
// ReSharper disable StringLiteralTypo
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace HamedStack.PasswordMeter;

/// <summary>
/// Class responsible for evaluating password strength based on various criteria.
/// </summary>
[SuppressMessage("GeneratedRegex", "SYSLIB1045:Convert to \'GeneratedRegexAttribute\'.")]
public class PasswordMeter
{
    private readonly string _password;
    private readonly PasswordOptions? _options;
    private List<string> _errors = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordMeter"/> class.
    /// </summary>
    /// <param name="password">The password to be evaluated.</param>
    /// <param name="options">Optional parameters for customizing password requirements.</param>
    public PasswordMeter(string password, PasswordOptions? options = default)
    {
        _password = password;
        _options = options;
    }

    /// <summary>
    /// Calculates the estimated time required to crack the password.
    /// </summary>
    /// <param name="crackTimeOption">Optional parameters for customizing the crack time calculation.</param>
    /// <returns>A <see cref="CrackTimeResult"/> object containing the calculated crack time.</returns>
    public CrackTimeResult CalculateCrackTime(CrackTimeOption? crackTimeOption = null)
    {
        crackTimeOption ??= new CrackTimeOption();
        var guessesPerSecond = crackTimeOption.GuessesPerSecond ?? Math.Pow(10, 11) * 5;
        var possibleCharacters = crackTimeOption.PossibleCharacters ?? 95;
        var passwordLength = _password.Length;
        var combinations = Math.Pow(possibleCharacters, passwordLength);

        return new CrackTimeResult
        {
            Seconds = combinations / guessesPerSecond,
            Description =
                TimeConverter.FormatTimeUnits(TimeConverter.SecondsToTimeUnits(combinations / guessesPerSecond))
        };
    }

    /// <summary>
    /// Compares the strength of the current password with a new password.
    /// </summary>
    /// <param name="newPassword">The new password for comparison.</param>
    /// <returns>A <see cref="PasswordComparison"/> object containing the comparison results.</returns>
    public PasswordComparison ComparePassword(string newPassword)
    {
        var oldScore = ComputeScore().Score;
        var newScore = new PasswordMeter(newPassword, _options).ComputeScore().Score;

        return new PasswordComparison
        {
            Difference = Math.Round((double) ((newScore - oldScore) / oldScore * 100), 2),
            DifferencePercentage = Math.Round((double) (newScore / oldScore), 2),
            OldPasswordScore = oldScore,
            NewPasswordScore = newScore
        };
    }

    /// <summary>
    /// Computes the overall score for the current password based on various criteria.
    /// </summary>
    /// <returns>A <see cref="ScoreResult"/> object containing the computed score and associated errors.</returns>
    public ScoreResult ComputeScore()
    {
        var score = 0;
        _errors = Validate().ToList();

        if (_errors.Count > 0)
        {
            return new ScoreResult
            {
                Errors = _errors,
                Score = -1
            };
        }

        // Additions
        score += NumberOfCharacters();
        score += UppercaseLetters();
        score += LowercaseLetters();
        score += Numbers();
        score += Symbols();
        score += MiddleNumbersOrSymbols();
        score += Requirements();
        score += Entropy();

        // Deductions
        score += LettersOnly();
        score += NumbersOnly();
        score += ConsecutiveUppercaseLetters();
        score += ConsecutiveLowercaseLetters();
        score += ConsecutiveNumbers();
        score += SequentialLetters();
        score += SequentialNumbers();
        score += SequentialSymbols();
        score += RepeatedCharacters();
        score += DatePatterns();
        score += KeyboardPatterns();

        return new ScoreResult
        {
            Score = score,
            Errors = _errors
        };
    }

    /// <summary>
    /// Counts the occurrences of a specified pattern in the password.
    /// </summary>
    /// <param name="pattern">The regex pattern to count.</param>
    /// <returns>The count of occurrences of the specified pattern.</returns>
    private int CountPattern(Regex pattern)
    {
        var matches = pattern.Matches(_password);
        return matches.Count;
    }

    /// <summary>
    /// Calculates the score based on the number of characters in the password.
    /// </summary>
    /// <returns>The score contribution based on the number of characters.</returns>
    private int NumberOfCharacters() => _password.Length * 4;

    /// <summary>
    /// Calculates the score based on the number of uppercase letters in the password.
    /// </summary>
    /// <returns>The score contribution based on uppercase letters.</returns>
    private int UppercaseLetters()
    {
        var n = CountPattern(new Regex("[A-Z]"));
        return (_password.Length - n) * 2;
    }

    /// <summary>
    /// Calculates the score based on the number of lowercase letters in the password.
    /// </summary>
    /// <returns>The score contribution based on lowercase letters.</returns>
    private int LowercaseLetters()
    {
        var n = CountPattern(new Regex("[a-z]"));
        return (_password.Length - n) * 2;
    }

    /// <summary>
    /// Calculates the score based on the number of numbers in the password.
    /// </summary>
    /// <returns>The score contribution based on numbers.</returns>
    private int Numbers()
    {
        var n = CountPattern(new Regex("\\d"));
        return n * 4;
    }

    /// <summary>
    /// Calculates the score based on the number of symbols in the password.
    /// </summary>
    /// <returns>The score contribution based on symbols.</returns>
    private int Symbols()
    {
        var n = CountPattern(new Regex("\\W"));
        return n * 6;
    }

    /// <summary>
    /// Calculates the score based on the presence of numbers or symbols in the middle of the password.
    /// </summary>
    /// <returns>The score contribution based on middle numbers or symbols.</returns>
    private int MiddleNumbersOrSymbols()
    {
        if (_password.Length <= 2) return 0;

        var middle = _password.Substring(1, _password.Length - 2);
        var n = Regex.Matches(middle, @"[\d\W]").Count;
        return n * 2;
    }

    /// <summary>
    /// Calculates the score based on meeting various password requirements.
    /// </summary>
    /// <returns>The score contribution based on meeting requirements.</returns>
    private int Requirements()
    {
        var conditions = new[]
        {
            _password.Length >= 8,
            CountPattern(new Regex("[A-Z]")) > 0,
            CountPattern(new Regex("[a-z]")) > 0,
            CountPattern(new Regex("\\d")) > 0,
            CountPattern(new Regex("\\W")) > 0
        };

        var validConditions = conditions.Count(c => c);
        return validConditions >= 3 ? validConditions * 2 : 0;
    }

    /// <summary>
    /// Calculates the score based on the presence of letters only in the password.
    /// </summary>
    /// <returns>The score deduction for having letters only.</returns>
    private int LettersOnly()
    {
        if (Regex.IsMatch(_password, "^[A-Za-z]+$"))
        {
            return -_password.Length;
        }

        return 0;
    }

    /// <summary>
    /// Calculates the score based on the presence of numbers only in the password.
    /// </summary>
    /// <returns>The score deduction for having numbers only.</returns>
    private int NumbersOnly()
    {
        if (Regex.IsMatch(_password, @"^\d+$"))
        {
            return -_password.Length;
        }

        return 0;
    }

    /// <summary>
    /// Calculates the score based on consecutive uppercase letters in the password.
    /// </summary>
    /// <returns>The score deduction for consecutive uppercase letters.</returns>
    private int ConsecutiveUppercaseLetters()
    {
        var n = CountPattern(new Regex("(?=[A-Z]{2})"));
        return -n * 2;
    }

    /// <summary>
    /// Calculates the score based on consecutive lowercase letters in the password.
    /// </summary>
    /// <returns>The score deduction for consecutive lowercase letters.</returns>
    private int ConsecutiveLowercaseLetters()
    {
        var n = CountPattern(new Regex("(?=[a-z]{2})"));
        return -n * 2;
    }

    /// <summary>
    /// Calculates the score based on consecutive numbers in the password.
    /// </summary>
    /// <returns>The score deduction for consecutive numbers.</returns>
    private int ConsecutiveNumbers()
    {
        var n = CountPattern(new Regex("(?=\\d{2})"));
        return -n * 2;
    }

    /// <summary>
    /// Calculates the score based on sequential letters in the password.
    /// </summary>
    /// <returns>The score deduction for sequential letters.</returns>
    private int SequentialLetters()
    {
        var n = CountPattern(
            new Regex(
                "(abc|bcd|cde|def|efg|fgh|ghi|hij|ijk|jkl|klm|lmn|mno|nop|opq|pqr|qrs|rst|stu|tuv|uvw|vwx|wxy|xyz)",
                RegexOptions.IgnoreCase)
        );
        return -n * 3;
    }

    /// <summary>
    /// Calculates the score based on sequential numbers in the password.
    /// </summary>
    /// <returns>The score deduction for sequential numbers.</returns>
    private int SequentialNumbers()
    {
        var n = CountPattern(new Regex("(012|123|234|345|456|567|678|789)"));
        return -n * 3;
    }

    /// <summary>
    /// Calculates the score based on sequential symbols in the password.
    /// </summary>
    /// <returns>The score deduction for sequential symbols.</returns>
    private int SequentialSymbols()
    {
        // Assuming a common keyboard layout for symbols
        var n = CountPattern(new Regex(
            "(!@#|@#\\$|\\$%|\\%^|\\^&|&\\*|\\*\\(|\\(\\))",
            RegexOptions.IgnoreCase)
        );
        return -n * 3;
    }

    /// <summary>
    /// Calculates the score based on repeated characters in the password.
    /// </summary>
    /// <returns>The score deduction for repeated characters.</returns>
    private int RepeatedCharacters()
    {
        var charMap = new Dictionary<char, int>();
        foreach (var character in _password.ToLower())
        {
            if (charMap.ContainsKey(character))
            {
                charMap[character]++;
            }
            else
            {
                charMap[character] = 1;
            }
        }

        var penalty = charMap.Where(pair => pair.Value > 1).Sum(pair => (int) Math.Pow(pair.Value, 2));
        return -penalty;
    }

    /// <summary>
    /// Calculates the score based on the entropy of the password.
    /// </summary>
    /// <returns>The score contribution based on entropy.</returns>
    private int Entropy()
    {
        var uniqueChars = _password.Distinct().Count();
        var entropy = (int) Math.Round(_password.Length * Math.Log2(uniqueChars));
        return entropy;
    }

    /// <summary>
    /// Calculates the score based on date-related patterns in the password.
    /// </summary>
    /// <returns>The score deduction for date-related patterns.</returns>
    private int DatePatterns()
    {
        // DDMMYYYY, MMDDYYYY, YYYYMMDD
        var n1 = CountPattern(new Regex(
            @"(0[1-9]|[12][0-9]|3[01])(0[1-9]|1[0-2])\d{4}|(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])\d{4}|\d{4}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])"
        ));
        // DDMMYY, MMDDYY, YYMMDD
        var n2 = CountPattern(new Regex(
            @"(0[1-9]|[12][0-9]|3[01])(0[1-9]|1[0-2])\d{2}|(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])\d{2}|\d{2}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])"
        ));
        var total = n1 + n2;
        if (total > 0)
        {
            return -total * 5;
        }

        return 0;
    }

    /// <summary>
    /// Calculates the score based on keyboard-related patterns in the password.
    /// </summary>
    /// <returns>The score deduction for keyboard-related patterns.</returns>
    private int KeyboardPatterns()
    {
        var patterns = new[]
        {
            "1234567890",
            "234567890",
            "34567890",
            "4567890",
            "567890",
            "67890",
            "7890",
            "890",
            "0987654321",
            "987654321",
            "87654321",
            "7654321",
            "654321",
            "54321",
            "4321",
            "321",
            "qwertyuiop",
            "wertyuiop",
            "ertyuiop",
            "rtyuiop",
            "tyuiop",
            "yuiop",
            "uiop",
            "iop",
            "op",
            "poiuytrewq",
            "oiuytrewq",
            "iuytrewq",
            "uytrewq",
            "ytrewq",
            "trewq",
            "rewq",
            "ewq",
            "wq",
            "asdfghjkl",
            "sdfghjkl",
            "dfghjkl",
            "fghjkl",
            "ghjkl",
            "hjkl",
            "jkl",
            "lkjhgfdsa",
            "kjhgfdsa",
            "jhgfdsa",
            "hgfdsa",
            "gfdsa",
            "fdsa",
            "dsa",
            "zxcvbnm",
            "xcvbnm",
            "cvbnm",
            "vbnm",
            "bnm",
            "mnbvcxz",
            "nbvcxz",
            "bvcxz",
            "vcxz",
            "cxz",
            "1qaz",
            "qazwsx",
            "azwsxedc",
            "2wsx",
            "wsxedc",
            "sedcrfv",
            "edcrfvtg",
            "dcfvgb",
            "3edc",
            "rfvtgb",
            "fvtgbyhn",
            "vtgbyhnujm",
            "4rfv",
            "fvgb",
            "gbhn",
            "bhnj",
            "hnjm",
            "5tgb",
            "6yhn",
            "7ujm",
            ";/p0",
            ".lo9",
            ",ki8",
            "mj7",
            "nh6",
            "bg5",
            "vf4",
            "cd3",
            "xe2",
            "za1",
            "123456",
            "234567",
            "345678",
            "456789",
            "098765",
            "987654",
            "876543",
            "765432",
            "543210",
            "qwerty",
            "wertyu",
            "ertyui",
            "rtyuio",
            "poiuyt",
            "oiuytr",
            "iuytre",
            "uytrew",
            "asdfgh",
            "sdfghj",
            "dfghjk",
            "lkjhgf",
            "kjhgsd",
            "jhgfsa",
            "hgfasd",
            "zxcvbn",
            "mnbvcx",
            "qwer",
            "asdf",
            "zxcv",
            "poiuy"
        };

        var count = 0;
        foreach (var pattern in patterns)
        {
            var reg = new Regex(pattern, RegexOptions.IgnoreCase);
            if (reg.IsMatch(_password))
            {
                count++;
            }

            var reversePattern = new string(pattern.Reverse().ToArray());
            var reverseReg = new Regex(reversePattern, RegexOptions.IgnoreCase);
            if (reverseReg.IsMatch(_password))
            {
                count++;
            }
        }

        if (count > 0)
        {
            return -count * 5;
        }

        return 0;
    }

    /// <summary>
    /// Validates the password based on specified criteria and returns a list of errors.
    /// </summary>
    /// <returns>A list of error messages if the password is invalid; otherwise, an empty list.</returns>
    private IEnumerable<string> Validate()
    {
        _errors = new List<string>();
        var messages = _options?.CustomErrorMessages ?? new CustomPasswordErrorMessages();

        if (_password.Length == 0)
        {
            _errors.Add(messages.Empty ?? "Password is empty.");
        }

        if (_options is {MinLength: not null} && _password.Length < _options.MinLength.Value)
        {
            _errors.Add(messages.TooShort ?? "Password is too short.");
        }

        if (_options is {MaxLength: not null} && _password.Length > _options.MaxLength.Value)
        {
            _errors.Add(messages.TooLong ?? "Password is too long.");
        }

        if (_options is {UppercaseLettersMinLength: not null} &&
            CountPattern(new Regex("[A-Z]")) < _options.UppercaseLettersMinLength.Value)
        {
            _errors.Add(messages.NotEnoughUppercase ?? "Not enough uppercase letters.");
        }

        if (_options is {LowercaseLettersMinLength: not null} &&
            CountPattern(new Regex("[a-z]")) < _options.LowercaseLettersMinLength.Value)
        {
            _errors.Add(messages.NotEnoughLowercase ?? "Not enough lowercase letters.");
        }

        if (_options is {NumbersMinLength: not null} &&
            CountPattern(new Regex("\\d")) < _options.NumbersMinLength.Value)
        {
            _errors.Add(messages.NotEnoughNumbers ?? "Not enough numbers.");
        }

        if (_options is {SymbolsMinLength: not null} &&
            CountPattern(new Regex("\\W")) < _options.SymbolsMinLength.Value)
        {
            _errors.Add(messages.NotEnoughSymbols ?? "Not enough symbols.");
        }

        if (_options?.Include != null && _options.Include.Any(item => !_password.Contains(item)))
        {
            _errors.Add(messages.DoesNotIncludeAll ?? "Password must include all specified characters.");
        }

        if (_options?.Exclude != null && _options.Exclude.Any(item => _password.Contains(item)))
        {
            _errors.Add(messages.ContainsExcluded ?? "Password contains excluded characters.");
        }

        if (_options?.StartsWith != null && !_password.StartsWith(_options.StartsWith))
        {
            _errors.Add(messages.DoesNotStartWith ?? "Password does not start with the specified character.");
        }

        if (_options?.EndsWith != null && !_password.EndsWith(_options.EndsWith))
        {
            _errors.Add(messages.DoesNotEndWith ?? "Password does not end with the specified character.");
        }

        if (_options?.IncludeOne != null && !_options.IncludeOne.Any(item => _password.Contains(item)))
        {
            _errors.Add(messages.DoesNotIncludeOneOf ??
                        "Password must contain at least one of the specified characters.");
        }

        return _errors;
    }

    /// <summary>
    /// Determines the password strength based on the computed score and predefined strength scores.
    /// </summary>
    /// <param name="score">The computed score for the password.</param>
    /// <param name="strengthScores">An array of predefined strength scores.</param>
    /// <returns>The determined password strength.</returns>
    public PasswordStrength GetStrength(int score, PasswordStrengthScore[] strengthScores)
    {
        if (!IsValidPasswordStrengthScore(strengthScores))
        {
            throw new Exception("");
        }

        if (score < 0)
        {
            return PasswordStrength.Invalid;
        }

        var enums = Enum.GetValues(typeof(PasswordStrength)).Cast<PasswordStrength>();
        foreach (var e in enums)
        {
            var val = strengthScores.FirstOrDefault(x => x.PasswordStrength == e && e != PasswordStrength.Invalid);
            if (val == null) continue;

            var rank = val.MaxScore;
            if (score < rank)
            {
                return e;
            }

            if (score == rank)
            {
                return e;
            }
        }

        return PasswordStrength.Perfect;
    }

    /// <summary>
    /// Validates the integrity of the provided password strength scores.
    /// </summary>
    /// <param name="strengthScores">An array of predefined strength scores.</param>
    /// <returns>True if the password strength scores are valid; otherwise, false.</returns>
    private static bool IsValidPasswordStrengthScore(IReadOnlyCollection<PasswordStrengthScore> strengthScores)
    {
        if (strengthScores.Select(x => x.PasswordStrength).Distinct().Count() != strengthScores.Count)
        {
            return false;
        }

        var enums = Enum.GetValues(typeof(PasswordStrength)).Cast<PasswordStrength>();
        var value = 0;
        foreach (var e in enums)
        {
            var val = strengthScores.FirstOrDefault(x => x.PasswordStrength == e);
            if (val == null && e != PasswordStrength.Invalid)
            {
                return false;
            }

            if (value > val?.MaxScore)
            {
                return false;
            }

            if (val != null && e != PasswordStrength.Invalid)
            {
                value = val.MaxScore;
            }
        }

        return true;
    }
}