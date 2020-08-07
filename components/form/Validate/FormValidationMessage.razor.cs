// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign
{
    /// <summary>
    /// FormValidationMessage is copy from ValidationMessage.
    /// Displays a list of validation messages for a specified field within a cascaded <see cref="EditContext"/>.
    /// </summary>
    public partial class FormValidationMessage<TValue> : ComponentBase, IDisposable
    {
        private EditContext _previousEditContext;
        private Expression<Func<TValue>> _previousFieldAccessor;

        //private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;
        private FieldIdentifier _fieldIdentifier;

        /// <summary>
        /// Gets or sets a collection of additional attributes that will be applied to the created <c>div</c> element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        [CascadingParameter] private EditContext CurrentEditContext { get; set; }

        public AntInputComponentBase<TValue> _control;

        [Parameter]
        public AntInputComponentBase<TValue> Control
        {
            get
            {
                return _control;
            }
            set
            {
                if (value != null && _control != value)
                {
                    _control = value;
                    _fieldIdentifier = _control.FieldIdentifier;
                    _previousFieldAccessor = _control.ValueExpression;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
        }
    }
}
