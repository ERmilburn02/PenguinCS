using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Items placed on book cover
/// </summary>
public partial class CoverItem
{
    /// <summary>
    /// Unique penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Cover item ID
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// Cover X position
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Cover Y position
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// Stamp cover rotation
    /// </summary>
    public int Rotation { get; set; }

    /// <summary>
    /// Stamp cover depth
    /// </summary>
    public int Depth { get; set; }

    public virtual Item Item { get; set; }

    public virtual Penguin Penguin { get; set; }
}
