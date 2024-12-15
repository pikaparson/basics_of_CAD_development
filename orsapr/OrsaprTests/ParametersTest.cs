using NUnit;
using orsapr;

namespace OrsaprTests
{
    [TestFixture]
    public class ParametersTest
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