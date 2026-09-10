using System;
using Colony.Godot.Scripts.UI.Screens;

namespace Colony.Godot.Scripts.Infrastructure.Navigation;

public interface IScreenNavigator
{
    bool CanNavigateBack { get; }

    void Register(string route, Func<BaseScreen> factory);
    void NavigateTo(string route, object? payload = null);
    TPayload? GetPayload<TPayload>() where TPayload : class;
    void ClearPayload();
    void NavigateBack();
}