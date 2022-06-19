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
        #region Null Or Empty Object Tests
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
        #endregion
        #region Public Holiday Tests
        [Test]
        [TestCase("2022-01-06 12:00:00")] // Epiphany day - Thursday
        [TestCase("2022-06-06 08:00:00")] // National day - Monday
        [TestCase("2023-12-25 12:00:00")] // Christmas day - Monday
        public void Public_Holidays_Should_Be_Tool_Free(DateTime tollPass)
        {
            var expected = true;

            var result = tollFeeCalculator.IsTollFreeDate(tollPass);

            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("2022-04-14 12:00:00")] // day before Good Friday - Thursday
        [TestCase("2023-01-05 08:00:00")] // day before Epiphany day - Thursday
        [TestCase("2023-05-18 12:00:00")] // day before Ascension Day - Wednesday
        public void Day_Before_Public_Holiday_Should_Be_Tool_Free(DateTime tollPass)
        {
            var expected = true;

            var result = tollFeeCalculator.IsTollFreeDate(tollPass);

            Assert.AreEqual(expected, result);
        }
        #endregion
        #region Free Toll Passage Test
        [Test]
        [TestCase("2022-06-20 05:59:59")]
        [TestCase("2022-06-20 18:30:01")] 
        public void Toll_Pass_Before_And_After_Fee_Starts_Should_Be_Free(DateTime tollPass)
        {
            var expected = true;

            var result = tollFeeCalculator.IsTollFreeTime(tollPass);

            Assert.AreEqual(expected, result);
        }
        [Test]
        [TestCase("2022-06-20 06:00:00")]
        [TestCase("2022-06-20 18:29:29")]
        public void Toll_Pass_During_Fee_Time_Should_Not_Be_Free(DateTime tollPass)
        {
            var expected = false;

            var result = tollFeeCalculator.IsTollFreeTime(tollPass);

            Assert.AreEqual(expected, result);
        }
        #endregion

    }
}