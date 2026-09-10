using System;
using Colony.Godot.Scripts.Infrastructure.Navigation;
using Godot;

namespace Colony.Godot.Scripts.UI.Screens;

public abstract partial class BaseScreen : Control
{
    protected readonly IScreenNavigator _navigator;

    protected BaseScreen(IScreenNavigator navigator)
    {
        _navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
    }

    public virtual void OnEnter()
    {
        Visible = true;
    }

    public virtual void OnExit()
    {
        Visible = false;
    }
}