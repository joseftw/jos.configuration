using System.Collections.Generic;

namespace JOS.Configuration.Tests;

public class Configuration : Dictionary<string, string>
{
    public Configuration()
    {
        this["Data:SomeInteger"] = "1";
        this["Data:SomeString"] = "any";
        this["Data:SomeList:0"] = "1";
        this["Data:SomeList:1"] = "2";
        this["Data:SomeList:2"] = "3";
        this["Data:Nested:SomeInteger"] = "2";
        this["Data:Nested:SomeString"] = "other";
        this["Data:Nested:SomeList:0"] = "4";
        this["Data:Nested:SomeList:1"] = "5";
        this["Data:Nested:SomeList:2"] = "6";
    }
}
