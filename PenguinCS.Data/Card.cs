using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Server jitsu card crumbs
/// </summary>
public partial class Card
{
    /// <summary>
    /// Unique card ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Card name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Card set ID
    /// </summary>
    public int SetId { get; set; }

    /// <summary>
    /// Card power ID
    /// </summary>
    public int PowerId { get; set; }

    /// <summary>
    /// Card element
    /// </summary>
    public char Element { get; set; }

    /// <summary>
    /// Card color
    /// </summary>
    public char Color { get; set; }

    /// <summary>
    /// Value of card
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Play description
    /// </summary>
    public string Description { get; set; }

    public virtual ICollection<CardStarterDeck> CardStarterDecks { get; set; } = new List<CardStarterDeck>();

    public virtual ICollection<PenguinCard> PenguinCards { get; set; } = new List<PenguinCard>();

    public virtual ICollection<RedemptionCode> Codes { get; set; } = new List<RedemptionCode>();
}
