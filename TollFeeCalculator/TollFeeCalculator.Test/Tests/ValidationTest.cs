using NUnit.Framework;
using System;

namespace TollFeeCalculator.Test
{
    [TestFixture]
    public class ValidationTest
    {
        readonly TollCalculator tollFeeCalculator = new TollCalculator();
        readonly Vehicle car = new Vehicle(VehicleType.Car);

        readonly DateTime[] generalTollTimePasses = new DateTime[]
        {
                new DateTime(2022, 06, 17, 08, 00, 00),
                new DateTime(2022, 06, 22, 08, 00, 00),
                new DateTime(2022, 06, 18, 08, 00, 00)
        };

        [Test]
        public void Null_Parameters_Should_Return_0_Toll_Fee()
        {
            var expected = 0;

            var carAndTimeNullResult = tollFeeCalculator.GetTollFee(null, null);
            var carNullResult = tollFeeCalculator.GetTollFee(null, generalTollTimePasses);
            var timeNullResult = tollFeeCalculator.GetTollFee(car, null);
            var resultSum = carAndTimeNullResult + carNullResult + timeNullResult;

            Assert.AreEqual(expected, resultSum);
        }
        [Test]
        public void Empty_Toll_Passes_Collection_Should_Return_0_Toll_Fee()
        {
            var expected = 0;
            var emptyTollPasses = new DateTime[] { };

            var result = tollFeeCalculator.GetTollFee(car, emptyTollPasses);

            Assert.AreEqual(expected, result);
        }
    }
}