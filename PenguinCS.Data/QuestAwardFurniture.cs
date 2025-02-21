using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

public partial class QuestAwardFurniture
{
    /// <summary>
    /// Quest ID
    /// </summary>
    public int QuestId { get; set; }

    /// <summary>
    /// Furniture item ID
    /// </summary>
    public int FurnitureId { get; set; }

    public int Quantity { get; set; }

    public virtual Furniture Furniture { get; set; }

    public virtual Quest Quest { get; set; }
}
