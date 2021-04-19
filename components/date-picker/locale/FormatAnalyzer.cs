using System;
using System.Collections.Generic;
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
        private int _formatLength;

        private List<string> _separators = new();
        private List<DateTimePartialType> _partialsOrder = new();


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

        public FormatAnalyzer(string format)
        {
            _formatLength = format.Length;
            AnalyzeFormat(format);
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
                        _separators[^0] += format[i];
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
    }
}
