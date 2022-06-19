using System;

namespace TollFeeCalculator
{
    internal class Program
    {    
        // TODO JR: Remove Main and Program.cs, only used for testing purpose before tests are added
        static void Main(string[] args)
        {
            DateTime[] dates = new DateTime[] {
                new DateTime(2022, 06, 17, 08, 00, 00),
                new DateTime(2022, 06, 18, 08, 00, 00)         
            };
            Car vehicle = new Car();
            int result = new TollCalculator().GetTollFee(vehicle, dates);
            Console.ReadLine();
        }
    }
}
