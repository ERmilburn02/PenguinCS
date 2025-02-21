using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Server waddle games
/// </summary>
public partial class RoomWaddle
{
    /// <summary>
    /// Waddle ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Room ID of waddle
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// Number of seats
    /// </summary>
    public int Seats { get; set; }

    /// <summary>
    /// Game of waddle
    /// </summary>
    public string Game { get; set; }

    public virtual Room Room { get; set; }
}
