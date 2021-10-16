using NUnit.Framework;
using HexCS.Core;
using HexUN.Math;

namespace HexCSTests.Mathematics
{
    [TestFixture]
    public class InterpolationTests
    {
        [Test]
        public void Works()
        {
            // Arrange
            Interpolation interp = new Interpolation(1, 2, 2, EEasingFunction.Linear);

            // Act
            float t0 = interp.Interpolate(0);
            float t1 = interp.Interpolate(1);
            float t2 = interp.Interpolate(2);

            bool failed = false;

            foreach (EEasingFunction funcName in UTEnum.GetEnumAsArray<EEasingFunction>())
            {
                DEasingFunction func = funcName.Function();

                float zero = func(0);
                float one = func(1);

                if (zero != 0 || one != 1)
                {
                    failed = true;
                    break;
                }

                if (funcName.ToString().StartsWith("InOut"))
                {
                    float point5 = func(0.5f);

                    if(point5 != 0.5f)
                    {
                        failed = true;
                        break;
                    }
                }
            }

            // Assert
            Assert.That(t0 == 1);
            Assert.That(t1 == 1.5);
            Assert.That(t2 == 2);
            Assert.That(!failed);
        }
    }
}   