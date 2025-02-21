using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Penguin owned furniture
/// </summary>
public partial class PenguinFurniture
{
    /// <summary>
    /// Owner penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Furniture item ID
    /// </summary>
    public int FurnitureId { get; set; }

    /// <summary>
    /// Quantity owned
    /// </summary>
    public int Quantity { get; set; }

    public virtual Furniture Furniture { get; set; }

    public virtual Penguin Penguin { get; set; }
}
