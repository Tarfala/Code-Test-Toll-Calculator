using System;

namespace TollFeeCalculator.Test.Constants
{
    public class TestConstants
    {
        public Vehicle Car { get { return new Vehicle(VehicleType.Car); }}
        public DateTime[] GeneralTollTimePasses { get { 
            return new DateTime[] {
                new DateTime(2022, 06, 17, 08, 00, 00), 
                new DateTime(2022, 06, 22, 08, 00, 00), 
                new DateTime(2022, 06, 18, 08, 00, 00) };} 
        }
    }
}
