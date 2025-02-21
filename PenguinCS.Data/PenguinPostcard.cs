using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Sent postcards
/// </summary>
public partial class PenguinPostcard
{
    /// <summary>
    /// Unique postcard ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Postcard type ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Sender penguin ID
    /// </summary>
    public int? SenderId { get; set; }

    /// <summary>
    /// Postcard type ID
    /// </summary>
    public int PostcardId { get; set; }

    /// <summary>
    /// Postcard type ID
    /// </summary>
    public DateTime SendDate { get; set; }

    /// <summary>
    /// Postcard details
    /// </summary>
    public string Details { get; set; }

    /// <summary>
    /// Is read?
    /// </summary>
    public bool HasRead { get; set; }

    public virtual Penguin Penguin { get; set; }

    public virtual Postcard Postcard { get; set; }

    public virtual Penguin Sender { get; set; }
}
