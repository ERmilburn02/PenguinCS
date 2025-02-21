using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Redemption book answers
/// </summary>
public partial class RedemptionBookWord
{
    public int QuestionId { get; set; }

    /// <summary>
    /// Redemption book ID
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Page number inside book
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Line number of page
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    /// The nth word on the line
    /// </summary>
    public int WordNumber { get; set; }

    /// <summary>
    /// The correct word
    /// </summary>
    public string Answer { get; set; }

    public virtual RedemptionBook Book { get; set; }
}
