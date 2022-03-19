// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// 用于保存对比内容的实体类
    /// </summary>
    public class CompareValueEntity 
    {
        /// <summary>
        /// 状态改变时触发
        /// </summary>
        public EventCallback StatusChanged { get; set; }

        private string _propertyname;
        private string _displayname;
        private object _value1;
        private object _value2;
        private bool _match;
        private string _format;

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName
        {
            get { return _propertyname; }
            set
            {
                if (_propertyname != value)
                {
                    _propertyname = value;
                }
            }
        }

        /// <summary>
        /// 显示的名称
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(_displayname))
                    return PropertyName;
                else
                    return _displayname;
            }
            set
            {
                if (_displayname != value)
                {
                    _displayname = value;
                }
            }
        }
        /// <summary>
        /// 比较的值1，当前值
        /// </summary>
        public object Value1
        {
            get { return _value1; }
            set
            {
                if (_value1 != value)
                {
                    _value1 = value;
                    Compare();
                }
            }
        }
        /// <summary>
        /// 比较的值2，被修改前的值
        /// </summary>
        public object Value2
        {
            get { return _value2; }
            set
            {
                if (_value2 != value)
                {
                    _value2 = value;
                    Compare();
                }
            }
        }
        /// <summary>
        /// 获取Value1和2是否相等
        /// </summary>
        public bool Match
        {
            get { return _match; }
            private set
            {
                if (_match != value)
                {
                    _match = value;
                    OnStatusChanged();
                }
            }
        }

        public int DisplayOrder { get; set; }

        public IValueConverter FormatConverter { get; set; }
        public string Format { get; set; }

        private string GetPropertyStringValue(object value)
        {
            if (FormatConverter != null)
            {
                return (FormatConverter.Convert(value, typeof(string), null, System.Globalization.CultureInfo.CurrentUICulture) ?? "").ToString();
            }


            if (!string.IsNullOrEmpty(Format))
            {
                return string.Format("{0:" + Format + "}", value);
            }

            return (value ?? "").ToString();
        }

        private void Compare()
        {
            if (Value1 == null && Value2 == null)
            {
                Match = true;
            }
            else if (Value1 != null && Value2 != null)
            {
                if (Value1.GetType() != Value2.GetType())
                    Match = false;
                else
                {
                    if (Value1 is IComparable)
                    {
                        Match = (((IComparable)Value1).CompareTo(Value2) == 0);
                    }
                    else
                        Match = (Value1 == Value2);
                }
            }
            else
            {
                Match = false;
            }
        }
        /// <summary>
        /// 获取被比较的值1，当前值
        /// </summary>
        /// <returns></returns>
        public string GetValue1()
        {
            return GetValue(Value1);
        }
        /// <summary>
        /// 获取被比较的值2，被修改前的值
        /// </summary>
        /// <returns></returns>
        public string GetValue2()
        {
            return GetValue(Value2);
        }

        private string GetValue(object value)
        {
            if (value == null)
                return "{NULL}";
            else
            {
                return GetPropertyStringValue(value);
            }
        }

        private void OnStatusChanged()
        {
            if (StatusChanged.HasDelegate)
            {
                StatusChanged.InvokeAsync(null);
            }
        }

    }

    /// <summary>
    /// 定义一个属性，包含当前对象需要对比的另一个实体对象的实例
    /// </summary>
    public interface ICompareEntityProperty
    {
        [Display(AutoGenerateField = false)]
        object CompareEntity { get; set; }
    }

}
