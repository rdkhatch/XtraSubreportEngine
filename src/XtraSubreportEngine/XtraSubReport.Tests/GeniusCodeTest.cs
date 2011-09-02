using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeniusCode.Framework.Extensions;

namespace XtraSubReport.Tests
{
    [TestClass]
    public class GeniusCodeTest
    {
        [TestMethod]
        public void FullOuterJoinTest()
        {
            var Ryan = new Person { Name = "Ryan" };
            var Jer = new Person { Name = "Jer" };
            var people = new List<Person>(){ Ryan, Jer};

            var Camp = new Dog() { Name = "Camp", Owner = Ryan };
            var Brody = new Dog() { Name = "Brody", Owner = Ryan};
            var Homeless = new Dog() { Name = "Homeless" };
            var dogs = new List<Dog>() { Camp, Brody, Homeless };

            Func<Person, Dog, bool> match = (person, dog) => { return dog == null ? false : ReferenceEquals(dog.Owner, person); };

            var join = people.FullOuterJoin(dogs, match).ToList();

            Assert.AreEqual(4, join.Count);
            Assert.IsTrue( join.Contains(new Tuple<Person,Dog>(null, Homeless) ));
            Assert.IsTrue( join.Contains(new Tuple<Person,Dog>(Ryan, Camp) ));
            Assert.IsTrue( join.Contains(new Tuple<Person,Dog>(Ryan, Brody) ));
            Assert.IsTrue( join.Contains(new Tuple<Person,Dog>(Jer, null) ));
        }

        public class Dog
        {
            public string Name { get; set; }
            public Person Owner { get; set; }
        }

        public class Person
        {
            public string Name { get; set; }
        }

    }
}
