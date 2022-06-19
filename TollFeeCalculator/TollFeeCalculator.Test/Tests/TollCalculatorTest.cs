using NUnit.Framework;
using System;
using TollFeeCalculator.Test.Constants;

namespace TollFeeCalculator.Test.Tests
{
    [TestFixture]
    internal class TollCalculatorTest
    {
        readonly TollCalculator tollFeeCalculator = new TollCalculator();
        readonly TestConstants testConstants = new TestConstants();

        [Test]
        public void When_Car_Passes_All_Tolls_With_Fee_18_Once_Get_Correct_Fee()
        {
            DateTime[] dates = new DateTime[] {
                new DateTime(2022, 06, 17, 07, 00, 00),
                new DateTime(2022, 06, 17, 16, 59, 59)
            };

            var expected = 36;
            var result = tollFeeCalculator.GetTollFee(testConstants.Car, dates);

            Assert.AreEqual(expected, result);
        }
    }
}
