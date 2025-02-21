using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Server character crumbs
/// </summary>
public partial class Character
{
    /// <summary>
    /// Unique character ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Character name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Character gift item ID
    /// </summary>
    public int? GiftId { get; set; }

    /// <summary>
    /// Character stamp ID
    /// </summary>
    public int? StampId { get; set; }

    public virtual ICollection<CharacterBuddy> CharacterBuddies { get; set; } = new List<CharacterBuddy>();

    public virtual Item Gift { get; set; }

    public virtual ICollection<Penguin> Penguins { get; set; } = new List<Penguin>();

    public virtual Stamp Stamp { get; set; }
}
