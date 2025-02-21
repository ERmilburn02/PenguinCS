using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Player igloo likes
/// </summary>
public partial class IglooLike
{
    /// <summary>
    /// Igloo unique ID
    /// </summary>
    public int IglooId { get; set; }

    /// <summary>
    /// Liker unique ID
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// Number of likes
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Date of like
    /// </summary>
    public DateTime Date { get; set; }

    public virtual PenguinIglooRoom Igloo { get; set; }

    public virtual Penguin Player { get; set; }
}
