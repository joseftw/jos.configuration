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
        public void ShouldBindExistingStringCorrectly()
        {
            var result = _configuration.GetRequiredValue<string>("string");

            result.ShouldBe("data");
        }

        [Fact]
        public void ShouldThrowExceptionForNonExistingString()
        {
            var exception = Should.Throw<Exception>(() => _configuration.GetRequiredValue<string>("nonExistingString"));

            exception.Message.ShouldBe("'nonExistingString' had no value, have you forgot to add it to the Configuration?");
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

            exception.Message.ShouldBe("'nonExistingInt' had no value, have you forgot to add it to the Configuration?");
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

            exception.Message.ShouldBe("'nonExistingBool' had no value, have you forgot to add it to the Configuration?");
        }

        [Fact]
        public void ShouldBindDateTimeOffsetCorrectly()
        {
            var result = _configuration.GetRequiredValue<DateTimeOffset>("date");

            result.ShouldBe(new DateTime(2021, 01, 02, 08, 09, 10, DateTimeKind.Utc));
        }

        [Fact]
        public void ShouldThrowExceptionForNonExistingDateTimeOffset()
        {
            var exception = Should.Throw<Exception>(() => _configuration.GetRequiredValue<DateTimeOffset>("nonExistingDateTimeOffset"));

            exception.Message.ShouldBe("'nonExistingDateTimeOffset' had no value, have you forgot to add it to the Configuration?");
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
            var firstItem = resultList.First();

            resultList.Count.ShouldBe(3);
            firstItem.Date.ShouldBe(new DateTime(2021, 01, 02, 08, 09, 10, DateTimeKind.Utc));
        }

        [Fact]
        public void ShouldHandleUtcDateTimesInComplexArrayCorrectly()
        {
            var result = _configuration.GetRequiredValues<ExampleOptions>("arrayComplex");

            var resultList = result.ToList();
            var firstItem = resultList.First();

            resultList.Count.ShouldBe(3);
            firstItem.Date.ShouldBe(new DateTime(2021, 01, 02, 08, 09, 10, DateTimeKind.Utc));
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
            var exception = Should.Throw<Exception>(() => _configuration.GetRequiredOptions<ExampleOptions>("nonExistingExampleOptions"));

            exception.Message.ShouldBe("Section 'nonExistingExampleOptions' not found in configuration.");
        }

        [Fact]
        public void ShouldReturnUtcDatesByDefaultForDateTimeOffsetWhenCallingGetRequiredValue()
        {
            var result = _configuration.GetRequiredValue<DateTimeOffset>("date");

            result.ShouldBe(new DateTime(2021, 01, 02, 08, 09, 10, DateTimeKind.Utc));
        }

        [Fact]
        public void ShouldReturnUtcDatesByDefaultForDateTimeOffsetWhenCallingGetRequiredValues()
        {
            var result = _configuration.GetRequiredValues<DateTimeOffset>("dates");

            var utcDates = result.Select(x => x.UtcDateTime);

            utcDates.ShouldAllBe(x => x == new DateTime(2021, 01, 02, 08, 09, 10));
        }

        [Fact]
        public void ShouldReturnUtcDatesByDefaultForDateTimeOffsetWhenCallingGetRequiredOptions()
        {
            var result = _configuration.GetRequiredOptions<BlogExampleOptions>("blogExample");

            result.SomeDate.ShouldBe(new DateTime(2021, 01, 02, 08, 09, 10, DateTimeKind.Utc));
        }
    }
}
