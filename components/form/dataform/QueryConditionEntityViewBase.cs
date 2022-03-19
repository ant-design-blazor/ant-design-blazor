// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AntDesign.DataAnnotations;
using AntDesign.Core.Reflection;

namespace AntDesign
{
    public class ExpressionItem
    {
        public string Expression { get; set; }
        public string DisplayName { get; set; }

        public static ExpressionItem[] GetExpressionItemList<T>(params string[] properties)
        {
            Type t = typeof(T);
            var l = t.GetProperties().ToArray();
            if (properties != null && properties.Length > 0)
            {
                l = l.Where(p => properties.Any(p1 => p1 == p.Name)).ToArray();
            }
            if (l == null || l.Length == 0)
                return new ExpressionItem[] { };
            ExpressionItem[] ret = new ExpressionItem[l.Length];
            for (int i = 0; i < l.Length; i++)
            {
                ret[i] = new ExpressionItem() { Expression = l[i].Name, DisplayName = ExportFieldDisplayName(l[i]) };
            }
            return ret;
        }

        internal static string ExportFieldDisplayName(System.Reflection.PropertyInfo pi)
        {
            var eal = pi.GetCustomAttributes(typeof(DisplayAttribute), true);
            if (eal == null)
                return pi.Name;
            if (eal.Length == 0)
                return pi.Name;
            string s = ((DisplayAttribute)eal[0]).GetName();
            if (string.IsNullOrEmpty(s))
                return pi.Name;
            else
                return s;
        }
    }

    public class QueryConditionEntityViewBase
    {
        public DataBaseType DataBaseType { get; set; }

        public QueryConditionEntityViewBase()
        {
            var pl = this.GetType().GetProperties().Where(p1 => p1.PropertyType.GetInterface(typeof(IQueryConditionValueItem<>).FullName, false) != null).ToArray();
            if (pl == null || pl.Length == 0)
                return;
            try
            {
                for (int i = 0; i < pl.Length; i++)
                {
                    object value = Activator.CreateInstance(pl[i].PropertyType);
                    pl[i].SetValue(this, value, null);
                    object[] attr = pl[i].GetCustomAttributes(typeof(RangeAttribute), true);
                    if (attr != null && attr.Length > 0)
                    {
                        RangeAttribute r = (RangeAttribute)attr[0];
                        object minval = r.Minimum;
                        if (r.Minimum is string)
                            minval = ReflectUtility.Invoke(typeof(ReflectUtility), "Convert", new Type[] { typeof(string) }, new Type[] { r.OperandType }, r.Minimum);
                        ReflectUtility.SetProperty(value, "Value", minval);
                        ReflectUtility.SetProperty(value, "Value2", minval);
                        ReflectUtility.SetProperty(value, "Checked", false);
                    }
                }
            }
            catch// (Exception ex)
            {
                //no need for show exceptions.
            }
        }

        public override string ToString()
        {
            return ToString("; ");
        }
        /// <summary>
        /// 获取可描述包含条件的字符串
        /// </summary>
        /// <param name="splitconditions">各个条件之间的分隔符</param>
        /// <returns></returns>
        public string ToString(string splitconditions)
        {
            if (string.IsNullOrEmpty(splitconditions))
                splitconditions = "; ";
            StringBuilder sb = new StringBuilder();
            var pl = this.GetType().GetProperties().Where(p1 => p1.PropertyType.GetInterface(typeof(IQueryConditionValueItem<>).FullName, false) != null).ToArray();
            if (pl == null || pl.Length == 0)
                return "";
            for (int i = 0; i < pl.Length; i++)
            {
                object condition = pl[i].GetValue(this, null);
                if (!(bool)ReflectUtility.GetProperty(condition, "Checked"))
                    continue;
                char splitchar = ',';
                QueryConditionOperator operator1 = QueryConditionOperator.Equal;
                object[] qcoa = pl[i].GetCustomAttributes(typeof(QueryConditionOperatorAttribute), true);
                if (qcoa != null && qcoa.Length > 0)
                    operator1 = ((QueryConditionOperatorAttribute)qcoa[0]).Operator;
                string displayname = pl[i].Name;
                qcoa = pl[i].GetCustomAttributes(typeof(DisplayAttribute), true);
                if (qcoa != null && qcoa.Length > 0)
                    displayname = ((DisplayAttribute)qcoa[0]).GetName();
                object value = ReflectUtility.GetProperty(condition, "Value");
                object value2 = ReflectUtility.GetProperty(condition, "Value2");
                if (pl[i].PropertyType.IsSubTypeOf(typeof(QueryConditionMultiItem<>)))
                {
                    splitchar = (char)ReflectUtility.GetProperty(condition, "SplitCharInDB");
                    operator1 = QueryConditionOperator.In;
                }
                if (pl[i].PropertyType.IsSubTypeOf(typeof(IQueryConditionRangeItem<>)))
                {
                    operator1 = QueryConditionOperator.Between;
                }
                if (pl[i].PropertyType == typeof(QueryConditionExpressionItem))
                {
                    operator1 = ((QueryConditionExpressionItem)condition).Operator;
                }

                switch (operator1)
                {
                    case QueryConditionOperator.Equal:
                        sb.Append(displayname);
                        sb.Append("=");
                        sb.Append(GetObjectString(value));
                        sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.NotEqual:
                        sb.Append(displayname);
                        sb.Append("≠");
                        sb.Append(GetObjectString(value));
                        sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.BiggerOrEqual:
                        sb.Append(displayname);
                        sb.Append(">=");
                        sb.Append(GetObjectString(value));
                        sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.BiggerThan:
                        sb.Append(displayname);
                        sb.Append(">");
                        sb.Append(GetObjectString(value));
                        sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.BitAnd:
                        sb.Append(displayname);
                        sb.Append("&");
                        sb.Append(GetObjectString(value));
                        sb.Append("=");
                        sb.Append(displayname);
                        sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.BitOr:
                        sb.Append(displayname);
                        sb.Append("|");
                        sb.Append(GetObjectString(value));
                        sb.Append("=");
                        sb.Append(displayname);
                        break;
                    case QueryConditionOperator.Contain:
                        sb.Append(displayname);
                        sb.Append(" Contain ");
                        sb.Append(GetObjectString(value));
                        sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.NotContain:
                        sb.Append(displayname);
                        sb.Append(" Not Contain ");
                        sb.Append(GetObjectString(value));
                        sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.LessOrEqual:
                        sb.Append(displayname);
                        sb.Append("<=");
                        sb.Append(GetObjectString(value));
                        sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.LessThan:
                        sb.Append(displayname);
                        sb.Append("<");
                        sb.Append(GetObjectString(value));
                        sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.In:
                        sb.Append(displayname);
                        sb.Append(" In (");
                        string[] valuelist = (value ?? "").ToString().Split(splitchar);
                        for (int j = 0; j < valuelist.Length; j++)
                        {
                            sb.Append(GetObjectString(valuelist[j].Trim()));
                            if (j != valuelist.Length - 1)
                            {
                                sb.Append(splitchar);
                                sb.Append(' ');
                            }
                        }
                        sb.Append(")");
                        sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.Between:
                        sb.Append(displayname);
                        sb.Append(" Between ");
                        sb.Append(GetObjectString(value));
                        sb.Append(" ~ ");
                        sb.Append(GetObjectString(value2));
                        sb.Append(splitconditions);
                        break;
                }
            }
            return sb.ToString();
        }

        private string GetObjectString(object v)
        {
            if (v == null || v is DBNull)
                return "{NULL}";
            if (v is DateTime)
            {
                DateTime d = (DateTime)v;
                if (d.TimeOfDay.TotalSeconds < 0.1)
                    return d.ToString("yyyy-MM-dd");
                else
                    return d.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (v is decimal)
            {
                return ((decimal)v).ToString("N2");
            }
            else if (v is double)
            {
                return ((double)v).ToString("N6");
            }
            else if (v is string)
            {
                return "'" + v.ToString() + "'";
            }
            else
                return v.ToString();
        }

    }

    internal class EmptyQueryConditionEntity : QueryConditionEntityViewBase
    {
        [Display(AutoGenerateField = false)]
        [AutoGenerateBehavior(DataFormVisibility = false)]
        public QueryConditionStringItem EmptyID { get; set; }

        public EmptyQueryConditionEntity()
            : base()
        {
            EmptyID.EnforceCondition = true;
            EmptyID.Checked = false;
        }
    }


    public interface IQueryConditionItem
    {
        bool Checked { get; set; }
        bool EnforceCondition { get; set; }
    }

    public interface IQueryConditionValueItem<T> : IQueryConditionItem
    {
        T Value { get; set; }
    }
    public interface IQueryConditionRangeItem<T> : IQueryConditionValueItem<T> where T : struct
    {
        T Value2 { get; set; }
    }

    public class QueryConditionItem<T> : IQueryConditionValueItem<T>
    {
        private T _value = default(T);
        private bool _checked = false;
        private bool _enforce = false;

        public virtual T Value
        {
            get { return _value; }
            set
            {
                if (value == null && _value == null)
                    return;
                if (!object.Equals(_value, value))
                {
                    _value = value;
                    Checked = true;
                }
            }
        }

        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (_checked != value)
                {
                    _checked = value;
                }
            }
        }

        public bool EnforceCondition
        {
            get
            {
                return _enforce;
            }
            set
            {
                if (_enforce != value)
                {
                    _enforce = value;
                    if (_enforce == true)
                    {
                        _checked = value;
                    }
                }

            }
        }
    }

    public class QueryConditionRangeItem<T> : QueryConditionItem<T>, IQueryConditionRangeItem<T> where T : struct
    {
        private T _value2 = default(T);
        public virtual T Value2
        {
            get { return _value2; }
            set
            {
                if (!object.Equals(_value2, value))
                {
                    _value2 = value;
                    Checked = true;
                }
            }
        }
    }
    public class QueryConditionStringItem : QueryConditionItem<string> { }
    public class QueryConditionGuidItem : QueryConditionItem<Guid> { }
    public class QueryConditionInt32Item : QueryConditionItem<int> { }
    public class QueryConditionInt64Item : QueryConditionItem<long> { }
    public class QueryConditionDecimalItem : QueryConditionItem<decimal> { }
    public class QueryConditionDateTimeItem : QueryConditionItem<DateTime> { }
    public class QueryConditionDoubleItem : QueryConditionItem<double> { }
    public class QueryConditionInt32RangeItem : QueryConditionRangeItem<int> { }
    public class QueryConditionInt64RangeItem : QueryConditionRangeItem<long> { }
    public class QueryConditionDecimalRangeItem : QueryConditionRangeItem<decimal> { }
    public class QueryConditionDateTimeRangeItem : QueryConditionRangeItem<DateTime> { }
    public class QueryConditionDoubleRangeItem : QueryConditionRangeItem<double> { }
    public abstract class QueryConditionMultiItem<T> : QueryConditionItem<string>
    {
        public char SplitCharInDB { get; set; }

        public System.Collections.Generic.IEnumerable<T> Values { get; set; }

        public override string Value
        {
            get
            {
                if (Values == null || !Values.Any())
                {
                    return "";
                }
                else
                {
                    return string.Join(this.SplitCharInDB, this.Values.Select(f => f?.ToString()));
                }
            }
            set => this.Values = value?.Split(this.SplitCharInDB).Where(f => !string.IsNullOrEmpty(f) && this.CheckOneItemValid(f)).Select(f => ReflectUtility.Convert<T>(f)).ToList();
        }
        public QueryConditionMultiItem()
            : base()
        {
            SplitCharInDB = ',';
            //TODO
            if (typeof(T) != typeof(string) && typeof(T) != typeof(int) && typeof(T) != typeof(Guid))
            {
                throw new ArgumentOutOfRangeException("QueryConditionMultiItem<> currently only support string/int/guid.");
            }
        }

        protected abstract bool CheckOneItemValid(string s);
    }
    public class QueryConditionMultiStringItem : QueryConditionMultiItem<string>
    {
        protected override bool CheckOneItemValid(string s)
        {
            if (s.Contains('\''))
                return false;
            return true;
        }
    }
    public class QueryConditionMultiValueItem<T> : QueryConditionMultiItem<T> where T : struct
    {
        protected override bool CheckOneItemValid(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            return ReflectUtility.CanConvert<T>(s);
        }
    }
    public class QueryConditionExpressionItem : QueryConditionStringItem
    {
        private QueryConditionExpressionType _type = QueryConditionExpressionType.Property;
        public QueryConditionExpressionType Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                }
            }
        }
        public QueryConditionOperator[] Operators { get; set; }

        internal QueryConditionOperator Operator { get; set; }
        public ExpressionItem[] Expressions { get; set; }
    }
}
