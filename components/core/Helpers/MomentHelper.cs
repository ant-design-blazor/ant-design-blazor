using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AntDesign
{
    public static class MomentHelper
    {
        private const int DaysPerYear = 365;
        private const int DaysPerMonth = 31;
        private const int DaysPerWeek = 7;
        private const int HoursPerDay = 24;

        public static string FromNow(DateTime time)
        {
            var timespan = DateTime.Now - time;

            if (timespan.TotalDays >= DaysPerYear)
            {
                return $"{Math.Floor(timespan.TotalDays / DaysPerYear)} years ago";
            }

            if (timespan.TotalDays >= DaysPerMonth)
            {
                return $"{Math.Floor(timespan.TotalDays / DaysPerMonth)} months ago";
            }

            if (timespan.TotalDays >= DaysPerWeek)
            {
                return $"{Math.Floor(timespan.TotalDays / DaysPerWeek)} weeks ago";
            }

            if (timespan.TotalHours >= HoursPerDay)
            {
                return $"{Math.Floor(timespan.TotalDays)} days ago";
            }

            if (timespan.TotalHours >= 1)
            {
                return $"{Math.Floor(timespan.TotalHours)} hours ago";
            }

            if (timespan.TotalMinutes >= 1)
            {
                return $"{Math.Floor(timespan.TotalMinutes)} minutes ago";
            }

            return "a few seconds ago";
        }
    }
}
