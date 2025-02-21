using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// SoundStudio likes
/// </summary>
public partial class TrackLike
{
    /// <summary>
    /// Liker penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Liked track ID
    /// </summary>
    public int TrackId { get; set; }

    /// <summary>
    /// Timestamp of like
    /// </summary>
    public DateTime Date { get; set; }

    public virtual Penguin Penguin { get; set; }

    public virtual PenguinTrack Track { get; set; }
}
