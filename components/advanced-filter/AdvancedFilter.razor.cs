// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class AdvancedFilter<TItem> : AntDomComponentBase
    {
        /// <summary>
        /// Manually specified filterable fields. When null, fields are resolved from TItem's properties.
        /// </summary>
        [Parameter]
        public IReadOnlyList<FilterFieldDescriptor> Fields { get; set; }

        /// <summary>
        /// The current filter expression, built from user-defined conditions.
        /// </summary>
        [Parameter]
        public Expression<Func<TItem, bool>> FilterExpression { get; set; }

        [Parameter]
        public EventCallback<Expression<Func<TItem, bool>>> FilterExpressionChanged { get; set; }

        /// <summary>
        /// Fired whenever the filter conditions change.
        /// </summary>
        [Parameter]
        public EventCallback<Expression<Func<TItem, bool>>> OnFilterChanged { get; set; }

        /// <summary>
        /// The root condition node tree representing all user-defined filter conditions.
        /// </summary>
        [Parameter]
        public FilterConditionNode ConditionNode { get; set; }

        [Parameter]
        public EventCallback<FilterConditionNode> ConditionNodeChanged { get; set; }

        /// <summary>
        /// Whether to show condition group nesting (with parentheses/sub-groups).
        /// </summary>
        [Parameter]
        public bool AllowGroup { get; set; } = true;

        /// <summary>
        /// Whether to show the "Matching all/any of the conditions" header.
        /// </summary>
        [Parameter]
        public bool ShowHeader { get; set; } = true;

        [Parameter]
        public string Size { get; set; } = "small";

        [Parameter]
        public AdvancedFilterLocale Locale { get; set; }

        [Parameter]
        public FilterOptionsLocale FilterOptionsLocale { get; set; }

        [Parameter]
        public AdvancedFilterInputResolver InputResolver { get; set; }

        private FilterConditionNode _rootNode;
        private IReadOnlyList<FilterFieldDescriptor> _fieldList;
        private IReadOnlyDictionary<string, FilterFieldDescriptor> _fieldMap;
        private FilterConditionNode _lastConditionNode;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper.Add("ant-advanced-filter");

            Locale ??= new AdvancedFilterLocale();
            FilterOptionsLocale ??= new FilterOptionsLocale();
            InputResolver ??= new AdvancedFilterInputResolver();

            _fieldList = Fields ?? FilterFieldResolver.Resolve<TItem>();
            _fieldMap = _fieldList.ToDictionary(f => f.PropertyName);

            _rootNode = ConditionNode ?? FilterConditionNode.CreateGroup();
            _lastConditionNode = ConditionNode;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_fieldList == null || !ReferenceEquals(Fields, null) && !ReferenceEquals(Fields, _fieldList))
            {
                _fieldList = Fields ?? FilterFieldResolver.Resolve<TItem>();
                _fieldMap = _fieldList.ToDictionary(f => f.PropertyName);
            }

            if (ConditionNode != null && !ReferenceEquals(ConditionNode, _lastConditionNode))
            {
                _rootNode = ConditionNode;
                _lastConditionNode = ConditionNode;
            }

            Locale ??= new AdvancedFilterLocale();
            FilterOptionsLocale ??= new FilterOptionsLocale();
            InputResolver ??= new AdvancedFilterInputResolver();
        }

        /// <summary>
        /// Set the fields from outside (e.g. from a Table component).
        /// </summary>
        public void SetFields(IReadOnlyList<FilterFieldDescriptor> fields)
        {
            _fieldList = fields ?? FilterFieldResolver.Resolve<TItem>();
            _fieldMap = _fieldList.ToDictionary(f => f.PropertyName);
            StateHasChanged();
        }

        private void AddCondition()
        {
            _rootNode.Children.Add(FilterConditionNode.CreateCondition());
            NotifyChanged();
        }

        private void AddGroup()
        {
            _rootNode.Children.Add(FilterConditionNode.CreateGroup());
            NotifyChanged();
        }

        private void RemoveNode(FilterConditionNode node)
        {
            _rootNode.Children.Remove(node);
            NotifyChanged();
        }

        private void ClearAll()
        {
            _rootNode.Children.Clear();
            NotifyChanged();
        }

        private void OnLogicalOperatorChanged(TableFilterCondition value)
        {
            _rootNode.LogicalOperator = value;
            NotifyChanged();
        }

        private void OnConditionChanged()
        {
            NotifyChanged();
        }

        private void NotifyChanged()
        {
            var expression = AdvancedFilterExpressionBuilder.Build<TItem>(_rootNode, _fieldMap);

            if (FilterExpressionChanged.HasDelegate)
                FilterExpressionChanged.InvokeAsync(expression);

            if (OnFilterChanged.HasDelegate)
                OnFilterChanged.InvokeAsync(expression);

            if (ConditionNodeChanged.HasDelegate)
                ConditionNodeChanged.InvokeAsync(_rootNode);

            _lastConditionNode = _rootNode;
            StateHasChanged();
        }

        /// <summary>
        /// Build and return the current filter expression.
        /// </summary>
        public Expression<Func<TItem, bool>> BuildFilterExpression()
        {
            return AdvancedFilterExpressionBuilder.Build<TItem>(_rootNode, _fieldMap);
        }

        public FilterConditionNode GetConditionModel()
        {
            return AdvancedFilterStateSerializer.Clone(_rootNode, _fieldList);
        }

        public string SerializeConditionModel()
        {
            return AdvancedFilterStateSerializer.Serialize(_rootNode);
        }

        public void ReloadConditionModel(FilterConditionNode conditionNode, bool notifyChanged = false)
        {
            _rootNode = conditionNode ?? FilterConditionNode.CreateGroup();
            _lastConditionNode = _rootNode;

            if (notifyChanged)
            {
                NotifyChanged();
            }
            else
            {
                StateHasChanged();
            }
        }

        public void DeserializeConditionModel(string json, bool notifyChanged = false)
        {
            ReloadConditionModel(AdvancedFilterStateSerializer.Deserialize(json, _fieldList), notifyChanged);
        }
    }
}
