using Godot;

namespace Colony.Godot.Scripts.UI.Screens.MapSetup.UI;

public class LoadingOverlay
{
    // Root Control
    private Control LoadingRoot { get; }

    // UI Elements
    private Label CurrentStepLabel { get; set; } = null!;

    public LoadingOverlay()
    {
        LoadingRoot = new Control
        {
            Name = "LoadingRoot",
            Visible = false,
            MouseFilter = Control.MouseFilterEnum.Stop,
        };
        LoadingRoot.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
    }

    public Control BuildLoadingOverlay()
    {
        var blurOverlay = new ColorRect
        {
            Name = "BlurOverlay",
            Color = new Color(0f, 0f, 0f, 0.35f),
            MouseFilter = Control.MouseFilterEnum.Stop,
        };
        blurOverlay.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        LoadingRoot.AddChild(blurOverlay);

        var stepContainer = new CenterContainer
        {
            Name = "StepContainer",
            MouseFilter = Control.MouseFilterEnum.Ignore,
        };
        stepContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        LoadingRoot.AddChild(stepContainer);

        CurrentStepLabel = new Label
        {
            Name = "StepLabel",
            Text = "Loading...",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        CurrentStepLabel.AddThemeFontSizeOverride("font_size", 28);
        stepContainer.AddChild(CurrentStepLabel);

        return LoadingRoot;
    }

    public void Show()
    {
        LoadingRoot.Visible = true;
    }

    public void Hide()
    {
        LoadingRoot.Visible = false;
    }

    public void UpdateCurrentStepLabel(string step)
    {
        CurrentStepLabel.Text = $"{step} ...";
    }
}