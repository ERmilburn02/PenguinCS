using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Penguin Card Jitsu decks
/// </summary>
public partial class PenguinCard
{
    /// <summary>
    /// Owner penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Card type ID
    /// </summary>
    public int CardId { get; set; }

    /// <summary>
    /// Quantity owned
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Quantity owned as member
    /// </summary>
    public int MemberQuantity { get; set; }

    public virtual Card Card { get; set; }

    public virtual Penguin Penguin { get; set; }
}
