using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;

namespace LogicTests
{
    [TestFixture]
    public class ParametersTest
    {
        [Test]
        public void SeatLengthTest()
        {
            Parameters parameters = new Parameters();
            parameters.SeatLength = 10;
            Assert.That(parameters.SeatLength, Is.EqualTo(10));
        }

        [Test]
        public void SeatWidthTest()
        {
            Parameters parameters = new Parameters();
            parameters.SeatWidth = 10;
            Assert.That(parameters.SeatWidth, Is.EqualTo(10));
        }

        [Test]
        public void SeatThicknessTest()
        {
            Parameters parameters = new Parameters();
            parameters.SeatThickness = 10;
            Assert.That(parameters.SeatThickness, Is.EqualTo(10));
        }

        [Test]
        public void LegLengthTest()
        {
            Parameters parameters = new Parameters();
            parameters.LegLength = 10;
            Assert.That(parameters.LegLength, Is.EqualTo(10));
        }

        [Test]
        public void LegWidthTest()
        {
            Parameters parameters = new Parameters();
            parameters.LegWidth = 10;
            Assert.That(parameters.LegWidth, Is.EqualTo(10));
        }

        [Test]
        public void LegHeightTest()
        {
            Parameters parameters = new Parameters();
            parameters.LegHeight = 10;
            Assert.That(parameters.LegHeight, Is.EqualTo(10));
        }

        [Test]
        public void ValidateTest()
        {
            Parameters parameters = new Parameters();
            Assert.IsNotNull(parameters);
            Assert.That(parameters.CheckDependentParametersValue(), Is.EqualTo(false));

            parameters.SeatThickness = 30;
            parameters.LegLength = 300;

            Assert.That(parameters.CheckDependentParametersValue(), Is.EqualTo(true));

            parameters.SeatThickness = 21;
            parameters.LegLength = 309;

            Assert.That(parameters.CheckDependentParametersValue(), Is.EqualTo(true));

            parameters.SeatThickness = 20;
            parameters.LegLength = 300;

            Assert.That(parameters.CheckDependentParametersValue(), Is.EqualTo(false));
        }
    }
}
