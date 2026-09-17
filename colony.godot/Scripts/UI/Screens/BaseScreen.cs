using System;
using Colony.Godot.Scripts.Infrastructure.Navigation;
using Godot;

namespace Colony.Godot.Scripts.UI.Screens;

public abstract partial class BaseScreen : Node
{
    protected readonly IScreenNavigator _navigator;
    protected Node? VisualRoot { get; set; }

    protected BaseScreen(IScreenNavigator navigator)
    {
        _navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
    }

    public virtual void OnEnter()
    {
        SetVisualActive(true);
    }

    public virtual void OnExit()
    {
        SetVisualActive(false);
    }

    protected void SetVisualActive(bool active)
    {
        if (VisualRoot is CanvasItem canvasItem)
            canvasItem.Visible = active;

        if (VisualRoot != null)
            VisualRoot.ProcessMode = active
                ? ProcessModeEnum.Inherit
                : ProcessModeEnum.Disabled;
    }
}