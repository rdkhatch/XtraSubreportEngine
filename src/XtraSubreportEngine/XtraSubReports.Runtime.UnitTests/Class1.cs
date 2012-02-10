using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtraSubReports.Runtime.UnitTests
{
    public class Person2
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public List<Dog> Dogs { get; set; }
    }

    public class Dog
    {
        public string Name { get; set; }
    }
}
