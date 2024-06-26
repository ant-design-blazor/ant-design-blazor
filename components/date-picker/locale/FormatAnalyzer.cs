using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
        private int _amPmDesignatorLength;
        private int _formatLength;

        private List<string> _separators = new();
        private List<DateTimePartialType> _partialsOrder = new();
        private Dictionary<DateTimePartialType, int> _parsedMap;
        private readonly string _analyzerType;
        private bool _hasPrefix;
        private int _startPosition;
        private int _separatorPrefixOffset;
        private readonly DatePickerLocale _locale;
        private readonly CultureInfo _cultureInfo;

        public enum DateTimePartialType
        {
            Nothing,
            Second,
            Minute,
            Hour,
            Day,
            Month,
            Year,
            AmPmDesignator
        }

        public FormatAnalyzer(string format, string analyzerType, DatePickerLocale locale, CultureInfo cultureInfo)
        {
            _formatLength = format.Length;
            _analyzerType = analyzerType;
            _locale = locale;
            _cultureInfo = cultureInfo;
            //Quarter and Week have individual approaches, so no need to analyze format
            //if (!(_analyzerType == DatePickerType.Quarter || _analyzerType == DatePickerType.Week))
            AnalyzeFormat(format);
        }

        private void AnalyzeFormat(string format)
        {
            _parsedMap = new();
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
                else if (format[i].IsIn('h', 'H', 'm', 's', 't'))
                {
                    inDate = false;
                    isLastSeparator = false;
                    _ = format[i] switch
                    {
                        'h' => Increment(ref _hourLength, ref partialOrder, DateTimePartialType.Hour),
                        'H' => Increment(ref _hourLength, ref partialOrder, DateTimePartialType.Hour),
                        'm' => Increment(ref _minuteLength, ref partialOrder, DateTimePartialType.Minute),
                        's' => Increment(ref _secondLength, ref partialOrder, DateTimePartialType.Second),
                        't' => Increment(ref _amPmDesignatorLength, ref partialOrder, DateTimePartialType.AmPmDesignator),
                        _ => throw new ArgumentException("Character not covered")
                    };
                }
                else //separators
                {
                    if (!isLastSeparator)
                    {
                        _separators.Add(format[i].ToString());
                        if (inDate is null)
                            _hasPrefix = true;
                    }
                    else
                    {
                        _separators[_separators.Count - 1] += format[i];
                    }
                    isLastSeparator = true;
                }
            }
            if (_hasPrefix)
            {
                _startPosition = _separators[0].Length;
                _separatorPrefixOffset = _hasPrefix ? 1 : 0;
            }
        }

        private bool Increment(ref int lengthValue, ref int partialOrder, DateTimePartialType partialType)
        {
            if (lengthValue == 0)
            {
                _parsedMap.Add(partialType, 0);
                _partialsOrder.Add(partialType);
                partialOrder++;
            }
            lengthValue++;
            return true;
        }

        public bool IsFullString(string forEvaluation)
        {
            if (string.IsNullOrEmpty(forEvaluation) || forEvaluation.Length < _formatLength)
                return false;

            int startPosition = _startPosition, endingPosition, parsed;
            for (int i = 0; i < _partialsOrder.Count; i++)
            {
                if (i < (_separators.Count - _separatorPrefixOffset))
                    endingPosition = forEvaluation.IndexOf(_separators[i + _separatorPrefixOffset], startPosition);
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
                    DateTimePartialType.AmPmDesignator => (minLen: _amPmDesignatorLength, maxLen: 2),
                    _ => throw new ArgumentException("Partial not covered")
                };
                //check width of the partial
                if (!(borders.minLen <= partial.Length && partial.Length <= borders.maxLen))
                    return false;

                if (_partialsOrder[i] == DateTimePartialType.AmPmDesignator)
                {
                    if (CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator.StartsWith(partial,
                                                                                          StringComparison.InvariantCultureIgnoreCase))
                    {
                        _parsedMap[_partialsOrder[i]] = 0;
                    }
                    else if (CultureInfo.CurrentCulture.DateTimeFormat.PMDesignator.StartsWith(partial,
                                                                                              StringComparison.InvariantCultureIgnoreCase))
                    {
                        _parsedMap[_partialsOrder[i]] = 1;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    //check if partial is pars-able and grater than 0
                    if (int.TryParse(partial, out parsed))
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
                        startPosition = endingPosition + _separators[i + _separatorPrefixOffset].Length;
                    _parsedMap[_partialsOrder[i]] = parsed;
                }
            }
            return true;
        }

        public (bool, DateTime) TryParseQuarterString(string forEvaluation,
            string separator = "-", string quarterPrefix = "Q")
        {
            var arr = forEvaluation.Split(separator);
            if (arr.Length != 2)
                return (false, default);

            if (!ExtractYearPartial(forEvaluation, arr[0], out int year))
                return (false, default);

            if (!arr[1].StartsWith(quarterPrefix.ToUpper()) && !arr[1].StartsWith(quarterPrefix.ToLower()))
                return (false, default);

            string quarterAsString = arr[1].Substring(quarterPrefix.Length).Trim();
            if (quarterAsString.Length == 1
                && int.TryParse(quarterAsString, out int quarter)
                && quarter > 0 && quarter <= 4)
            {
                //pick first day/month of the quarter
                return (true, new DateTime(year, quarter * 3 - 2, 1, _cultureInfo.Calendar));
            }

            return (false, default);
        }

        public (bool, DateTime) TryParseWeekString(string forEvaluation, string separator = "-")
        {
            var arr = forEvaluation.Split(separator);
            if (arr.Length != 2)
                return (false, default);

            if (!ExtractYearPartial(forEvaluation, arr[0], out int year))
                return (false, default);

            if (!arr[1].EndsWith(_locale.Lang.Week))
                return (false, default);

            string weekAsString = arr[1].Substring(0, arr[1].Length - _locale.Lang.Week.Length).Trim();

            if (!(weekAsString.Length > 0 && weekAsString.Length <= 2
                && int.TryParse(weekAsString, out int week)
                && week > 0 && week < 55))
                return (false, default);

            //pick first day of the week
            var resultDate = new DateTime(year, 1, 1, _cultureInfo.Calendar).AddDays(week * 7 - 7);
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

        private bool ExtractYearPartial(string forEvaluation, string partial, out int year)
        {
            year = 0;
            if (!(partial.Length >= _locale.Lang.YearFormat.Length))
                return false;

            int startPosition = _startPosition, endingPosition;
            endingPosition = forEvaluation.IndexOf(_separators[_separatorPrefixOffset][0], startPosition);
            string yearPartial = partial.Substring(startPosition, endingPosition - startPosition).Trim();
            if (!(_yearLength <= yearPartial.Length && yearPartial.Length <= 4))
                return false;

            if (!int.TryParse(yearPartial, out year))
                return false;
            return true;
        }

        private Func<string, (bool, DateTime)> _converter;

        private Func<string, (bool, DateTime)> Converter
        {
            get
            {
                if (_converter is null)
                {
                    switch (_analyzerType)
                    {
                        case DatePickerType.Year:
                            _converter = (pickerString) => TryParseYear(pickerString);
                            break;

                        case DatePickerType.Quarter:
                            _converter = (pickerString) => TryParseQuarterString(pickerString);
                            break;

                        case DatePickerType.Week:
                            _converter = (pickerString) => TryParseWeekString(pickerString);
                            break;

                        default:
                            _converter = (pickerString) => TryParseDate(pickerString);
                            break;
                    }
                }
                return _converter;
            }
        }

        public bool TryPickerStringConvert(string pickerString, out DateTime parsedValue, string mask = null)
        {
            if (!string.IsNullOrEmpty(mask) && !DateTime.TryParseExact(pickerString, mask, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                parsedValue = default;
                return false;
            }

            var resultTuple = Converter(pickerString);
            parsedValue = resultTuple.Item1 ? resultTuple.Item2 : default;
            return resultTuple.Item1;
        }

        private (bool, DateTime) TryParseDate(string pickerString)
        {
            if (IsFullString(pickerString))
            {
                return (TryBuildDateFromPartials(out DateTime result), result);
            }
            return (false, default);
        }

        private bool TryBuildDateFromPartials(out DateTime result)
        {
            result = default;
            int year = 1, month = 1, day = 1, hour = 0, minute = 0, second = 0;
            var use12hours = _parsedMap.ContainsKey(DateTimePartialType.AmPmDesignator);

            foreach (var item in _parsedMap)
            {
                switch (item.Key)
                {
                    case DateTimePartialType.Year:
                        if (item.Value < 0)
                            return false;
                        year = item.Value;
                        break;

                    case DateTimePartialType.Month:
                        if (item.Value < 1 || item.Value > 12)
                            return false;
                        month = item.Value;
                        break;

                    case DateTimePartialType.Day:
                        if (item.Value < 1 || item.Value > 31)
                            return false;
                        day = item.Value;
                        break;

                    case DateTimePartialType.Hour:
                        if (item.Value < 0 || item.Value > 23)
                            return false;
                        if (use12hours && item.Value > 12)
                            return false;
                        hour = item.Value;
                        break;

                    case DateTimePartialType.Minute:
                        if (item.Value < 0 || item.Value > 59)
                            return false;
                        minute = item.Value;
                        break;

                    case DateTimePartialType.Second:
                        if (item.Value < 0 || item.Value > 59)
                            return false;
                        second = item.Value;
                        break;

                    case DateTimePartialType.AmPmDesignator:
                        if (item.Value == 1)
                            hour += hour == 12 ? 0 : 12;
                        else if (hour == 12)
                            hour = 0;
                        break;
                }
            }
            if (DateTime.DaysInMonth(year, month) < day)
                return false;
            result = new DateTime(year, month, day, hour, minute, second, 0, _cultureInfo.Calendar, DateTimeKind.Local);
            return true;
        }

        public (bool, DateTime) TryParseYear(string pickerString)
        {
            if (IsFullString(pickerString))
            {
                return (true, new DateTime(_parsedMap[DateTimePartialType.Year], 1, 1, _cultureInfo.Calendar));
            }
            return (false, default);
        }
    }
}
