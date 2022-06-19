using NUnit.Framework;
using System;
using TollFeeCalculator.Test.Constants;

namespace TollFeeCalculator.Test
{
    [TestFixture]
    public class ValidationTest
    {
        readonly TollCalculator tollFeeCalculator = new TollCalculator();
        readonly TestConstants testConstants = new TestConstants();

        [Test]
        public void Null_Parameters_Should_Return_0_Toll_Fee()
        {
            var expected = 0;

            var carAndTimeNullResult = tollFeeCalculator.GetTollFee(null, null);
            var carNullResult = tollFeeCalculator.GetTollFee(null, testConstants.GeneralTollTimePasses);
            var timeNullResult = tollFeeCalculator.GetTollFee(testConstants.Car, null);
            var resultSum = carAndTimeNullResult + carNullResult + timeNullResult;

            Assert.AreEqual(expected, resultSum);
        }
        [Test]
        public void Empty_Toll_Passes_Collection_Should_Return_0_Toll_Fee()
        {
            var expected = 0;
            var emptyTollPasses = new DateTime[] { };

            var result = tollFeeCalculator.GetTollFee(testConstants.Car, emptyTollPasses);

            Assert.AreEqual(expected, result);
        }
    }
}