using System;

namespace JOS.Configuration.Tests
{
    public class BlogExampleOptions
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public DateTimeOffset SomeDate { get; set; }
    }
}
