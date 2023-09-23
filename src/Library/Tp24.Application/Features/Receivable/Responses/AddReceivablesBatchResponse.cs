namespace Tp24.Application.Features.Receivable.Responses;

/// <summary>
///     Represents the response after processing a batch of receivables.
///     It provides information on the total processed, successes, duplicates, and existing entries.
/// </summary>
public class AddReceivablesBatchResponse
{
    /// <summary>
    ///     Gets or sets the total number of items processed.
    /// </summary>
    public int TotalProcessed { get; set; }

    /// <summary>
    ///     Gets or sets the number of items processed successfully.
    /// </summary>
    public int SuccessCount { get; set; }

    /// <summary>
    ///     Gets or sets the count of duplicate items detected in the batch.
    /// </summary>
    public int DuplicatesCount { get; set; }

    /// <summary>
    ///     Gets or sets the count of items that already exist in the database.
    /// </summary>
    public int ExistingCount { get; set; }
}