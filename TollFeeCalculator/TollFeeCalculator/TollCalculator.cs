using PublicHoliday;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using TollFeeCalculator;

public class TollCalculator
{
    readonly TollFeeConstants constants = new TollFeeConstants();
    /// <summary>
    /// Calculates the fee for a single vehicles toll passages. Works with single or mutiple days.
    /// </summary>
    /// <param name="vehicle">The vehicle passing the toll</param>
    /// <param name="tollPasses">Datetime of all toll passage(s)</param>
    /// <returns></returns>

    public decimal GetTollFee(Vehicle vehicle, DateTime[] tollPasses)
    {
        if (tollPasses == null || tollPasses.Length == 0 || 
           (vehicle == null || vehicle.VehicleIsTollFree)) return 0;

        IList<DateTime> tollPassesDateSorted = tollPasses.OrderBy(tollpass => tollpass.Date).ToList();
        IList<DateTime> tollPassesThatWillHaveFee = RemoveTollFreePasses(tollPassesDateSorted);
        IList<TollPassageAndFee> tollPassageAndFee = GetTollPassageAndFee(tollPassesThatWillHaveFee);
        return GetTollFeeThatWillBeCharged(tollPassageAndFee);

    }
    private IList<DateTime> RemoveTollFreePasses(IList<DateTime> tollPasses)
    {
        tollPasses = RemoveTollFreeDay(tollPasses);
        tollPasses = RemoveTollFreeTime(tollPasses);
        return tollPasses;
    }
    private IList<DateTime> RemoveTollFreeDay(IList<DateTime> tollPasses)
    {
        return tollPasses.Where(tollPass => !IsTollFreeDate(tollPass)).ToList();
    }
    // Using the PublicHoliday nougat to check if the datetime is during a public holiday, etc.
    // Reference: https://github.com/martinjw/Holiday
    // License: https://github.com/martinjw/Holiday/blob/master/license.txt
    public bool IsTollFreeDate(DateTime tollPass)
    {
        string tollPassMonth = tollPass.ToString("MMMM", new CultureInfo("en-US"));
        if (constants.tollFreeMonth.Any(month => month.ToString() == tollPassMonth)) return true;

        SwedenPublicHoliday publicHoliday = new SwedenPublicHoliday();
        if (publicHoliday.IsPublicHoliday(tollPass) ||
            publicHoliday.IsPublicHoliday(tollPass.AddDays(1)) ||
            !publicHoliday.IsWorkingDay(tollPass)) return true;

        return false;
    }
    private IList<DateTime> RemoveTollFreeTime(IList<DateTime> tollPasses)
    {
        return tollPasses.Where(tollPass => !IsTollFreeTime(tollPass)).ToList();
    }
    public bool IsTollFreeTime(DateTime tollPass)
    {
        var tollFeeStartTime = new TimeSpan(06, 00, 00);
        var tollFeeEndTime = new TimeSpan(18, 30, 00);
        if (tollPass.TimeOfDay < tollFeeStartTime || tollPass.TimeOfDay >= tollFeeEndTime) return true;
        return false;
    }
    private IList<TollPassageAndFee> GetTollPassageAndFee(IList<DateTime> tollPassesThatWillHaveFee)
    {
        return tollPassesThatWillHaveFee.Select(tollPassage => new TollPassageAndFee
        {
            TollPassage = tollPassage,
            Fee = GetFeeForTollPass(tollPassage)
        }).ToList();
    }
    private decimal GetFeeForTollPass(DateTime tollPassage)
    {
        Regex rgx = new Regex("[^0-9]");
        // Removes everything except numbers
        decimal passingTime = int.Parse(rgx.Replace(tollPassage.TimeOfDay.ToString(), ""));

        if (060000 <= passingTime && passingTime < 063000) return 8;
        if (063000 <= passingTime && passingTime < 070000) return 13;
        if (070000 <= passingTime && passingTime < 080000) return 18;
        if (080000 <= passingTime && passingTime < 083000) return 13;
        if (083000 <= passingTime && passingTime < 150000) return 8;
        if (150000 <= passingTime && passingTime < 153000) return 13;
        if (153000 <= passingTime && passingTime < 170000) return 18;
        if (170000 <= passingTime && passingTime < 180000) return 13;
        if (180000 <= passingTime && passingTime < 183000) return 8;
        return 0;
    }
    private decimal GetTollFeeThatWillBeCharged(IList<TollPassageAndFee> tollPassageAndFee)
    {
        decimal totalFee = 0;
        while (tollPassageAndFee.Count != 0)
        {
            IList<TollPassageAndFee> tollPassesAndFeeSameDay = GetSameDayPassagesAsFirst(tollPassageAndFee);
            tollPassageAndFee = tollPassageAndFee.Except(tollPassesAndFeeSameDay).ToList();
            totalFee += CalculateSingeDayFee(tollPassesAndFeeSameDay);
        }

        return totalFee;
    }
    private IList<TollPassageAndFee> GetSameDayPassagesAsFirst(IList<TollPassageAndFee> tollPassageAndFee)
    {
        return tollPassageAndFee.Where(tollPass => tollPass.TollPassage.Date == tollPassageAndFee[0].TollPassage.Date).ToList();
    }

    private decimal CalculateSingeDayFee(IList<TollPassageAndFee> tollPassagesToHandle)
    {
        decimal totalFeeForOneDay = 0;
        decimal maximumFeeForOneDay = constants.MaximumFeeParDay;
        var sixtyMinutes = new TimeSpan(1, 00, 00);

        while (tollPassagesToHandle.Count != 0)
        {
            IList<TollPassageAndFee> passesWithin60Minutes = tollPassagesToHandle.Where(allPassesSameDay => allPassesSameDay.TollPassage < tollPassagesToHandle[0].TollPassage + sixtyMinutes).ToList();
            tollPassagesToHandle = tollPassagesToHandle.Except(passesWithin60Minutes).ToList();
            totalFeeForOneDay += passesWithin60Minutes.Max(tollPassage => tollPassage.Fee);
        }
        return totalFeeForOneDay <= maximumFeeForOneDay ? totalFeeForOneDay : maximumFeeForOneDay;
    }
}