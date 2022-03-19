// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign.DataAnnotations
{
    /// <summary>
    /// set the width in DataForm   
    /// </summary>
    public class SizeInDataFormAttribute : Attribute
    {
        /// <summary>
        /// Size
        /// </summary>
        public SizeInDataForm Size { get; private set; }

        public SizeInDataFormAttribute(SizeInDataForm size)
        {
            Size = size;
        }
    }

    /// <summary>
    /// FormItem's size in DataForm
    /// </summary>
    public enum SizeInDataForm
    {
        /// <summary>
        /// Normal，due to DataForm's column count，will take the place of one cell.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Take the whole row
        /// </summary>
        FullLine = 1,
        /// <summary>
        /// Take two rows in edit mode, one row in QueryMode
        /// </summary>
        TwoLines = 2,
        /// <summary>
        /// Take four rows in edit mode, one row in QueryMode
        /// </summary>
        FourLines = 4
    }


    /// <summary>
    /// For Selector componments, set DataSource，LableName/ValueName
    /// </summary>
    public class DataSourceBindAttribute : Attribute
    {
        public string LabelPath { get; private set; }
        public string ValuePath { get; private set; }
        public string DataSourcePath { get; private set; }
        public Type BindType { get; private set; }

        public DataSourceBindAttribute(Type bindType, string dataSourcePath, string labelPath, string valuePath)
        {
            if (bindType == null && string.IsNullOrEmpty(dataSourcePath))
                throw new ArgumentNullException("DataSourceBindAttribute need a bindType and/or dataSourcePath. (Null dataSourcePath=bind BindType itself.)");
            BindType = bindType;
            LabelPath = labelPath;
            ValuePath = valuePath;
            DataSourcePath = dataSourcePath;
            if (bindType != null && string.IsNullOrEmpty(dataSourcePath))
            {
                if (bindType.GetInterface(typeof(System.Collections.IEnumerable).FullName, false) == null)
                    throw new ArgumentException("If DataSourceBindAttribute.BindType is a non-IEnumerable type, you have to specifies dataSourcePath property.");
            }
        }
        /// <summary>
        /// Only defined the DataSource(for the IEnumerable[string] datasource.)
        /// </summary>
        /// <param name="bindType">Type include the datasouce collection</param>
        /// <param name="dataSourcePath">The property in <see cref="BindType"/> which is a datasouce collection</param>
        public DataSourceBindAttribute(Type bindType, string dataSourcePath)
            : this(bindType, dataSourcePath, null, null)
        {

        }
    }

    public class QueryConditionOperatorAttribute : Attribute
    {
        public QueryConditionOperator Operator { get; private set; }
        public QueryConditionOperatorAttribute(QueryConditionOperator op)
        {
            Operator = op;
            if (op == QueryConditionOperator.None)
            {
                throw new Exception("Invalid QueryConditionOperator.None;");
            }
        }
    }

    public class AutoGenerateBehaviorAttribute : Attribute
    {
        public bool DataFormVisibility { get; set; } = true;
        public bool DataFormEnabled { get; set; } = true;
        public bool DynamicTableVisibility { get; set; } = true;
        public bool DiffFormVisibility { get; set; } = true;
    }

    public class SortAbilityAttribute : Attribute
    {
        public QuerySortIn DefaultSortMode { get; set; } = QuerySortIn.Ascending;
        /// <summary>
        /// Smaller number indicate higher priority.
        /// </summary>
        public int Priority { get; set; } = 1;
        /// <summary>
        /// Is default sort property or not.
        /// </summary>
        public bool DefaultSortField { get; set; } = false;
        /// <summary>
        /// Is this property support sort in EFCore?
        /// </summary>
        public bool Sortable { get; set; } = true;
    }
}
