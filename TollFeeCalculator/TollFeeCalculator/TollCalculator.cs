using PublicHoliday;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TollFeeCalculator;
using static TollFeeCalculator.TollFeeConstants;

public class TollCalculator
{
    readonly TollFeeConstants constants = new TollFeeConstants();
    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(Vehicle vehicle, DateTime[] tollPasses)
    {
        if (tollPasses == null || tollPasses.Length == 0 || 
           (vehicle == null || vehicle.VehicleIsTollFree)) return 0;

        IList<DateTime> tollPassesDateSorted = tollPasses.OrderBy(tollpass => tollpass.Date).ToList();
        IList<DateTime> tollPassesThatWillHaveFee = RemoveTollFreePasses(tollPassesDateSorted);


        DateTime intervalStart = tollPassesDateSorted.First();
        int totalFee = 0;
        foreach (DateTime date in tollPassesDateSorted)
        {
            int nextFee = GetTollFee(date, vehicle);
            int tempFee = GetTollFee(intervalStart, vehicle);

            long diffInMillies = date.Millisecond - intervalStart.Millisecond;
            long minutes = diffInMillies/1000/60;

            if (minutes <= 60)
            {
                if (totalFee > 0) totalFee -= tempFee;
                if (nextFee >= tempFee) tempFee = nextFee;
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;
            }
        }
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private IList<DateTime> RemoveTollFreePasses(IList<DateTime> tollPasses)
    {
        tollPasses = RemoveTollFreeDay(tollPasses);
        tollPasses = RemoveTollFreeTime(tollPasses);
        return tollPasses;
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

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        int hour = date.Hour;
        int minute = date.Minute;

        if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
        else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
        else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
        else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
        else return 0;
    }
}