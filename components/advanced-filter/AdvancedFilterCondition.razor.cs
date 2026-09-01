// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class AdvancedFilterCondition : AntDomComponentBase
    {
        [Parameter]
        public IReadOnlyList<FilterFieldDescriptor> Fields { get; set; }

        [Parameter]
        public IReadOnlyDictionary<string, FilterFieldDescriptor> FieldMap { get; set; }

        [Parameter]
        public FilterConditionNode Node { get; set; }

        [Parameter]
        public FilterOptionsLocale FilterOptionsLocale { get; set; }

        [Parameter]
        public AdvancedFilterInputResolver InputResolver { get; set; }

        [Parameter]
        public string Size { get; set; }

        [Parameter]
        public EventCallback OnChanged { get; set; }

        [Parameter]
        public EventCallback OnRemove { get; set; }

        private FilterFieldDescriptor _currentField;
        private AdvancedFilterLocale _locale;
        private TableFilter _tableFilter;
        private AdvancedFilterInputContext _inputContext;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _locale = new AdvancedFilterLocale();
            UpdateCurrentField();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            UpdateCurrentField();
        }

        private void UpdateCurrentField()
        {
            if (!string.IsNullOrEmpty(Node?.PropertyName) && FieldMap != null)
            {
                FieldMap.TryGetValue(Node.PropertyName, out _currentField);
            }
            else
            {
                _currentField = null;
            }

            // Sync a TableFilter object for the FilterInput render fragment
            _tableFilter = new TableFilter
            {
                Value = Node?.Value,
                FilterCompareOperator = Node?.CompareOperator ?? TableFilterCompareOperator.Equals,
            };

            var inputKind = (InputResolver ?? new AdvancedFilterInputResolver()).Resolve(_currentField, _tableFilter.FilterCompareOperator);
            _inputContext = new AdvancedFilterInputContext
            {
                Field = _currentField,
                Node = Node,
                InputKind = inputKind,
                TableFilter = _tableFilter,
                Options = _currentField?.ValueOptions,
                Attributes = _currentField?.FilterType?.InputAttributes ?? new(),
                ValueChanged = OnValueChanged,
                Confirm = () => _ = OnChanged.InvokeAsync(null),
            };
        }

        private void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = Node.PropertyName != propertyName;

            Node.PropertyName = propertyName;

            if (FieldMap.TryGetValue(propertyName, out var field))
            {
                _currentField = field;
                if (propertyChanged)
                {
                    Node.Value = null;
                    Node.CompareOperator = GetDefaultCompareOperator(field);
                }
            }
            else
            {
                _currentField = null;
                Node.Value = null;
            }

            _ = OnChanged.InvokeAsync(null);
        }

        private void OnOperatorChanged(TableFilterCompareOperator op)
        {
            Node.CompareOperator = op;
            if (_tableFilter != null)
            {
                _tableFilter.FilterCompareOperator = op;
            }

            if (op == TableFilterCompareOperator.Between && Node.Value != null && Node.Value.GetType().IsArray == false)
            {
                Node.Value = null;
            }

            _ = OnChanged.InvokeAsync(null);
        }

        private void OnValueChanged(object value)
        {
            Node.Value = value;
            _tableFilter.Value = value;
            _ = OnChanged.InvokeAsync(null);
        }

        private static bool IsUnaryOperator(TableFilterCompareOperator op)
        {
            return op == TableFilterCompareOperator.IsNull || op == TableFilterCompareOperator.IsNotNull;
        }

        private static TableFilterCompareOperator GetDefaultCompareOperator(FilterFieldDescriptor field)
        {
            return field?.ValueOptions?.Count > 0
                ? TableFilterCompareOperator.Equals
                : field?.FilterType?.DefaultCompareOperator ?? TableFilterCompareOperator.Equals;
        }
    }
}
