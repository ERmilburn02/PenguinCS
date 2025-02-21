using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Penguin igloo settings
/// </summary>
public partial class PenguinIglooRoom
{
    /// <summary>
    /// Unique igloo ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Owner penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Igloo type ID
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// Igloo flooring ID
    /// </summary>
    public int Flooring { get; set; }

    /// <summary>
    /// Igloo music ID
    /// </summary>
    public int Music { get; set; }

    public int Location { get; set; }

    /// <summary>
    /// Is igloo locked?
    /// </summary>
    public bool Locked { get; set; }

    /// <summary>
    /// Is entered in competition?
    /// </summary>
    public bool Competition { get; set; }

    public virtual Flooring FlooringNavigation { get; set; }

    public virtual ICollection<IglooFurniture> IglooFurnitures { get; set; } = new List<IglooFurniture>();

    public virtual ICollection<IglooLike> IglooLikes { get; set; } = new List<IglooLike>();

    public virtual Location LocationNavigation { get; set; }

    public virtual Penguin Penguin { get; set; }

    public virtual ICollection<Penguin> Penguins { get; set; } = new List<Penguin>();

    public virtual Igloo TypeNavigation { get; set; }
}
