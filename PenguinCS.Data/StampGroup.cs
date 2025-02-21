using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Stamp group collections
/// </summary>
public partial class StampGroup
{
    /// <summary>
    /// Unique stamp group ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of stamp group
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Parent stamp group ID
    /// </summary>
    public int? ParentId { get; set; }

    public virtual ICollection<StampGroup> InverseParent { get; set; } = new List<StampGroup>();

    public virtual StampGroup Parent { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<Stamp> Stamps { get; set; } = new List<Stamp>();
}
