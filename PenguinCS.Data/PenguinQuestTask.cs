using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Completed quest tasks
/// </summary>
public partial class PenguinQuestTask
{
    /// <summary>
    /// Completed task ID
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Task penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    public bool Complete { get; set; }

    public virtual Penguin Penguin { get; set; }

    public virtual QuestTask Task { get; set; }
}
