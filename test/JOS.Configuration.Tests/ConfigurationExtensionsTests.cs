using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Shouldly;
using Xunit;

namespace JOS.Configuration.Tests
{
    public class ConfigurationExtensionsTests
    {
        private readonly IConfiguration _configuration;
        public ConfigurationExtensionsTests()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        [Fact]
        public void ShouldBindExistingIntegerCorrectly()
        {
            var result = _configuration.GetRequiredValue<int>("int");

            result.ShouldBe(1);
        }

        [Fact]
        public void ShouldThrowExceptionForNonExistingInteger()
        {
            var exception = Should.Throw<Exception>(() => _configuration.GetRequiredValue<int>("nonExistingInt"));

            exception.Message.ShouldBe("Key 'nonExistingInt' had no value, have you forgot to add it to Configuration?");
        }

        [Fact]
        public void ShouldBindExistingBoolCorrectly()
        {
            var result = _configuration.GetRequiredValue<bool>("bool");

            result.ShouldBeTrue();
        }

        [Fact]
        public void ShouldThrowExceptionForNonExistingBool()
        {
            var exception = Should.Throw<Exception>(() => _configuration.GetRequiredValue<int>("nonExistingBool"));

            exception.Message.ShouldBe("Key 'nonExistingBool' had no value, have you forgot to add it to Configuration?");
        }

        [Fact]
        public void ShouldBindExistingUtcDateTimeCorrectly()
        {
            var result = _configuration.GetRequiredValue<DateTime>("datetimeUtc");

            result.Kind.ShouldBe(DateTimeKind.Utc);
            result.ShouldBe(new DateTime(2021, 01, 02, 08, 09, 10, DateTimeKind.Utc));
        }

        [Fact]
        public void ShouldBindExistingUnspecifiedDateTimeCorrectly()
        {
            var result = _configuration.GetRequiredValue<DateTime>("datetime");

            result.Kind.ShouldBe(DateTimeKind.Unspecified);
            result.ShouldBe(new DateTime(2021, 01, 02, 08, 09, 10, DateTimeKind.Unspecified));
        }

        [Fact]
        public void ShouldThrowExceptionForNonExistingDateTime()
        {
            var exception = Should.Throw<Exception>(() => _configuration.GetRequiredValue<DateTime>("nonExistingDateTime"));

            exception.Message.ShouldBe("Key 'nonExistingDateTime' had no value, have you forgot to add it to Configuration?");
        }

        [Fact]
        public void ShouldBindExistingArrayCorrectly()
        {
            var result = _configuration.GetRequiredValues<string>("arraySimple");

            var resultList = result.ToList();

            resultList.Count.ShouldBe(3);
            resultList.ShouldContain("value1");
            resultList.ShouldContain("value2");
            resultList.ShouldContain("value3");
        }

        [Fact]
        public void ShouldBindExistingComplexArrayCorrectly()
        {
            var result = _configuration.GetRequiredValues<ExampleOptions>("arrayComplex");

            var resultList = result.ToList();

            resultList.Count.ShouldBe(3);
            resultList.ShouldContain(x => x.String == "value1" && x.Bool && x.Int == 1);
            resultList.ShouldContain(x => x.String == "value2" && x.Bool && x.Int == 2);
            resultList.ShouldContain(x => x.String == "value3" && !x.Bool && x.Int == 3);
        }

        [Fact]
        public void ShouldHandleDateTimesInComplexArrayCorrectly()
        {
            var result = _configuration.GetRequiredValues<ExampleOptions>("arrayComplex");

            var resultList = result.ToList();

            resultList.Count.ShouldBe(3);
            var firstItem = resultList.First();
            firstItem.DateTime.Kind.ShouldBe(DateTimeKind.Unspecified);
            firstItem.DateTime.ShouldBe(new DateTime(2021, 01, 02, 08, 09, 10, DateTimeKind.Unspecified));
        }

        [Fact]
        public void ShouldHandleUtcDateTimesInComplexArrayCorrectly()
        {
            var result = _configuration.GetRequiredValues<ExampleOptions>("arrayComplex");

            var resultList = result.ToList();

            resultList.Count.ShouldBe(3);
            var firstItem = resultList.First();
            firstItem.DateTimeUtc.Kind.ShouldBe(DateTimeKind.Utc);
            firstItem.DateTimeUtc.ShouldBe(new DateTime(2021, 01, 02, 08, 09, 10, DateTimeKind.Utc));
        }

        [Fact]
        public void ShouldThrowExceptionWhenTryingToBindEnumerableToNonExistingSection()
        {
            var exception = Should.Throw<Exception>(() => _configuration.GetRequiredValues<ExampleOptions>("nonExistingExampleOptions"));

            exception.Message.ShouldBe("Section 'nonExistingExampleOptions' not found in configuration.");
        }

        [Fact]
        public void ShouldThrowExceptionWhenTryingToBindToNonExistingSection()
        {
            var exception = Should.Throw<Exception>(() => _configuration.GetRequiredValue<ExampleOptions>("nonExistingExampleOptions"));

            exception.Message.ShouldBe("Section 'nonExistingExampleOptions' not found in configuration.");
        }
    }
}
