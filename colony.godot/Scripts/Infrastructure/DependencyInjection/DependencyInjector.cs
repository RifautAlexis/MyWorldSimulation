using System;
using System.Linq;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace Colony.Godot.Scripts.Infrastructure.DependencyInjection;

public static class DependencyInjector
{
    public static void Inject(object target, IServiceProvider services)
    {
        GD.Print($"Injecting dependencies into {target.GetType().Name}");
        
        var interfaces = target
            .GetType()
            .GetInterfaces()
            .Where(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() ==
                typeof(IInject<>));

        foreach (var injectable in interfaces)
        {
            var dependencyType = injectable.GetGenericArguments()[0];

            GD.Print($"Injecting {dependencyType.Name}");

            var dependency = services.GetRequiredService(dependencyType);

            var method = injectable.GetMethod("Inject");

            method!.Invoke(target, new[] { dependency });
        }
    }
}