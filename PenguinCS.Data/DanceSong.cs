using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Dance contest multiplayer tracks
/// </summary>
public partial class DanceSong
{
    /// <summary>
    /// Unique song ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of song
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Length of song in milliseconds
    /// </summary>
    public int SongLengthMillis { get; set; }

    /// <summary>
    /// Length of song in beats
    /// </summary>
    public int SongLength { get; set; }

    /// <summary>
    /// Milliseconds per song note
    /// </summary>
    public int MillisPerBar { get; set; }
}
