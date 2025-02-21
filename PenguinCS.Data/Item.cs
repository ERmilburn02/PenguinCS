using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Server item crumbs
/// </summary>
public partial class Item
{
    /// <summary>
    /// Unique item ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Item name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Item clothing type
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// Cost of item
    /// </summary>
    public int Cost { get; set; }

    /// <summary>
    /// Is member-only?
    /// </summary>
    public bool Member { get; set; }

    /// <summary>
    /// Is bait item?
    /// </summary>
    public bool Bait { get; set; }

    /// <summary>
    /// Is item patched?
    /// </summary>
    public bool Patched { get; set; }

    /// <summary>
    /// Add to default legacy inventory?
    /// </summary>
    public bool LegacyInventory { get; set; }

    /// <summary>
    /// Add to default vanilla inventory?
    /// </summary>
    public bool VanillaInventory { get; set; }

    /// <summary>
    /// Is EPF item?
    /// </summary>
    public bool Epf { get; set; }

    /// <summary>
    /// Gives tour status?
    /// </summary>
    public bool Tour { get; set; }

    /// <summary>
    /// Release date of item
    /// </summary>
    public DateOnly ReleaseDate { get; set; }

    /// <summary>
    /// Is a treasure item?
    /// </summary>
    public bool Treasure { get; set; }

    /// <summary>
    /// Is a innocent item?
    /// </summary>
    public bool Innocent { get; set; }

    public virtual ICollection<CardStarterDeck> CardStarterDecks { get; set; } = new List<CardStarterDeck>();

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    public virtual ICollection<CoverItem> CoverItems { get; set; } = new List<CoverItem>();

    public virtual ICollection<Penguin> PenguinBodyNavigations { get; set; } = new List<Penguin>();

    public virtual ICollection<Penguin> PenguinColorNavigations { get; set; } = new List<Penguin>();

    public virtual ICollection<Penguin> PenguinFaceNavigations { get; set; } = new List<Penguin>();

    public virtual ICollection<Penguin> PenguinFeetNavigations { get; set; } = new List<Penguin>();

    public virtual ICollection<Penguin> PenguinFlagNavigations { get; set; } = new List<Penguin>();

    public virtual ICollection<Penguin> PenguinHandNavigations { get; set; } = new List<Penguin>();

    public virtual ICollection<Penguin> PenguinHeadNavigations { get; set; } = new List<Penguin>();

    public virtual ICollection<Penguin> PenguinNeckNavigations { get; set; } = new List<Penguin>();

    public virtual ICollection<Penguin> PenguinPhotoNavigations { get; set; } = new List<Penguin>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<RedemptionCode> Codes { get; set; } = new List<RedemptionCode>();

    public virtual ICollection<Penguin> Penguins { get; set; } = new List<Penguin>();

    public virtual ICollection<Puffle> Puffles { get; set; } = new List<Puffle>();

    public virtual ICollection<Quest> Quests { get; set; } = new List<Quest>();
}
