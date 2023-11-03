using System.Collections.Generic;

namespace JOS.Configuration.Tests;

public class DataOptions
{
    public int SomeInteger { get; set; }
    public string SomeString { get; set; } = null!;
    public List<int> SomeList { get; set; } = null!;
    public Nested Nested { get; set; } = null!;
}

public class Nested
{
    public int SomeInteger { get; set; }
    public string SomeString { get; set; } = null!;
    public List<int> SomeList { get; set; } = null!;
}
