using Godot;

namespace Colony.Godot.Scripts.UI.Screens;

public abstract partial class BaseScreen : Control
{
    public virtual void OnEnter()
    {
        Visible = true;
    }

    public virtual void OnExit()
    {
        Visible = false;
    }
}
