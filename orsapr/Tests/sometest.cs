using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class sometest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ValidateTest()
        {
            Parameters parameters = new Parameters();
            Assert.That(parameters.CheckDependentParametersValue(), Is.EqualTo(false));

            parameters.SeatThickness = 30;
            parameters.LegLength = 300;
            Assert.That(parameters.CheckDependentParametersValue(), Is.EqualTo(true));
        }
    }
}
