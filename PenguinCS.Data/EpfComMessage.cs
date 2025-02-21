using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// EPF phone com messages
/// </summary>
public partial class EpfComMessage
{
    /// <summary>
    /// Message content
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Character ID of message
    /// </summary>
    public int CharacterId { get; set; }

    /// <summary>
    /// Date of message creation
    /// </summary>
    public DateTime Date { get; set; }

    public virtual Character Character { get; set; }
}
