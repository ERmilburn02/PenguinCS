using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Penguin SoundStudio tracks
/// </summary>
public partial class PenguinTrack
{
    /// <summary>
    /// Unique track ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of track
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Owner penguin ID
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Is song shared?
    /// </summary>
    public bool Sharing { get; set; }

    /// <summary>
    /// Song data
    /// </summary>
    public string Pattern { get; set; }

    public virtual Penguin Owner { get; set; }

    public virtual ICollection<TrackLike> TrackLikes { get; set; } = new List<TrackLike>();
}
