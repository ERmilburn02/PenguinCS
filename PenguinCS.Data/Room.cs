using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Server room crumbs
/// </summary>
public partial class Room
{
    /// <summary>
    /// Unique room ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Internal room key
    /// </summary>
    public int InternalId { get; set; }

    /// <summary>
    /// Room name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Is member-only?
    /// </summary>
    public bool Member { get; set; }

    /// <summary>
    /// Maximum room users
    /// </summary>
    public int MaxUsers { get; set; }

    /// <summary>
    /// Required inventory item
    /// </summary>
    public int? RequiredItem { get; set; }

    /// <summary>
    /// Is game room?
    /// </summary>
    public bool Game { get; set; }

    /// <summary>
    /// Is blackhole game room?
    /// </summary>
    public bool Blackhole { get; set; }

    /// <summary>
    /// Is spawn room?
    /// </summary>
    public bool Spawn { get; set; }

    public int? StampGroup { get; set; }

    public virtual ICollection<PenguinGameDatum> PenguinGameData { get; set; } = new List<PenguinGameDatum>();

    public virtual ICollection<QuestTask> QuestTasks { get; set; } = new List<QuestTask>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual Item RequiredItemNavigation { get; set; }

    public virtual ICollection<RoomTable> RoomTables { get; set; } = new List<RoomTable>();

    public virtual ICollection<RoomWaddle> RoomWaddles { get; set; } = new List<RoomWaddle>();

    public virtual StampGroup StampGroupNavigation { get; set; }
}
