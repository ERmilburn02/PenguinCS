using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Adopted puffles
/// </summary>
public partial class PenguinPuffle
{
    /// <summary>
    /// Unique puffle ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Owner penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Puffle type ID
    /// </summary>
    public int PuffleId { get; set; }

    /// <summary>
    /// Puffle name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Date of adoption
    /// </summary>
    public DateTime AdoptionDate { get; set; }

    /// <summary>
    /// Puffle health %
    /// </summary>
    public int Food { get; set; }

    /// <summary>
    /// Puffle hunger %
    /// </summary>
    public int Play { get; set; }

    /// <summary>
    /// Puffle rest %
    /// </summary>
    public int Rest { get; set; }

    /// <summary>
    /// Puffle clean %
    /// </summary>
    public int Clean { get; set; }

    /// <summary>
    /// Puffle hat item ID
    /// </summary>
    public int? Hat { get; set; }

    /// <summary>
    /// Is in backyard?
    /// </summary>
    public bool? Backyard { get; set; }

    /// <summary>
    /// Has dug?
    /// </summary>
    public bool? HasDug { get; set; }

    public virtual PuffleItem HatNavigation { get; set; }

    public virtual Penguin Penguin { get; set; }

    public virtual ICollection<Penguin> Penguins { get; set; } = new List<Penguin>();

    public virtual Puffle Puffle { get; set; }
}
