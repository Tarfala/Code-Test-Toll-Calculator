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
        readonly TollFeeConstants tollFeeConstants = new TollFeeConstants();

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

        [Test]
        public void When_Car_Passes_All_Tolls_With_Fee_8_Once_Get_Correct_Fee()
        {
            DateTime[] dates = new DateTime[] {
                new DateTime(2022, 06, 20, 06, 00, 00),
                new DateTime(2022, 06, 20, 14, 59, 59),
                new DateTime(2022, 06, 20, 18, 00, 00)
            };

            var expected = 24;
            var result = tollFeeCalculator.GetTollFee(testConstants.Car, dates);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void When_Fee_Exceeds_Maxiumum_Fee_Get_Maximum_Fee()
        {
            DateTime[] dates = new DateTime[] {
                new DateTime(2021, 03, 02, 06, 00, 00),            
                new DateTime(2021, 03, 02, 07, 00, 00),
                new DateTime(2021, 03, 02, 08, 00, 00),
                new DateTime(2021, 03, 02, 09, 00, 00),
                new DateTime(2021, 03, 02, 10, 00, 00),
                new DateTime(2021, 03, 02, 11, 00, 00),
                new DateTime(2021, 03, 02, 12, 00, 00),
                new DateTime(2021, 03, 02, 13, 00, 00)
            };

            var expected = tollFeeConstants.MaximumFeeParDay;
            var result = tollFeeCalculator.GetTollFee(testConstants.Car, dates);

            Assert.AreEqual(expected, result);
        }
        [Test]
        public void When_Passing_Toll_At_0800_1500_1800_Get_Correct_Fee()
        {
            DateTime[] dates = new DateTime[] {
                new DateTime(2022, 06, 20, 08, 00, 00),
                new DateTime(2022, 03, 02, 15, 00, 00),
                new DateTime(2022, 03, 02, 18, 00, 00)
            };

            var expected = 34;
            var result = tollFeeCalculator.GetTollFee(testConstants.Car, dates);

            Assert.AreEqual(expected, result);
        }
        [Test]
        public void Fee_Can_Exceed_Maxiumum_Fee_If_Passes_On_Diffrent_Days()
        {
            DateTime[] dates = new DateTime[] {
                new DateTime(2022, 06, 20, 08, 00, 00),
                new DateTime(2022, 06, 20, 15, 00, 00),
                new DateTime(2022, 06, 20, 18, 00, 00),
                new DateTime(2022, 06, 21, 08, 00, 00),
                new DateTime(2022, 06, 21, 15, 00, 00),
                new DateTime(2022, 06, 21, 18, 00, 00)
            };

            var expected = 68;
            var result = tollFeeCalculator.GetTollFee(testConstants.Car, dates);

            Assert.AreEqual(expected, result);
        }
    }
}
