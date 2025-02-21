using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Stamps placed on book cover
/// </summary>
public partial class CoverStamp
{
    /// <summary>
    /// Unique penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Cover stamp ID
    /// </summary>
    public int StampId { get; set; }

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

    public virtual Penguin Penguin { get; set; }

    public virtual Stamp Stamp { get; set; }
}
