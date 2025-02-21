using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Custom penguin attributes
/// </summary>
public partial class PenguinAttribute
{
    /// <summary>
    /// Attribute unique identifier
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Value of attribute
    /// </summary>
    public string Value { get; set; }

    public virtual Penguin Penguin { get; set; }
}
