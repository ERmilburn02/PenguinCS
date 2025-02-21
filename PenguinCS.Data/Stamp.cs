using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Server stamp crumbs
/// </summary>
public partial class Stamp
{
    /// <summary>
    /// Unique stamp ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Stamp name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Stamp group ID
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Is member-only?
    /// </summary>
    public bool Member { get; set; }

    /// <summary>
    /// Stamp difficulty ranking
    /// </summary>
    public int Rank { get; set; }

    /// <summary>
    /// Stamp description
    /// </summary>
    public string Description { get; set; }

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    public virtual ICollection<CoverStamp> CoverStamps { get; set; } = new List<CoverStamp>();

    public virtual StampGroup Group { get; set; }

    public virtual ICollection<PenguinStamp> PenguinStamps { get; set; } = new List<PenguinStamp>();
}
