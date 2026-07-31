using Godot;

namespace Colony.Godot.Scripts.Infrastructure;

public partial class ScreenHost : Control
{
    public override void _Ready()
    {
        SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
    }
}