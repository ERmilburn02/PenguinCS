using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Player map quests
/// </summary>
public partial class Quest
{
    /// <summary>
    /// Unique quest ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Short name of quest
    /// </summary>
    public string Name { get; set; }

    public virtual ICollection<QuestAwardFurniture> QuestAwardFurnitures { get; set; } = new List<QuestAwardFurniture>();

    public virtual ICollection<QuestAwardPuffleItem> QuestAwardPuffleItems { get; set; } = new List<QuestAwardPuffleItem>();

    public virtual ICollection<QuestTask> QuestTasks { get; set; } = new List<QuestTask>();

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
