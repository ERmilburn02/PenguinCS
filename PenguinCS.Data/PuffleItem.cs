using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Server puffle care item crumbs
/// </summary>
public partial class PuffleItem
{
    /// <summary>
    /// Unique care item ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Parent care item ID
    /// </summary>
    public int ParentId { get; set; }

    /// <summary>
    /// Care item name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Type of care item
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// External play mode
    /// </summary>
    public string PlayExternal { get; set; }

    /// <summary>
    /// Cost of care item
    /// </summary>
    public int Cost { get; set; }

    /// <summary>
    /// Base quantity of purchase
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Is member-only?
    /// </summary>
    public bool Member { get; set; }

    /// <summary>
    /// Effect on puffle food level
    /// </summary>
    public int FoodEffect { get; set; }

    /// <summary>
    /// Effect on puffle rest level
    /// </summary>
    public int RestEffect { get; set; }

    /// <summary>
    /// Effect on puffle play level
    /// </summary>
    public int PlayEffect { get; set; }

    /// <summary>
    /// Effect on puffle clean level
    /// </summary>
    public int CleanEffect { get; set; }

    public virtual ICollection<PuffleItem> InverseParent { get; set; } = new List<PuffleItem>();

    public virtual PuffleItem Parent { get; set; }

    public virtual ICollection<PenguinPuffleItem> PenguinPuffleItems { get; set; } = new List<PenguinPuffleItem>();

    public virtual ICollection<PenguinPuffle> PenguinPuffles { get; set; } = new List<PenguinPuffle>();

    public virtual ICollection<Puffle> PuffleFavouriteFoodNavigations { get; set; } = new List<Puffle>();

    public virtual ICollection<Puffle> PuffleFavouriteToyNavigations { get; set; } = new List<Puffle>();

    public virtual ICollection<QuestAwardPuffleItem> QuestAwardPuffleItems { get; set; } = new List<QuestAwardPuffleItem>();

    public virtual ICollection<RedemptionCode> Codes { get; set; } = new List<RedemptionCode>();

    public virtual ICollection<Puffle> Puffles { get; set; } = new List<Puffle>();
}
