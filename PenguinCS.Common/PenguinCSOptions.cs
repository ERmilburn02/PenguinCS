namespace PenguinCS.Common;

public class PenguinCSOptions
{
    public string RandomKey { get; set; }

    public ushort LegacyVersion { get; set; }
    public ushort VanillaVersion { get; set; }

    public ushort PreActivationDays { get; set; }

    public ushort AuthTTLSeconds { get; set; }

    public ushort MaxPlayers { get; set; }
}