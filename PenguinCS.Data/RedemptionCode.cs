using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Redemption codes
/// </summary>
public partial class RedemptionCode
{
    /// <summary>
    /// Unique code ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Redemption code
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Code type
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Code coins amount
    /// </summary>
    public int Coins { get; set; }

    /// <summary>
    /// Expiry date
    /// </summary>
    public DateTime? Expires { get; set; }

    /// <summary>
    /// Number of uses
    /// </summary>
    public int? Uses { get; set; }

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();

    public virtual ICollection<Flooring> Floorings { get; set; } = new List<Flooring>();

    public virtual ICollection<Furniture> Furnitures { get; set; } = new List<Furniture>();

    public virtual ICollection<Igloo> Igloos { get; set; } = new List<Igloo>();

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual ICollection<Penguin> Penguins { get; set; } = new List<Penguin>();

    public virtual ICollection<PuffleItem> PuffleItems { get; set; } = new List<PuffleItem>();

    public virtual ICollection<Puffle> Puffles { get; set; } = new List<Puffle>();
}
