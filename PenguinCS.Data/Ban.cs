using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Penguin ban records
/// </summary>
public partial class Ban
{
    /// <summary>
    /// Banned penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Issue date
    /// </summary>
    public DateTime Issued { get; set; }

    /// <summary>
    /// Expiry date
    /// </summary>
    public DateTime Expires { get; set; }

    /// <summary>
    /// Moderator penguin ID
    /// </summary>
    public int? ModeratorId { get; set; }

    /// <summary>
    /// Ban reason
    /// </summary>
    public int Reason { get; set; }

    /// <summary>
    /// Ban comment
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// Banned for message
    /// </summary>
    public string Message { get; set; }

    public virtual Penguin Moderator { get; set; }

    public virtual Penguin Penguin { get; set; }
}
