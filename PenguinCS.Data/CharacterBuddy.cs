using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Penguin character buddies
/// </summary>
public partial class CharacterBuddy
{
    public int PenguinId { get; set; }

    public int CharacterId { get; set; }

    public bool BestBuddy { get; set; }

    public virtual Character Character { get; set; }

    public virtual Penguin Penguin { get; set; }
}
