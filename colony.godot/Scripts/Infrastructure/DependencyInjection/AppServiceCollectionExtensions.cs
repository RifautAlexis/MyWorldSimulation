using Colony.Godot.Scripts.Bootstrap;
using Colony.Godot.Scripts.Infrastructure.Navigation;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace Colony.Godot.Scripts.Infrastructure.DependencyInjection;

public static class AppServiceCollectionExtensions
{
    public static IServiceCollection AddColonyApp(this IServiceCollection services, Application application)
    {
        services.AddSingleton(application);
        services.AddSingleton<IScreenNavigator>(provider => new ScreenNavigator(application));

        return services;
    }
}
