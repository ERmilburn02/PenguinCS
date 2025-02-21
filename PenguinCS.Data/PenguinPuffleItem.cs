using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Owned puffle care items
/// </summary>
public partial class PenguinPuffleItem
{
    /// <summary>
    /// Owner penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Puffle care item ID
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// Quantity owned
    /// </summary>
    public int Quantity { get; set; }

    public virtual PuffleItem Item { get; set; }

    public virtual Penguin Penguin { get; set; }
}
