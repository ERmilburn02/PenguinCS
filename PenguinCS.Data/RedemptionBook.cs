using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Redemption books
/// </summary>
public partial class RedemptionBook
{
    /// <summary>
    /// Unique redemption book ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Book name
    /// </summary>
    public string Name { get; set; }

    public virtual ICollection<RedemptionBookWord> RedemptionBookWords { get; set; } = new List<RedemptionBookWord>();

    public virtual ICollection<Penguin> Penguins { get; set; } = new List<Penguin>();
}
