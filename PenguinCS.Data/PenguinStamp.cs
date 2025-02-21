using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Penguin earned stamps
/// </summary>
public partial class PenguinStamp
{
    /// <summary>
    /// Stamp penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Stamp ID
    /// </summary>
    public int StampId { get; set; }

    /// <summary>
    /// Is recently earned?
    /// </summary>
    public bool Recent { get; set; }

    public virtual Penguin Penguin { get; set; }

    public virtual Stamp Stamp { get; set; }
}
