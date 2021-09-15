using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace JOS.Configuration
{
    public static class ConfigurationExtensions
    {
        public static T GetRequiredValue<T>(
            this IConfiguration configuration,
            string key)
        {
            var value = configuration.GetValue(typeof(T?), key);
            if (value == null)
            {
                throw MissingRequiredKeyException(key);
            }

            return (T)value;
        }

        public static T GetRequiredOptions<T>(
            this IConfiguration configuration,
            string key
        ) where T : new()
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

        // Inspired by GetRequiredSection in dotnet 6
        // https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationextensions.getrequiredsection?view=dotnet-plat-ext-6.0
        // Replace with built in method when upgrading this to dotnet 6
        private static IConfigurationSection GetRequiredSection(this IConfiguration configuration, string key)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var section = configuration.GetSection(key);
            if (section.Exists())
            {
                return section;
            }

            throw new Exception($"Section '{key}' not found in configuration.");
        }

        private static bool Exists(this IConfigurationSection? section)
        {
            if (section == null)
            {
                return false;
            }
            return section.Value != null || section.GetChildren().Any();
        }

        private static Exception MissingRequiredKeyException(string key) =>
            throw new Exception($"'{key}' had no value, have you forgot to add it to the Configuration?");
    }
}
