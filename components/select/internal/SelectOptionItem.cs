// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Select.Internal
{
    /// <summary>
    /// The data model for a SelectOption that is used internally.
    /// </summary>
    /// <typeparam name="TItemValue"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public class SelectOptionItem<TItemValue, TItem>
    {
        internal ElementReference Ref { get => ChildComponent?.Ref ?? default; }
        //public ForwardRef SelectedTagRefBack { get; set; } = new ForwardRef();

        /// <summary>
        /// ElementRef that will be used by <see cref="SelectContent{TItemValue, TItem}"/> only
        /// for selected Mode=Multiple and MaxTagCount=Responsive
        /// </summary>
        internal ElementReference SelectedTagRef { get; set; }

        internal decimal Width { get; set; } //TODO: rename tp Width

        internal Guid InternalId { get; set; } = Guid.NewGuid();

        private TItemValue _value;

        public TItemValue Value
        {
            get => _value;
            set
            {
                _value = value;
            }
        }

        private TItem _item;

        public TItem Item
        {
            get => _item;
            set
            {
                _item = value;
            }
        }

        private string _label = string.Empty;

        public string Label
        {
            get => _label;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _label = value;

                    if (ChildComponent != null)
                        ChildComponent.InternalLabel = value;
                }
                else
                {
                    if (!_label.Equals(value, StringComparison.InvariantCulture))
                    {
                        _label = value;

                        if (ChildComponent != null)
                            ChildComponent.InternalLabel = value;
                    }
                }
            }
        }

        public RenderFragment LabelTemplate { get; set; }

        private string _groupName = string.Empty;

        public string GroupName
        {
            get => _groupName;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _groupName = value;

                    if (ChildComponent != null)
                        ChildComponent.GroupName = value;
                }
                else
                {
                    if (!_groupName.Equals(value, StringComparison.InvariantCulture))
                    {
                        _groupName = value;

                        if (ChildComponent != null)
                            ChildComponent.GroupName = value;
                    }
                }
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;

                    if (ChildComponent != null)
                        ChildComponent.IsSelected = value;
                }
            }
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;

                    if (ChildComponent != null)
                        ChildComponent.IsActive = value;
                }
            }
        }

        private bool _isDisabled;

        public bool IsDisabled
        {
            get => _isDisabled;
            set
            {
                if (_isDisabled != value)
                {
                    _isDisabled = value;

                    if (ChildComponent != null)
                        ChildComponent.IsDisabled = value;
                }
            }
        }

        private bool _isHidden;

        public bool IsHidden
        {
            get => _isHidden;
            set
            {
                if (_isHidden != value)
                {
                    _isHidden = value;

                    if (ChildComponent != null)
                        ChildComponent.IsHidden = value;
                }
            }
        }

        public bool IsAddedTag { get; set; }

        private SelectOption<TItemValue, TItem> _childComponent;

        public SelectOption<TItemValue, TItem> ChildComponent
        {
            get => _childComponent;
            set
            {
                if (value != null)
                {
                    _childComponent = value;
                    _childComponent.Model = this;
                    _childComponent.IsActive = _isActive;
                    _childComponent.IsSelected = _isSelected;
                    _childComponent.IsHidden = _isHidden;
                    _childComponent.IsDisabled = _isDisabled;
                    _childComponent.InternalLabel = _label;
                    _childComponent.GroupName = _groupName;
                }
            }
        }
    }
}
