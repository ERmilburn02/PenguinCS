using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

public partial class PenguinGameDatum
{
    public int PenguinId { get; set; }

    public int RoomId { get; set; }

    public int Index { get; set; }

    public string Data { get; set; }

    public virtual Penguin Penguin { get; set; }

    public virtual Room Room { get; set; }
}
