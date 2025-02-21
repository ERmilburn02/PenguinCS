using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Server furniture crumbs
/// </summary>
public partial class Furniture
{
    /// <summary>
    /// Unique furniture ID
    /// </summary>
    public int Id { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// Furniture type ID
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// Furniture sort ID
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// Cost of furniture
    /// </summary>
    public int Cost { get; set; }

    /// <summary>
    /// Is member-only?
    /// </summary>
    public bool Member { get; set; }

    /// <summary>
    /// Is furniture patched?
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
    /// Is furniture bait?
    /// </summary>
    public bool Bait { get; set; }

    /// <summary>
    /// Max inventory quantity
    /// </summary>
    public int MaxQuantity { get; set; }

    /// <summary>
    /// Is furniture innocent?
    /// </summary>
    public bool Innocent { get; set; }

    public virtual ICollection<IglooFurniture> IglooFurnitures { get; set; } = new List<IglooFurniture>();

    public virtual ICollection<PenguinFurniture> PenguinFurnitures { get; set; } = new List<PenguinFurniture>();

    public virtual ICollection<QuestAwardFurniture> QuestAwardFurnitures { get; set; } = new List<QuestAwardFurniture>();

    public virtual ICollection<RedemptionCode> Codes { get; set; } = new List<RedemptionCode>();

    public virtual ICollection<Puffle> Puffles { get; set; } = new List<Puffle>();
}
