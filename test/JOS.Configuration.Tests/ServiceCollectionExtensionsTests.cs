using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace JOS.Configuration.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        private readonly IConfiguration _configuration;
        public ServiceCollectionExtensionsTests()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        [Fact]
        public void ShouldAddOptionsToServiceCollection()
        {
            var services = new ServiceCollection();

            services.AddPocoOptions<BlogExampleOptions>("BlogExample", _configuration);
            var serviceProvider = services.BuildServiceProvider();
            var blogExampleOptions = serviceProvider.GetRequiredService<BlogExampleOptions>();

            blogExampleOptions.Name.ShouldBe("Josef");
            blogExampleOptions.Enabled.ShouldBeTrue();
            blogExampleOptions.SomeDate.ShouldBe(new DateTime(2021, 01, 02, 08, 09, 10, DateTimeKind.Utc));
        }

        [Fact]
        public void ShouldAddOptionsToServiceCollectionWhenUsingOutParameter()
        {
            var services = new ServiceCollection();

            services.AddPocoOptions<BlogExampleOptions>("BlogExample", _configuration, out var options);
            var serviceProvider = services.BuildServiceProvider();
            var blogExampleOptions = serviceProvider.GetRequiredService<BlogExampleOptions>();
            
            options.ShouldBeSameAs(blogExampleOptions);
        }
    }
}
