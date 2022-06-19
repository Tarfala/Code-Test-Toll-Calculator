using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator
{
    public class TollFeeConstants
    {
        public readonly List<Month> tollFreeMonth = new List<Month>
        { Month.July };
        public enum Month
        {
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }
        public int MaximumFeeParDay = 60;
    }
}
