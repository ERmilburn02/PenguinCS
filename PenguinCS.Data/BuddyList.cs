using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Penguin buddy relationships
/// </summary>
public partial class BuddyList
{
    public int PenguinId { get; set; }

    public int BuddyId { get; set; }

    public bool BestBuddy { get; set; }

    public virtual Penguin Buddy { get; set; }

    public virtual Penguin Penguin { get; set; }
}
