using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

public partial class QuestAwardPuffleItem
{
    /// <summary>
    /// Quest ID
    /// </summary>
    public int QuestId { get; set; }

    /// <summary>
    /// Puffle care item ID
    /// </summary>
    public int PuffleItemId { get; set; }

    public int Quantity { get; set; }

    public virtual PuffleItem PuffleItem { get; set; }

    public virtual Quest Quest { get; set; }
}
