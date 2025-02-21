using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Penguin login records
/// </summary>
public partial class Login
{
    /// <summary>
    /// Unique login ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Login penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Login date
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Connection IP address
    /// </summary>
    public string IpHash { get; set; }

    /// <summary>
    /// Minutes played for session
    /// </summary>
    public int MinutesPlayed { get; set; }

    public virtual Penguin Penguin { get; set; }
}
