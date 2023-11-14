// See https://aka.ms/new-console-template for more information

using JOS.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

var configuration = new ConfigurationBuilder().AddInMemoryCollection(new List<KeyValuePair<string, string?>>
{
    new("SomeKey", "SomeValue")
}).Build();

var value = configuration.GetRequiredValue<string>("SomeKey");
Console.WriteLine(value);
