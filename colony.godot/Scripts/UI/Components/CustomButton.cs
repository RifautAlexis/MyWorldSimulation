using Godot;

namespace Colony.Godot.Scripts.UI.Components;

public partial class CustomButton : Button
{
    public Color BackgroundColor { get; set; } = new(0.16f, 0.2f, 0.29f);
    public Color HoverBackgroundColor { get; set; } = new(0.22f, 0.27f, 0.38f);
    public Color PressedBackgroundColor { get; set; } = new(0.11f, 0.15f, 0.22f);
    public Color DisabledBackgroundColor { get; set; } = new(0.1f, 0.1f, 0.1f, 0.6f);
    public Color FontColor { get; set; } = Colors.White;
    public Color DisabledFontColor { get; set; } = new(0.8f, 0.8f, 0.8f, 0.7f);
    public int FontSize { get; set; } = 20;
    public int CornerRadius { get; set; } = 12;
    public int HorizontalPadding { get; set; } = 18;
    public int VerticalPadding { get; set; } = 12;

    public override void _Ready()
    {
        ApplyStyle();
    }

    public void ApplyStyle()
    {
        CustomMinimumSize = new Vector2(0f, 50f);
        FocusMode = FocusModeEnum.All;

        AddThemeColorOverride("font_color", FontColor);
        AddThemeColorOverride("font_hover_color", FontColor);
        AddThemeColorOverride("font_pressed_color", FontColor);
        AddThemeColorOverride("font_disabled_color", DisabledFontColor);
        AddThemeFontSizeOverride("font_size", FontSize);
        AddThemeConstantOverride("h_separation", 8);

        AddThemeStyleboxOverride("normal", CreateStyleBox(BackgroundColor));
        AddThemeStyleboxOverride("hover", CreateStyleBox(HoverBackgroundColor));
        AddThemeStyleboxOverride("pressed", CreateStyleBox(PressedBackgroundColor));
        AddThemeStyleboxOverride("focus", CreateStyleBox(HoverBackgroundColor));
        AddThemeStyleboxOverride("disabled", CreateStyleBox(DisabledBackgroundColor));
    }

    private StyleBoxFlat CreateStyleBox(Color backgroundColor)
    {
        return new StyleBoxFlat
        {
            BgColor = backgroundColor,
            CornerRadiusTopLeft = CornerRadius,
            CornerRadiusTopRight = CornerRadius,
            CornerRadiusBottomRight = CornerRadius,
            CornerRadiusBottomLeft = CornerRadius,
            ContentMarginLeft = HorizontalPadding,
            ContentMarginRight = HorizontalPadding,
            ContentMarginTop = VerticalPadding,
            ContentMarginBottom = VerticalPadding,
        };
    }
}
