namespace Colony.Godot.Scripts.Infrastructure.DependencyInjection;

public interface IInject<in T>
{
    void Inject(T dependency);
}