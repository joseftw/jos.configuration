using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JOS.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPocoOptions<T>(
        this IServiceCollection services, string key, IConfiguration configuration) where T : class, new()
    {
        var options = configuration.GetRequiredOptions<T>(key);
        services.AddSingleton<T>(options);
        return services;
    }

    public static IServiceCollection AddPocoOptions<T>(
        this IServiceCollection services,
        string key,
        IConfiguration configuration, out T options) where T : class, new()
    {
        options = configuration.GetRequiredOptions<T>(key);
        services.AddSingleton<T>(options);
        return services;
    }
}

