using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Custom plugin attributes
/// </summary>
public partial class PluginAttribute
{
    /// <summary>
    /// Attribute unique identifier
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Name of plugin attribute belongs to
    /// </summary>
    public string PluginName { get; set; }

    /// <summary>
    /// Value of attribute
    /// </summary>
    public string Value { get; set; }
}
