using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Server table games
/// </summary>
public partial class RoomTable
{
    /// <summary>
    /// Table ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Room ID of table
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// Game of table
    /// </summary>
    public string Game { get; set; }

    public virtual Room Room { get; set; }
}
