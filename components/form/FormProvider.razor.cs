// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class FormProvider : AntComponentBase, IFormProvider
    {
        [Parameter]
        public EventCallback<FormProviderFinishEventArgs> OnFormFinish { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private Dictionary<string, IForm> _forms = new Dictionary<string, IForm>();

        void IFormProvider.AddForm(IForm form)
        {
            _forms.Add(form.Name, form);

            form.OnFinishEvent += OnFinish;
        }

        void IFormProvider.RemoveForm(IForm form)
        {
            _forms.Remove(form.Name);

            form.OnFinishEvent += OnFinish;
        }

        private void OnFinish(IForm form)
        {
            var args = new FormProviderFinishEventArgs
            {
                Forms = _forms,
                FinishForm = form
            };

            OnFormFinish.InvokeAsync(args);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _forms.Values.ForEach(form =>
            {
                form.OnFinishEvent -= OnFinish;
            });
        }

        public IForm GetForm(string formName)
        {
            if (_forms.TryGetValue(formName, out IForm form))
            {
                return form;
            }

            return null;
        }

        public bool TryGetForm(string formName, out IForm form)
        {
            if (_forms.TryGetValue(formName, out form))
            {
                return true;
            }

            return false;
        }
    }
}
