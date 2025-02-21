using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Penguin moderator warnings
/// </summary>
public partial class Warning
{
    /// <summary>
    /// Warning penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Warning issue date
    /// </summary>
    public DateTime Issued { get; set; }

    /// <summary>
    /// Warning expiry date
    /// </summary>
    public DateTime Expires { get; set; }

    public virtual Penguin Penguin { get; set; }
}
