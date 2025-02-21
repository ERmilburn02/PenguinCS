using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Jitsu card starter decks
/// </summary>
public partial class CardStarterDeck
{
    /// <summary>
    /// Starter deck item ID
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// Starter deck card ID
    /// </summary>
    public int CardId { get; set; }

    /// <summary>
    /// Card quantity
    /// </summary>
    public int Quantity { get; set; }

    public virtual Card Card { get; set; }

    public virtual Item Item { get; set; }
}
