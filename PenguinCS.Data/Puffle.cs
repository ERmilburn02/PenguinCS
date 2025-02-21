using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Server puffle crumbs
/// </summary>
public partial class Puffle
{
    /// <summary>
    /// Unique puffle ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Base color puffle ID
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// Puffle name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Puffle cost
    /// </summary>
    public int Cost { get; set; }

    /// <summary>
    /// Is member-only?
    /// </summary>
    public bool Member { get; set; }

    /// <summary>
    /// Favourite puffle-care item
    /// </summary>
    public int FavouriteFood { get; set; }

    public int? FavouriteToy { get; set; }

    /// <summary>
    /// Runaway postcard ID
    /// </summary>
    public int? RunawayPostcard { get; set; }

    public virtual PuffleItem FavouriteFoodNavigation { get; set; }

    public virtual PuffleItem FavouriteToyNavigation { get; set; }

    public virtual ICollection<Puffle> InverseParent { get; set; } = new List<Puffle>();

    public virtual Puffle Parent { get; set; }

    public virtual ICollection<PenguinPuffle> PenguinPuffles { get; set; } = new List<PenguinPuffle>();

    public virtual Postcard RunawayPostcardNavigation { get; set; }

    public virtual ICollection<RedemptionCode> Codes { get; set; } = new List<RedemptionCode>();

    public virtual ICollection<Furniture> Furnitures { get; set; } = new List<Furniture>();

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual ICollection<PuffleItem> PuffleItems { get; set; } = new List<PuffleItem>();
}
