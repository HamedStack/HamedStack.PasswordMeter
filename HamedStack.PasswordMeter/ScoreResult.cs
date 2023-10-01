namespace HamedStack.PasswordMeter;

/// <summary>
/// Represents the result of scoring an entity, including a numeric score and a list of associated errors.
/// </summary>
public class ScoreResult
{
    /// <summary>
    /// Gets the numeric score assigned to the entity.
    /// </summary>
    /// <remarks>
    /// This property represents the evaluation or quality score given to the entity based on a scoring algorithm.
    /// </remarks>
    public int Score { get; init; }

    /// <summary>
    /// Gets or sets a list of errors associated with the scoring process.
    /// </summary>
    /// <remarks>
    /// If present, this list provides information about any issues or discrepancies found during the scoring.
    /// </remarks>
    public List<string>? Errors { get; set; }
}
