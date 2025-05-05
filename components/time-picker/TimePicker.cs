// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    /**
    <summary>
    <para>To select/input a time.</para>

    <h2>When To Use</h2>

    <para>By clicking the input box, you can select a time from a popup panel.</para>
    </summary>
    <inheritdoc />
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "https://gw.alipayobjects.com/zos/alicdn/h04Zsl98I/TimePicker.svg", Title = "TimePicker", SubTitle = "时间选择框")]

    public class TimePicker<TValue> : DatePicker<TValue>
    {
        public TimePicker()
        {
            Picker = DatePickerType.Time;
        }
    }
}
