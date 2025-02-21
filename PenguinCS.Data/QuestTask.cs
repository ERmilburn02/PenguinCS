using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Player map quest tasks
/// </summary>
public partial class QuestTask
{
    /// <summary>
    /// Unique task ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Task quest ID
    /// </summary>
    public int QuestId { get; set; }

    /// <summary>
    /// Description of task
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Room ID for completion
    /// </summary>
    public int? RoomId { get; set; }

    public string Data { get; set; }

    public virtual ICollection<PenguinQuestTask> PenguinQuestTasks { get; set; } = new List<PenguinQuestTask>();

    public virtual Quest Quest { get; set; }

    public virtual Room Room { get; set; }
}
