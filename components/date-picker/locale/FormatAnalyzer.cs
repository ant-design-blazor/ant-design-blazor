using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AntDesign.core.Extensions;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Datepicker.Locale
{
    public class FormatAnalyzer
    {
        private int _yearLength;
        private int _monthLength;
        private int _dayLength;
        private int _hourLength;
        private int _minuteLength;
        private int _secondLength;
        private int _formatLength;

        private List<string> _separators = new();
        private List<DateTimePartialType> _partialsOrder = new();
        private readonly string _anaylzerType;
        private readonly DatePickerLocale _locale;

        public enum DateTimePartialType
        {
            Nothing,
            Second,
            Minute,
            Hour,
            Day,
            Month,
            Year
        }

        public FormatAnalyzer(string format, string anaylzerType, DatePickerLocale locale)
        {
            _formatLength = format.Length;
            AnalyzeFormat(format);
            _anaylzerType = anaylzerType;
            _locale = locale;
        }

        private void AnalyzeFormat(string format)
        {
            bool? inDate = null;
            bool isLastSeparator = false;
            int partialOrder = 0;
            for (var i = 0; i < format.Length; i++)
            {
                if (format[i].IsIn('d', 'M', 'y'))
                {
                    inDate = true;
                    isLastSeparator = false;
                    _ = format[i] switch
                    {
                        'y' => Increment(ref _yearLength, ref partialOrder, DateTimePartialType.Year),
                        'M' => Increment(ref _monthLength, ref partialOrder, DateTimePartialType.Month),
                        'd' => Increment(ref _dayLength, ref partialOrder, DateTimePartialType.Day),
                        _ => throw new ArgumentException("Character not covered")
                    };
                }
                else if (format[i].IsIn('h', 'H', 'm', 's'))
                {
                    inDate = false;
                    isLastSeparator = false;
                    _ = format[i] switch
                    {
                        'h' => Increment(ref _hourLength, ref partialOrder, DateTimePartialType.Hour),
                        'H' => Increment(ref _hourLength, ref partialOrder, DateTimePartialType.Hour),
                        'm' => Increment(ref _minuteLength, ref partialOrder, DateTimePartialType.Minute),
                        's' => Increment(ref _secondLength, ref partialOrder, DateTimePartialType.Second),
                        _ => throw new ArgumentException("Character not covered")
                    };
                }
                else if (inDate is not null) //separators
                {
                    if (!isLastSeparator)
                    {
                        _separators.Add(format[i].ToString());
                    }
                    else
                    {
                        _separators[_separators.Count-1] += format[i];
                    }
                    isLastSeparator = true;
                }
            }
        }

        private bool Increment(ref int lengthValue, ref int partialOrder, DateTimePartialType partialType)
        {
            if (lengthValue == 0)
            {
                _partialsOrder.Add(partialType);
                partialOrder++;
            }
            lengthValue++;
            return true;
        }

        public bool IsFullString(string forEvaluation)
        {
            if (forEvaluation.Length < _formatLength)
                return false;

            int startPosition = 0, endingPosition;
            for (int i = 0; i < _partialsOrder.Count; i++)
            {
                if (i < _separators.Count)
                    endingPosition = forEvaluation.IndexOf(_separators[i], startPosition);
                else
                    endingPosition = forEvaluation.Length;
                //handles situation when separator was removed from date
                if (endingPosition < 0)
                    return false;
                string partial = forEvaluation.Substring(startPosition, endingPosition - startPosition);
                (int minLen, int maxLen) borders = _partialsOrder[i] switch
                {
                    DateTimePartialType.Year => (minLen: _yearLength, maxLen: 4),
                    DateTimePartialType.Month => (minLen: _monthLength, maxLen: 2),
                    DateTimePartialType.Day => (minLen: _dayLength, maxLen: 2),
                    DateTimePartialType.Hour => (minLen: _hourLength, maxLen: 2),
                    DateTimePartialType.Minute => (minLen: _minuteLength, maxLen: 2),
                    DateTimePartialType.Second => (minLen: _secondLength, maxLen: 2),
                    _ => throw new ArgumentException("Partial not covered")
                };
                //check width of the partial
                if (!(borders.minLen <= partial.Length && partial.Length <= borders.maxLen))
                    return false;
                //check if partial is pars-able and grater than 0                
                if (int.TryParse(partial, out int parsed))
                {
                    if ((parsed <= 0 && _partialsOrder[i] >= DateTimePartialType.Day)
                        || (parsed < 0 && _partialsOrder[i] < DateTimePartialType.Day))
                        return false;
                }
                else
                {
                    return false;
                }
                //check if all characters in partial are digits (exclude for example partial = "201 ")
                if (partial.Count(c => char.IsDigit(c)) != partial.Length)
                    return false;

                if (endingPosition < forEvaluation.Length)
                    startPosition = endingPosition + _separators[i].Length;
            }
            return true;
        }

        public (bool, DateTime) TryParseQuarterString(string forEvaluation,
            string separator = "-", string quarterPrefix = "Q")
        {
            var arr = forEvaluation.Split(separator);
            if (arr.Length != 2)
                return (false, default);

            if (!(arr[0].Trim().Length >= _locale.Lang.YearFormat.Length
                && arr[0].Trim().Length < 5
                && int.TryParse(arr[0], out int year)))
                return (false, default);

            if (!arr[1].StartsWith(quarterPrefix.ToUpper()) && !arr[1].StartsWith(quarterPrefix.ToLower()))
                return (false, default);

            string quarterAsString = arr[1].Substring(quarterPrefix.Length).Trim();
            if (quarterAsString.Length == 1
                && int.TryParse(quarterAsString, out int quarter)
                && quarter > 0 && quarter <= 4)
            {
                //pick first day/month of the quarter
                return (true, new DateTime(year, quarter * 3 - 2, 1));
            }

            return (false, default);
        }

        public (bool, DateTime) TryParseWeekString(string forEvaluation, string separator = "-")
        {
            var arr = forEvaluation.Split(separator);
            if (arr.Length != 2)
                return (false, default);

            if (!(arr[0].Trim().Length >= _locale.Lang.YearFormat.Length
                && arr[0].Trim().Length < 5
                && int.TryParse(arr[0], out int year)))
                return (false, default);

            if (!arr[1].EndsWith(_locale.Lang.Week))
                return (false, default);

            string weekAsString = arr[1].Substring(0, arr[1].Length - _locale.Lang.Week.Length).Trim();

            if (!(weekAsString.Length > 0 && weekAsString.Length <= 2
                && int.TryParse(weekAsString, out int week)
                && week > 0 && week < 55))
                return (false, default);

            //pick first day of the week
            var resultDate = new DateTime(year, 1, 1).AddDays(week * 7 - 7);
            if (week > 1)
            {
                int mondayOffset = (7 + (resultDate.DayOfWeek - DayOfWeek.Monday)) % 7;
                resultDate = resultDate.AddDays(-1 * mondayOffset);
            }
            //cover scenario of 54 weeks when most of times years do not have 54 weeks
            if (resultDate.Year == year)
                return (true, resultDate);

            return (false, default);
        }

        Func<string, CultureInfo, (bool, DateTime)> _converter;

        private Func<string, CultureInfo, (bool, DateTime)> Converter
        {
            get
            {
                if (_converter is null)
                {
                    Console.WriteLine("Converter setup");
                    switch (_anaylzerType)
                    {
                        case DatePickerType.Year:
                            _converter = (pickerString, currentCultureInfo) => TryParseYear(pickerString);
                            break;
                        case DatePickerType.Quarter:
                            _converter = (pickerString, currentCultureInfo) => TryParseQuarterString(pickerString);
                            break;
                        case DatePickerType.Week:
                            _converter = (pickerString, currentCultureInfo) => TryParseWeekString(pickerString);
                            break;
                        default:
                            _converter = (pickerString, currentCultureInfo) => TryParseDate(pickerString, currentCultureInfo);
                            break;
                    }
                }
                return _converter;
            }
        }

        public bool TryPickerStringConvert<TValue>(string pickerString, out TValue changeValue, CultureInfo currentCultureInfo, bool isDateTypeNullable)
        {
            var resultTuple = Converter(pickerString, currentCultureInfo);
            if (resultTuple.Item1)
            {
                return GetParsedValue(out changeValue, resultTuple.Item2, isDateTypeNullable);
            }
            changeValue = default;
            return false;
        }

        private (bool, DateTime) TryParseDate(string pickerString, CultureInfo currentCultureInfo)
        {
            if (IsFullString(pickerString))
            {
                return (BindConverter.TryConvertTo(pickerString, currentCultureInfo, out DateTime changeValue), changeValue);
            }
            return (false, default);
        }

        public (bool, DateTime) TryParseYear(string pickerString)
        {
            if (IsFullString(pickerString))
            {
                int value = int.Parse(pickerString);
                return (true, new DateTime(value, 1, 1));
            }
            return (false, default);
        }

        private bool GetParsedValue<TValue>(out TValue changeValue, DateTime foundDate, bool isDateTypeNullable)
        {
            if (isDateTypeNullable)
                changeValue = DataConvertionExtensions.Convert<DateTime?, TValue>(new DateTime?(foundDate));
            else
                changeValue = DataConvertionExtensions.Convert<DateTime, TValue>(foundDate);
            return true;
        }
    }
}
