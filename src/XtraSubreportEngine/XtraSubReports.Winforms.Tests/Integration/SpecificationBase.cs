using System.Linq;
using NUnit.Framework;

namespace XtraSubReports.Winforms.Tests.Integration
{
    public class SpecificationBase
    {
        [SetUp]
        public void Setup()
        {
            Given();
            When();
        }
        protected virtual void Given()
        { }
        protected virtual void When()
        { }
    }
}