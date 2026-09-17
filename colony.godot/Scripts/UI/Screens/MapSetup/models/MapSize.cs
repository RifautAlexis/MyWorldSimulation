namespace Colony.Godot.Scripts.UI.Screens.MapSetup.models;

public class MapSize
{
    public static readonly MapSize Small = new() { XAxis = 32, YAxis = 32 };
    public static readonly MapSize Medium = new() { XAxis = 64, YAxis = 64 };
    public static readonly MapSize Large = new() { XAxis = 128, YAxis = 128 };
    public int XAxis { get; private init; }
    public int YAxis { get; private init; }
}