using System;
using AntDesign.Internal;

namespace AntDesign
{
    /// <summary>
    /// Evaluates what values should be inserted to PickerValues (the date each
    /// picker will focus on when first shown).
    /// </summary>
    public static class RangePickerDefaults
    {
        public static void ProcessDefaults<TValue>(TValue value, TValue defaultValue,
            TValue defaultPickerValue, DateTime[] pickerValues, bool[] useDefaultPickerValue)
        {
            var isNullable = InternalConvert.IsNullable<TValue>();

            var defaultElementValue = Activator.CreateInstance(typeof(TValue).GetElementType());

            DateTime?[] evaluatedPickerValue = new DateTime?[2];

            var valueTemp = InternalConvert.ToDateTimeArray(value);
            var defaultTemp = InternalConvert.ToDateTimeArray(defaultValue);

            if (isNullable)
            {
                if (defaultPickerValue != null)
                {
                    evaluatedPickerValue = InternalConvert.ToDateTimeArray(defaultPickerValue);
                }
            }
            else
            {
                if (defaultPickerValue != null)
                {
                    evaluatedPickerValue = InternalConvert.ToDateTimeArray(defaultPickerValue);
                }
                else
                {
                    evaluatedPickerValue = new DateTime?[] { default(DateTime), default(DateTime) };
                }
            }

            useDefaultPickerValue[0] = EvaluateDefault(0, defaultElementValue, isNullable, evaluatedPickerValue, valueTemp, defaultTemp, pickerValues);
            useDefaultPickerValue[1] = EvaluateDefault(1, defaultElementValue, isNullable, evaluatedPickerValue, valueTemp, defaultTemp, pickerValues);

            if (useDefaultPickerValue[0])
            {
                pickerValues[0] = evaluatedPickerValue[0] ?? pickerValues[0];
            }

            if (useDefaultPickerValue[1])
            {
                pickerValues[1] = evaluatedPickerValue[1] ?? pickerValues[1];
            }
        }

        private static bool EvaluateDefault(int index, object defaultElementValue, bool isNullable,
            DateTime?[] evaluatedPickerValue, Array valueTemp, Array defaultTemp, DateTime[] pickerValues)
        {
            if (evaluatedPickerValue[index].Equals(defaultElementValue))
            {
                if (valueTemp != null && !(valueTemp.GetValue(index)?.Equals(defaultElementValue) ?? true))
                {
                    evaluatedPickerValue[index] = (DateTime?)valueTemp.GetValue(index);
                }
                else if (defaultTemp != null && !(defaultTemp.GetValue(index)?.Equals(defaultElementValue) ?? true))
                {
                    evaluatedPickerValue[index] = (DateTime?)defaultTemp.GetValue(index);
                }
                else if (!isNullable && valueTemp != null && (valueTemp.GetValue(index)?.Equals(defaultElementValue) ?? true))
                {
                    evaluatedPickerValue[index] = (DateTime?)valueTemp.GetValue(index);
                }
                else
                {
                    if (index == 1)
                    {
                        pickerValues[1] = pickerValues[0];
                        evaluatedPickerValue[1] = pickerValues[1];
                    }
                    return false;
                }
            }

            return true;
        }
    }
}
