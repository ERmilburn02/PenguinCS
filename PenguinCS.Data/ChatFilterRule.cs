using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

public partial class ChatFilterRule
{
    /// <summary>
    /// Word to filter
    /// </summary>
    public string Word { get; set; }

    /// <summary>
    /// Hide word from players
    /// </summary>
    public bool Filter { get; set; }

    /// <summary>
    /// Warn player for word
    /// </summary>
    public bool Warn { get; set; }

    /// <summary>
    /// Ban player for word
    /// </summary>
    public bool Ban { get; set; }
}
