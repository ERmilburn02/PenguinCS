using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Server flooring crumbs
/// </summary>
public partial class Flooring
{
    /// <summary>
    /// Unique flooring ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Flooring name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Cost of flooring
    /// </summary>
    public int Cost { get; set; }

    /// <summary>
    /// Is flooring patched?
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

    public virtual ICollection<PenguinIglooRoom> PenguinIglooRooms { get; set; } = new List<PenguinIglooRoom>();

    public virtual ICollection<RedemptionCode> Codes { get; set; } = new List<RedemptionCode>();

    public virtual ICollection<Penguin> Penguins { get; set; } = new List<Penguin>();
}
