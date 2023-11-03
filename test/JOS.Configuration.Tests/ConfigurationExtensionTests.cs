using Microsoft.Extensions.Configuration;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace JOS.Configuration.Tests;

public class ConfigurationExtensionTests
{
    private static readonly IConfiguration Configuration;

    static ConfigurationExtensionTests()
    {
        Configuration = new ConfigurationBuilder().AddInMemoryCollection(new Configuration()!).Build();
    }

    [Fact]
    public void GetRequiredValue_ShouldThrowExceptionIfKeyIsMissing()
    {
        var exception = Should.Throw<Exception>(() => Configuration.GetRequiredValue<string>("Data:NonExisting"));

        exception.Message.ShouldBe(
            "'Data:NonExisting' had no value, have you forgot to add it to the Configuration?");
    }

    [Theory]
    [InlineData("Data:SomeInteger", 1)]
    [InlineData("Data:Nested:SomeInteger", 2)]
    public void GetRequiredValue_ShouldReturnConfigurationValueIfExisting_Int(string key, int expected)
    {
        var result = Configuration.GetRequiredValue<int>(key);

        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("Data:SomeString", "any")]
    [InlineData("Data:Nested:SomeString", "other")]
    public void GetRequiredValue_ShouldReturnConfigurationValueIfExisting_String(string key, string expected)
    {
        var result = Configuration.GetRequiredValue<string>(key);

        result.ShouldBe(expected);
    }

    [Fact]
    public void GetRequiredOptions_ShouldThrowExceptionIfKeyIsMissing()
    {
        var exception = Should.Throw<Exception>(() => Configuration.GetRequiredOptions<DataOptions>("NonExisting"));

        exception.Message.ShouldBe("Section 'NonExisting' not found in configuration.");
    }

    [Fact]
    public void GetRequiredOptions_ShouldBindExistingOptionsCorrectly()
    {
        var result = Configuration.GetRequiredOptions<DataOptions>("Data");

        result.SomeInteger.ShouldBe(1);
        result.SomeString.ShouldBe("any");
        result.SomeList.Count.ShouldBe(3);
        result.SomeList.ShouldContain(1);
        result.SomeList.ShouldContain(2);
        result.SomeList.ShouldContain(3);
        result.Nested.SomeInteger.ShouldBe(2);
        result.Nested.SomeString.ShouldBe("other");
        result.Nested.SomeList.Count.ShouldBe(3);
        result.Nested.SomeList.ShouldContain(4);
        result.Nested.SomeList.ShouldContain(5);
        result.Nested.SomeList.ShouldContain(6);
    }

    [Fact]
    public void GetRequiredValues_ShouldThrowExceptionIfKeyIsMissing()
    {
        var exception = Should.Throw<Exception>(() => Configuration.GetRequiredValues<string>("NonExisting"));

        exception.Message.ShouldBe("Section 'NonExisting' not found in configuration.");
    }

    [Fact]
    public void GetRequiredValues_ShouldBindExistingValuesCorrectly()
    {
        var result = Configuration.GetRequiredValues<int>("Data:SomeList").ToList();

        result.Count.ShouldBe(3);
        result.ShouldContain(1);
        result.ShouldContain(2);
        result.ShouldContain(3);
    }
}
