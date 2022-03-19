// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using AntDesign.DataAnnotations;
using AntDesign.Core.Reflection;

namespace AntDesign
{
    public enum DataBaseType
    {
        //TODO, MySQL and the others
        Default = 0, SQLServer = 0, Oracle = 1, MySQL = 2
    }

    [DataContract(Name = "QueryConditions", IsReference = true)]
    public class QueryConditions
    {
        [DataMember()]
        public int PageIndex { get; set; }
        //public int PageCount { get; }
        //public int RecordCount { get; protected set; }
        [DataMember()]
        public int PageSize { get; set; }
        [DataMember()]
        public System.Collections.ObjectModel.ObservableCollection<QueryOrderByProperty> OrderBy { get; private set; }
        [DataMember()]
        public QuerySubConditions Conditions { get; private set; }
        public QueryConditions(QueryConditionEntityViewBase conditionentity, int pageindex, int pagesize)
        {
            PageIndex = pageindex;
            PageSize = pagesize;
            Conditions = new QuerySubConditions();
            OrderBy = new System.Collections.ObjectModel.ObservableCollection<QueryOrderByProperty>();
            if (conditionentity != null)
            {
                var pl = conditionentity.GetType().GetProperties().Where(p1 => p1.PropertyType.GetInterface(typeof(IQueryConditionValueItem<>).FullName, false) != null).ToArray();
                if (pl != null && pl.Length > 0)
                {
                    for (int i = 0; i < pl.Length; i++)
                    {
                        object val = pl[i].GetValue(conditionentity, null);
                        if (val != null)
                        {
                            IQueryConditionItem item1 = (IQueryConditionItem)val;
                            if (item1.Checked)
                            {
                                QueryCondition qc = new QueryCondition() { Checked = item1.Checked, Property = pl[i].Name };
                                QueryConditionOperatorAttribute optattr = (QueryConditionOperatorAttribute)Attribute.GetCustomAttribute(pl[i], typeof(QueryConditionOperatorAttribute));
                                qc.Operator = QueryConditionOperator.Equal;
                                qc.Value = ConvertAs(ReflectUtility.GetProperty(val, "Value"), conditionentity.DataBaseType);
                                if (qc.Value is Enum)
                                {
                                    qc.Value = ConvertAs((int)qc.Value, conditionentity.DataBaseType);
                                }
                                DisplayAttribute displayattb = (DisplayAttribute)Attribute.GetCustomAttribute(pl[i], typeof(DisplayAttribute));
                                if (displayattb != null)
                                    qc.DisplayName = displayattb.GetName();
                                else
                                    qc.DisplayName = pl[i].Name;
                                if (pl[i].PropertyType.GetInterface(typeof(IQueryConditionRangeItem<>).FullName, false) != null)
                                {
                                    qc.Operator = QueryConditionOperator.Between;
                                    qc.Value2 = ConvertAs(ReflectUtility.GetProperty(val, "Value2"), conditionentity.DataBaseType);
                                }
                                if (pl[i].PropertyType.IsSubTypeOf(typeof(QueryConditionMultiItem<>)))
                                {
                                    //qc.Value =FatReflectUtility.Invoke(item1, "GetOrganizedValue", null);
                                    qc.Operator = QueryConditionOperator.In;
                                }
                                if (pl[i].PropertyType == typeof(QueryConditionExpressionItem))
                                {
                                    if (qc.Value == null || !(qc.Value is string) || (string.IsNullOrEmpty((string)qc.Value)))
                                        throw new ArgumentException("QueryConditionExpressionItem should have a non-empty string value. (Field:" + qc.Property + ")");
                                    qc.Value = new QueryConditionExpression() { Expression = (string)qc.Value, Type = ((QueryConditionExpressionItem)item1).Type };
                                    qc.Operator = ((QueryConditionExpressionItem)val).Operator;
                                }
                                if (optattr != null)
                                    qc.Operator = optattr.Operator;
                                if (qc.Operator != QueryConditionOperator.None)
                                    Conditions.Add(qc);
                            }
                        }
                    }
                }
            }
        }

        public QueryConditions()
        {
            PageIndex = -1;
            PageSize = -1;
            Conditions = new QuerySubConditions();
            OrderBy = new System.Collections.ObjectModel.ObservableCollection<QueryOrderByProperty>();
        }
        private static object ConvertAs(object val, DataBaseType type)
        {
            if (type == DataBaseType.SQLServer)
                return val;
            else if (type == DataBaseType.Oracle)
            {
                if (val is int || val is long || val is float || val is double)
                    return Convert.ToDecimal(val);
                if (val is bool)
                    return ((bool)val) ? 1m : 0m;
            }
            return val;
        }

        public override string ToString()
        {
            return ToString(null);
        }

        public string ToString(string splitconditions)
        {
            if (Conditions == null)
                return "";
            return Conditions.GetString(splitconditions);

        }

    }

    [DataContract(Name = "QueryCondition", IsReference = true)]
    [KnownType(typeof(QueryConditionExpression))]
    [KnownType(typeof(QueryConditionExpressionType))]
    public class QueryCondition
    {
        [DataMember()]
        public bool Checked { get; set; }
        [DataMember()]
        public string Property { get; set; }
        [DataMember()]
        public string DisplayName { get; set; }
        [DataMember()]
        public object Value { get; set; }
        [DataMember()]
        public QueryConditionOperator Operator { get; set; }
        [DataMember()]
        public object Value2 { get; set; }
        [DataMember()]
        public string Target { get; set; }

        [DataMember()]
        public QuerySubConditions SubConditions { get; private set; }

        public QueryCondition()
        {
            SubConditions = new QuerySubConditions();// { SubConditonsRelation = ConditionRelation.AND };
        }
    }

    [DataContract(Name = "QueryOrderByProperty", IsReference = true)]
    public class QueryOrderByProperty
    {
        private string _displayname = null;
        public bool Checked
        {
            get; set;
        }

        [DataMember()]
        public string Property { get; set; }
        [DataMember()]
        public int Priority { get; set; }
        [DataMember()]
        public QuerySortIn Sort { get; set; }
        [DataMember()]
        public string DisplayName
        {
            get { return string.IsNullOrEmpty(_displayname) ? this.Property : _displayname; }
            set
            {
                if (_displayname == this.Property)
                {
                    _displayname = null;
                }
                else
                {
                    _displayname = value;
                }
            }
        }


        public override int GetHashCode()
        {
            int t = 0;
            if (Property != null)
                t += Property.GetHashCode();
            t = t ^ Priority;
            t = t ^ (int)Sort;
            return t;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is QueryOrderByProperty))
                return false;
            var o = (QueryOrderByProperty)obj;
            if (!string.Equals(Property, o.Property))
                return false;
            if (Priority != o.Priority)
                return false;
            if (Sort != o.Sort)
                return false;
            return true;
        }

        public static bool operator ==(QueryOrderByProperty p1, QueryOrderByProperty p2)
        {
            if ((object)p1 == null && (object)p2 == null)
                return true;
            if ((object)p1 != null)
                return p1.Equals((object)p2);
            else
                return false;
        }

        public static bool operator !=(QueryOrderByProperty p1, QueryOrderByProperty p2)
        {
            return !(p1 == p2);
        }

        private static QueryOrderByProperty[] GetOrderByProperties(Type t)
        {
            if (t == null)
                return null;
            System.Reflection.PropertyInfo[] pl = null;
            if (t.IsSubTypeOf(typeof(QueryConditionEntityViewBase)))
                pl = t.GetProperties().Where(p1 => p1.PropertyType.GetInterface(typeof(IQueryConditionValueItem<>).FullName, false) != null).ToArray();
            else
                pl = t.GetProperties().Where(p1 => p1.PropertyType.IsValueType || p1.PropertyType == typeof(string)).ToArray();
            List<QueryOrderByProperty> l = new List<QueryOrderByProperty>();
            if (pl != null && pl.Length > 0)
            {
                for (int i = 0; i < pl.Length; i++)
                {
                    QueryOrderByProperty orderby = new QueryOrderByProperty() { Checked = false, Priority = i + 1, Property = pl[i].Name, Sort = QuerySortIn.Ascending };
                    DisplayAttribute displayattb = (DisplayAttribute)Attribute.GetCustomAttribute(pl[i], typeof(DisplayAttribute));
                    SortAbilityAttribute saa = (SortAbilityAttribute)Attribute.GetCustomAttribute(pl[i], typeof(SortAbilityAttribute));
                    if (saa != null && !saa.Sortable)
                        continue;
                    if (displayattb != null)
                        orderby.DisplayName = displayattb.GetName();
                    else
                        orderby.DisplayName = pl[i].Name;
                    if (saa != null && saa.Sortable)
                    {
                        orderby.Checked = saa.DefaultSortField;
                        orderby.Priority = saa.Priority;
                        orderby.Sort = saa.DefaultSortMode;
                    }

                    l.Add(orderby);
                }
            }
            return l.ToArray();
        }

        public static QueryOrderByProperty[] GetOrderByPropertiesByConditions(QueryConditionEntityViewBase conditionentity)
        {
            if (conditionentity == null)
                return new QueryOrderByProperty[] { };
            return GetOrderByProperties(conditionentity.GetType());
        }

        public static QueryOrderByProperty[] GetOrderByPropertiesByConditions<T>() where T : QueryConditionEntityViewBase
        {
            return GetOrderByProperties(typeof(T));
        }
    }


    [DataContract(Name = "QuerySubConditions", IsReference = true)]
    public class QuerySubConditions : ICollection<QueryCondition>
    {
        public QuerySubConditions()
        {
            List = new System.Collections.ObjectModel.ObservableCollection<QueryCondition>();
        }

        [DataMember()]
        public System.Collections.ObjectModel.ObservableCollection<QueryCondition> List { get; private set; }

        [DataMember()]
        public ConditionRelation SubConditonsRelation { get; set; }

        public void AddRange(System.Collections.Generic.IEnumerable<QueryCondition> collection)
        {
            if (collection == null || !collection.Any())
                return;
            foreach (var c in collection)
            {
                if (c != null)
                    Add(c);
            }
        }

        public string GetString(string splitconditions)
        {
            if (string.IsNullOrEmpty(splitconditions))
                splitconditions = " " + SubConditonsRelation.ToString() + " ";
            StringBuilder sb = new StringBuilder();
            if (Count == 0 || !this.Any(c => c.Checked))
                return "";
            var clist = this.Where(c => c.Checked).ToArray();
            for (int i = 0; i < clist.Length; i++)
            {
                switch (clist[i].Operator)
                {
                    case QueryConditionOperator.Equal:
                        sb.Append(clist[i].DisplayName);
                        sb.Append("=");
                        sb.Append(GetObjectString(clist[i].Value));
                        if (i < clist.Length - 1)
                            sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.NotEqual:
                        sb.Append(clist[i].DisplayName);
                        sb.Append("≠");
                        sb.Append(GetObjectString(clist[i].Value));
                        if (i < clist.Length - 1)
                            sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.BiggerOrEqual:
                        sb.Append(clist[i].DisplayName);
                        sb.Append(">=");
                        sb.Append(GetObjectString(clist[i].Value));
                        if (i < clist.Length - 1)
                            sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.BiggerThan:
                        sb.Append(clist[i].DisplayName);
                        sb.Append(">");
                        sb.Append(GetObjectString(clist[i].Value));
                        if (i < clist.Length - 1)
                            sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.BitAnd:
                        sb.Append(clist[i].DisplayName);
                        sb.Append("&");
                        sb.Append(GetObjectString(clist[i].Value));
                        sb.Append("=");
                        sb.Append(clist[i].DisplayName);
                        if (i < clist.Length - 1)
                            sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.BitOr:
                        sb.Append(clist[i].DisplayName);
                        sb.Append("|");
                        sb.Append(GetObjectString(clist[i].Value));
                        sb.Append("=");
                        if (i < clist.Length - 1)
                            sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.Contain:
                        sb.Append(clist[i].DisplayName);
                        sb.Append(" Contain ");
                        sb.Append(GetObjectString(clist[i].Value));
                        if (i < clist.Length - 1)
                            sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.NotContain:
                        sb.Append(clist[i].DisplayName);
                        sb.Append(" NotContain ");
                        sb.Append(GetObjectString(clist[i].Value));
                        if (i < clist.Length - 1)
                            sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.LessOrEqual:
                        sb.Append(clist[i].DisplayName);
                        sb.Append("<=");
                        sb.Append(GetObjectString(clist[i].Value));
                        if (i < clist.Length - 1)
                            sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.LessThan:
                        sb.Append(clist[i].DisplayName);
                        sb.Append("<");
                        sb.Append(GetObjectString(clist[i].Value));
                        if (i < clist.Length - 1)
                            sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.In:
                        sb.Append(clist[i].DisplayName);
                        sb.Append(" In (");
                        sb.Append(GetObjectString(clist[i].Value));
                        sb.Append(")");
                        if (i < clist.Length - 1)
                            sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.Between:
                        sb.Append(clist[i].DisplayName);
                        sb.Append(" Between ");
                        sb.Append(GetObjectString(clist[i].Value));
                        sb.Append(" ~ ");
                        sb.Append(GetObjectString(clist[i].Value2));
                        if (i < clist.Length - 1)
                            sb.Append(splitconditions);
                        break;
                    case QueryConditionOperator.NotBetween:
                        sb.Append(clist[i].DisplayName);
                        sb.Append(" NotBetween ");
                        sb.Append(GetObjectString(clist[i].Value));
                        sb.Append(" ~ ");
                        sb.Append(GetObjectString(clist[i].Value2));
                        if (i < clist.Length - 1)
                            sb.Append(splitconditions);
                        break;
                }
                if (clist[i].SubConditions.Count > 0)
                {
                    sb.Append("(");
                    sb.Append(clist[i].SubConditions.GetString(null));
                    sb.Append(")");
                    if (i < clist.Length - 1)
                        sb.Append(splitconditions);
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

        public int IndexOf(QueryCondition item)
        {
            return List.IndexOf(item);
        }

        public void Insert(int index, QueryCondition item)
        {
            List.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            List.RemoveAt(index);
        }

        public QueryCondition this[int index]
        {
            get
            {
                return List[index];
            }
            set
            {
                List[index] = value;
            }
        }

        public void Add(QueryCondition item)
        {
            List.Add(item);
        }

        public void Clear()
        {
            List.Clear();
        }

        public bool Contains(QueryCondition item)
        {
            return List.Contains(item);
        }

        public void CopyTo(QueryCondition[] array, int arrayIndex)
        {
            List.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return List.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(QueryCondition item)
        {
            return List.Remove(item);
        }

        public IEnumerator<QueryCondition> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }
    }

    [DataContract(Name = "QueryConditionOperator", IsReference = false)]
    public enum QueryConditionOperator
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Equal = 1,
        [EnumMember]
        Contain = 2,
        [EnumMember]
        BiggerThan = 4,
        [EnumMember]
        LessThan = 8,
        [EnumMember]
        BiggerOrEqual = 5,
        [EnumMember]
        LessOrEqual = 9,
        [EnumMember]
        Between = 16,
        [EnumMember]
        BitAnd = 32,
        [EnumMember]
        BitOr = 64,
        [EnumMember]
        In = 128,
        [Obsolete("Do not use directly.")]
        [EnumMember]
        Not = 256,
        [EnumMember]
        NotEqual = 257,
        [EnumMember]
        NotContain = 258,
        [EnumMember]
        NotBetween = Not + Between
    }

    [DataContract(Name = "ConditionRelation", IsReference = false)]
    public enum ConditionRelation
    {
        [EnumMember]
        AND = 0,
        [EnumMember]
        OR = 1
    }

    [DataContract(Name = "QuerySortIn", IsReference = false)]
    public enum QuerySortIn
    {
        [EnumMember]
        Default = 0,
        [EnumMember]
        Ascending = 1,
        [EnumMember]
        Descending = 2
    }

    [DataContract(Name = "QueryConditionExpressionType", IsReference = false)]
    public enum QueryConditionExpressionType
    {
        [EnumMember]
        Property = 0,
        [EnumMember]
        Expression = 1 //TODO
    }

    [DataContract(Name = "QueryConditionExpression", IsReference = true)]
    public class QueryConditionExpression
    {

        public const string TableNamePlaceholder = "$T$";
        [DataMember()]
        public string Expression { get; set; }
        [DataMember()]
        public QueryConditionExpressionType Type { get; set; }
    }
}
