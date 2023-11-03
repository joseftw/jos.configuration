using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace JOS.Configuration;

public static class ConfigurationExtensions
{
    public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
    {
        var value = configuration.GetValue(typeof(T?), key);
        return value == null ? throw MissingRequiredKeyException(key) : (T)value;
    }

    public static T GetRequiredOptions<T>(this IConfiguration configuration, string key) where T : new()
    {
        var configurationSection = configuration.GetRequiredSection(key);
        var data = new T();
        configurationSection.Bind(data);
        return data;
    }

    public static IEnumerable<T> GetRequiredValues<T>(this IConfiguration configuration, string key)
    {
        var configurationSection = configuration.GetRequiredSection(key);
        var target = new List<T>();
        configurationSection.Bind(target);

        return target;
    }

    private static Exception MissingRequiredKeyException(string key) =>
        throw new Exception($"'{key}' had no value, have you forgot to add it to the Configuration?");
}
