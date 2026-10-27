// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Globalization;

namespace AntDesign
{
    /// <summary>Registration for a calendar-specific display formatter.</summary>
    public sealed class CalendarFormatterRegistration
    {
        /// <summary>
        /// Culture names handled by this formatter. Empty means every culture; a neutral culture name such as <c>zh</c> also matches its child cultures.
        /// </summary>
        public string[] CultureNames { get; init; } = Array.Empty<string>();
        public Func<System.Globalization.Calendar, CultureInfo, bool> CanFormat { get; init; }
        public Func<DateTime, string, CultureInfo, System.Globalization.Calendar, string> FormatDate { get; init; }
        public Func<DateTime, string, CultureInfo, System.Globalization.Calendar, string> FormatYear { get; init; }
        public Func<DateTime, string, CultureInfo, System.Globalization.Calendar, string> FormatMonth { get; init; }
        public Func<DateTime, System.Globalization.Calendar, CultureInfo, string> FormatDay { get; init; }
    }

    /// <summary>Resolves the formatter registered for a Calendar and CultureInfo pair.</summary>
    public static class CalendarFormatter
    {
        private static readonly object _lock = new();
        private static CalendarFormatterRegistration[] _registrations = Array.Empty<CalendarFormatterRegistration>();

        /// <summary>Registers a formatter. Newly registered formatters take precedence over previously registered formatters.</summary>
        public static void Register(CalendarFormatterRegistration registration)
        {
            if (registration == null) throw new ArgumentNullException(nameof(registration));
            if (registration.CanFormat == null) throw new ArgumentNullException(nameof(registration.CanFormat));
            if (registration.FormatDate == null) throw new ArgumentNullException(nameof(registration.FormatDate));
            if (registration.FormatYear == null) throw new ArgumentNullException(nameof(registration.FormatYear));
            if (registration.FormatMonth == null) throw new ArgumentNullException(nameof(registration.FormatMonth));
            if (registration.FormatDay == null) throw new ArgumentNullException(nameof(registration.FormatDay));
            lock (_lock)
            {
                _registrations = [registration, .. _registrations];
            }
        }

        public static string FormatDate(DateTime date, string format, CultureInfo culture, System.Globalization.Calendar calendar)
            => Find(calendar, culture)?.FormatDate(date, format, culture, calendar) ?? DateHelper.FormatDate(date, format, culture, calendar);

        public static string FormatYear(DateTime date, string format, CultureInfo culture, System.Globalization.Calendar calendar)
            => Find(calendar, culture)?.FormatYear(date, format, culture, calendar) ?? DateHelper.FormatDate(date, format, culture, calendar);

        public static string FormatMonth(DateTime date, string format, CultureInfo culture, System.Globalization.Calendar calendar)
            => Find(calendar, culture)?.FormatMonth(date, format, culture, calendar) ?? DateHelper.FormatDate(date, format, culture, calendar);

        public static string FormatDay(DateTime date, System.Globalization.Calendar calendar, CultureInfo culture)
            => Find(calendar, culture)?.FormatDay(date, calendar, culture) ?? calendar.GetDayOfMonth(date).ToString(culture);

        private static CalendarFormatterRegistration Find(System.Globalization.Calendar calendar, CultureInfo culture)
            => _registrations.FirstOrDefault(registration => registration.CanFormat(calendar, culture) && MatchesCulture(registration, culture));

        private static bool MatchesCulture(CalendarFormatterRegistration registration, CultureInfo culture)
        {
            return registration.CultureNames.Length == 0 || registration.CultureNames.Any(cultureName =>
                culture.Name.Equals(cultureName, StringComparison.OrdinalIgnoreCase)
                || culture.Name.StartsWith($"{cultureName}-", StringComparison.OrdinalIgnoreCase));
        }
    }
}
