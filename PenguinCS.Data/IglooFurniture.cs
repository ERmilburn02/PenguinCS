using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Furniture placed inside igloos
/// </summary>
public partial class IglooFurniture
{
    /// <summary>
    /// Furniture igloo ID
    /// </summary>
    public int IglooId { get; set; }

    /// <summary>
    /// Furniture item ID
    /// </summary>
    public int FurnitureId { get; set; }

    /// <summary>
    /// Igloo X position
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Igloo Y position
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// Furniture frame ID
    /// </summary>
    public int Frame { get; set; }

    /// <summary>
    /// Furniture rotation ID
    /// </summary>
    public int Rotation { get; set; }

    public virtual Furniture Furniture { get; set; }

    public virtual PenguinIglooRoom Igloo { get; set; }
}
