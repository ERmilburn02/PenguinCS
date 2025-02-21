using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Server postcard crumbs
/// </summary>
public partial class Postcard
{
    /// <summary>
    /// Unique postcard ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Postcard name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Cost of postcard
    /// </summary>
    public int Cost { get; set; }

    /// <summary>
    /// Can send postcard?
    /// </summary>
    public bool Enabled { get; set; }

    public virtual ICollection<PenguinPostcard> PenguinPostcards { get; set; } = new List<PenguinPostcard>();

    public virtual ICollection<Puffle> Puffles { get; set; } = new List<Puffle>();
}
