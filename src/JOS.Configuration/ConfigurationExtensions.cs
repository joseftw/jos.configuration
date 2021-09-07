using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace JOS.Configuration
{
    public static class ConfigurationExtensions
    {
        public static T GetRequiredValue<T>(
            this IConfiguration configuration,
            string key) where T : new()
        {
            if (typeof(T) == typeof(string) || typeof(T).IsValueType)
            {
                if (typeof(T) == typeof(DateTime))
                {
                    return (T)HandleDateTime(configuration, key);
                }

                var value = configuration.GetValue(typeof(T?), key);
                if (value == null)
                {
                    throw MissingRequiredKeyException(key);
                }

                return (T)value;
            }

            var configurationSection = configuration.GetRequiredSection(key);
            var data = new T();
            configurationSection.Bind(key, data);

            return data;
        }

        public static IEnumerable<T> GetRequiredValues<T>(this IConfiguration configuration, string key)
        {
            var configurationSection = configuration.GetRequiredSection(key);
            var target = new List<T>();
            configurationSection.Bind(target);
            var dateTimeProperties = typeof(T).GetProperties().Where(x => x.PropertyType == typeof(DateTime)).ToArray();

            if (!dateTimeProperties.Any())
            {
                return target;
            }

            foreach (var item in target)
            {
                foreach (var property in dateTimeProperties)
                {
                    var dateTime = (DateTime)property.GetValue(item)!;
                    // A UTC value will have kind set to Local -> Change to UTC
                    if (dateTime.Kind == DateTimeKind.Local)
                    {
                        property.SetValue(item, dateTime.ToUniversalTime());
                    }
                }
            }

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

        private static object HandleDateTime(IConfiguration configuration, string key)
        {
            var dateTimeValue = configuration.GetValue<string>(key);

            if (string.IsNullOrWhiteSpace(dateTimeValue))
            {
                throw MissingRequiredKeyException(key);
            }

            if (dateTimeValue.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
            {
                return DateTime.Parse(dateTimeValue, styles: DateTimeStyles.AdjustToUniversal);
            }

            return DateTime.Parse(dateTimeValue);
        }

        private static Exception MissingRequiredKeyException(string key) => 
            throw new Exception($"Key '{key}' had no value, have you forgot to add it to Configuration?");
    }
}
