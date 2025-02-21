using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Player reports
/// </summary>
public partial class Report
{
    /// <summary>
    /// Unique report ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Reported penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Reporting penguin ID
    /// </summary>
    public int ReporterId { get; set; }

    /// <summary>
    /// Report type ID
    /// </summary>
    public int ReportType { get; set; }

    /// <summary>
    /// Date of report
    /// </summary>
    public DateTime Date { get; set; }

    public int ServerId { get; set; }

    public int RoomId { get; set; }

    public virtual Penguin Penguin { get; set; }

    public virtual Penguin Reporter { get; set; }

    public virtual Room Room { get; set; }
}
