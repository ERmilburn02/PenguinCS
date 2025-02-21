using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Penguin membership records
/// </summary>
public partial class PenguinMembership
{
    /// <summary>
    /// Penguin ID of membership
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Start time of membership
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// End time of membership
    /// </summary>
    public DateTime? Expires { get; set; }

    public bool? StartAware { get; set; }

    public bool? ExpiresAware { get; set; }

    public bool? ExpiredAware { get; set; }

    public virtual Penguin Penguin { get; set; }
}
