// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using AntDesign.Form.Locale;
using AntDesign.Forms;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
    <para>High performance Form component with data scope management. Including data collection, verification, and styles.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>When you need to create an instance or collect information.</item>
        <item>When you need to validate fields in certain rules.</item>
    </list>
    </summary>
    <seealso cref="FormValidateMode"/>
    <seealso cref="FormItem" />
    <seealso cref="FormValidationRule"/>
    <seealso cref="FormValidateErrorMessages"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "https://gw.alipayobjects.com/zos/alicdn/ORmcdeaoO/Form.svg", Columns = 1, Title = "Form", SubTitle = "表单")]
#if NET6_0_OR_GREATER
    [CascadingTypeParameter(nameof(TModel))]
#endif
    public partial class Form<TModel> : AntDomComponentBase, IForm
    {
        private readonly string _prefixCls = "ant-form";

        /// <summary>
        /// Change how required/optional field labels are displayed on the form.
        /// <list type="bullet">
        ///     <item>Required - Will mark required fields</item>
        ///     <item>Optional - Will mark optional fields</item>
        ///     <item>None - Will mark no fields, regardless of required/optional</item>
        /// </list>
        /// </summary>
        [Parameter]
        public FormRequiredMark RequiredMark { get; set; } = FormRequiredMark.Required;

        /// <summary>
        /// Layout of form items in the form
        /// </summary>
        /// <default value="FormLayout.Horizontal"/>
        [Parameter]
        public FormLayout Layout { get; set; } = FormLayout.Horizontal;

        /// <summary>
        /// Content of the form. Typically contains different form inputs and layout elements.
        /// </summary>
        [Parameter]
        public RenderFragment<TModel> ChildContent { get; set; }

        /// <summary>
        /// Control the layout of the label. Commonly used to set widths for different screen sizes.
        /// </summary>
        [Parameter]
        public ColLayoutParam LabelCol { get; set; } = new ColLayoutParam();

        /// <summary>
        /// Align the label to the left or right
        /// </summary>
        [Parameter]
        public AntLabelAlignType? LabelAlign { get; set; }

        /// <summary>
        /// Gets/sets the <c>Span</c> property on <see cref="LabelCol"/>.
        /// </summary>
        [Parameter]
        public OneOf<string, int> LabelColSpan
        {
            get { return LabelCol.Span; }
            set { LabelCol.Span = value; }
        }

        /// <summary>
        /// Gets/sets the <c>Offset</c> property on <see cref="LabelCol"/>.
        /// </summary>
        [Parameter]
        public OneOf<string, int> LabelColOffset
        {
            get { return LabelCol.Offset; }
            set { LabelCol.Offset = value; }
        }

        /// <summary>
        /// Control the layout of the input element's wrapper. Commonly used to set widths for different screen sizes.
        /// </summary>
        [Parameter]
        public ColLayoutParam WrapperCol { get; set; } = new ColLayoutParam();

        /// <summary>
        /// Gets/sets the <c>Span</c> property on <see cref="WrapperCol"/>.
        /// </summary>
        [Parameter]
        public OneOf<string, int> WrapperColSpan
        {
            get { return WrapperCol.Span; }
            set { WrapperCol.Span = value; }
        }

        /// <summary>
        /// Gets/sets the <c>Offset</c> property on <see cref="WrapperColOffset"/>.
        /// </summary>
        [Parameter]
        public OneOf<string, int> WrapperColOffset
        {
            get { return WrapperCol.Offset; }
            set { WrapperCol.Offset = value; }
        }

        /// <summary>
        /// The size of the ant components inside the form
        /// </summary>
        [Parameter]
        public FormSize? Size { get; set; }

        /// <summary>
        /// Gets or sets the form handler name. This is required for posting it to a server-side endpoint.
        /// Or using for get the form instance from <see cref="AntDesign.FormProviderFinishEventArgs"/>.
        /// </summary>
        [Parameter]
        public string Name { get; set; }

        /// <summary>
        /// Http method used to submit form
        /// </summary>
        [Parameter]
        public HttpMethod Method { get; set; } = HttpMethod.Get;

        /// <summary>
        /// The model to bind the form inputs to
        /// </summary>
        [Parameter]
        public TModel Model
        {
            get { return _model; }
            set
            {
                //prevent building edit context in dead loop
                if (value is null && _isModelBuildFromNull)
                {
                    return;
                }

                if (!(_model?.Equals(value) ?? false))
                {
                    var wasNull = _model is null;
                    _model = value;
                    if (!wasNull)
                        BuildEditContext();
                }
            }
        }

        /// <summary>
        /// If the form is loading or not
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Loading { get; set; }

        /// <summary>
        /// Callback executed when the form is submitted and PASSES validation.
        /// </summary>
        [Parameter]
        public EventCallback<EditContext> OnFinish { get; set; }

        /// <summary>
        /// Callback executed when the form is submitted and FAILS validation.
        /// </summary>
        [Parameter]
        public EventCallback<EditContext> OnFinishFailed { get; set; }

        /// <summary>
        /// Callback executed when a field inside the form changes
        /// </summary>
        [Parameter]
        public EventCallback<FieldChangedEventArgs> OnFieldChanged { get; set; }

        /// <summary>
        /// Callback executed when validation is requested
        /// </summary>
        [Parameter]
        public EventCallback<ValidationRequestedEventArgs> OnValidationRequested { get; set; }

        /// <summary>
        /// Callback executed when the validation changes
        /// </summary>
        [Parameter]
        public EventCallback<ValidationStateChangedEventArgs> OnValidationStateChanged { get; set; }

        /// <summary>
        /// Validator to use in the form. Used when <see cref="ValidateMode"/> is <c>FormValidateMode.Default</c> or <c>FormValidateMode.Complex</c>
        /// </summary>
        [Parameter]
        public RenderFragment Validator { get; set; }

        /// <summary>
        /// Enable validation when component values change
        /// </summary>
        [Parameter]
        public bool ValidateOnChange { get; set; }

        /// <summary>
        /// Which mode of validation the form should use
        /// </summary>
        /// <default value="FormValidateMode.Complex"/>
        [Parameter]
        [Obsolete("Will use both attributes and rules in the same time.")]
        public FormValidateMode ValidateMode { get; set; } = FormValidateMode.Complex;

        /// <summary>
        /// If enabled, form submission is performed without fully reloading the page. This is equivalent to adding data-enhance to the form.
        /// </summary>
        [Parameter]
        public bool Enhance { get; set; }

        /// <summary>
        /// Whether input elements can by default have their values automatically completed by the browser
        /// </summary>
        [Parameter]
        public string Autocomplete { get; set; } = "off";

        /// <summary>
        /// The localization options
        /// </summary>
        [Parameter]
        public FormLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Form;

        [CascadingParameter(Name = "FormProvider")]
        private IFormProvider FormProvider { get; set; }

        /// <summary>
        /// A flag indicating if the form has been modified
        /// </summary>
        public bool IsModified => _editContext.IsModified();

        private EditContext _editContext;
        private IList<IFormItem> _formItems = new List<IFormItem>();
        private IList<IControlValueAccessor> _controls = new List<IControlValueAccessor>();
        private TModel _model;
        private FormRulesValidator _rulesValidator;
        private bool _isModelBuildFromNull;

        ColLayoutParam IForm.WrapperCol => WrapperCol;

        ColLayoutParam IForm.LabelCol => LabelCol;

        EditContext IForm.EditContext => _editContext;

        AntLabelAlignType? IForm.LabelAlign => LabelAlign;
        FormSize IForm.Size => Size.GetValueOrDefault();
        string IForm.Name => Name;
        object IForm.Model => Model;
        bool IForm.ValidateOnChange => ValidateOnChange;

        bool IForm.IsModified => _editContext.IsModified();

        FormValidateMode IForm.ValidateMode => ValidateMode;
        FormLocale IForm.Locale => Locale;

        private event Action<IForm> OnFinishEvent;

        event Action<IForm> IForm.OnFinishEvent
        {
            add
            {
                OnFinishEvent += value;
            }

            remove
            {
                OnFinishEvent -= value;
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Model ??= (TModel)Expression.New(typeof(TModel)).Constructor.Invoke(new object[] { });

            _editContext = new EditContext(Model);

            FormProvider?.AddForm(this);

            if (OnFieldChanged.HasDelegate)
                _editContext.OnFieldChanged += OnFieldChangedHandler;
            if (OnValidationRequested.HasDelegate)
                _editContext.OnValidationRequested += OnValidationRequestedHandler;
            if (OnValidationStateChanged.HasDelegate)
                _editContext.OnValidationStateChanged += OnValidationStateChangedHandler;

            if (UseRulesValidator)
            {
                _editContext.OnFieldChanged += RulesModeOnFieldChanged;
                _editContext.OnValidationRequested += RulesModeOnValidationRequested;
            }
        }

        private void OnFieldChangedHandler(object sender, FieldChangedEventArgs e) => InvokeAsync(() => OnFieldChanged.InvokeAsync(e));

        private void OnValidationRequestedHandler(object sender, ValidationRequestedEventArgs e) => InvokeAsync(() => OnValidationRequested.InvokeAsync(e));

        private void OnValidationStateChangedHandler(object sender, ValidationStateChangedEventArgs e) => InvokeAsync(() => OnValidationStateChanged.InvokeAsync(e));

        protected override void Dispose(bool disposing)
        {
            if (_editContext != null)
            {
                if (OnFieldChanged.HasDelegate)
                    _editContext.OnFieldChanged -= OnFieldChangedHandler;
                if (OnValidationRequested.HasDelegate)
                    _editContext.OnValidationRequested -= OnValidationRequestedHandler;
                if (OnValidationStateChanged.HasDelegate)
                    _editContext.OnValidationStateChanged -= OnValidationStateChangedHandler;

                if (UseRulesValidator)
                {
                    _editContext.OnFieldChanged -= RulesModeOnFieldChanged;
                    _editContext.OnValidationRequested -= RulesModeOnValidationRequested;
                }
            }

            base.Dispose(disposing);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            SetClass();
        }

        protected void SetClass()
        {
            this.ClassMapper.Clear()
                .Add(_prefixCls)
                .Get(() => $"{_prefixCls}-{Layout.ToString().ToLowerInvariant()}")
                .If($"{_prefixCls}-rtl", () => RTL)
               ;
        }

        private async Task OnValidSubmit(EditContext editContext)
        {
            await OnFinish.InvokeAsync(editContext);

            OnFinishEvent?.Invoke(this);
        }

        private async Task OnInvalidSubmit(EditContext editContext)
        {
            await OnFinishFailed.InvokeAsync(editContext);
        }

        private void RulesModeOnFieldChanged(object sender, FieldChangedEventArgs args)
        {

            _rulesValidator.ClearError(args.FieldIdentifier);

            var formItem = _formItems
                .FirstOrDefault(t => t.GetFieldIdentifier().Equals(args.FieldIdentifier));

            if (formItem == null)
            {
                return;
            }

            var result = formItem.ValidateFieldWithRules();

            if (result.Any())
            {
                var errors = new Dictionary<FieldIdentifier, List<string>>
                {
                    [args.FieldIdentifier] = [.. result.Select(r => r.ErrorMessage)]
                };

                _rulesValidator.DisplayErrors(errors);
            }

            // invoke StateHasChanged to update the form state like error message and IsModified
            StateHasChanged();
        }

        private void RulesModeOnValidationRequested(object sender, ValidationRequestedEventArgs args)
        {
            _rulesValidator.ClearErrors();

            var errors = new Dictionary<FieldIdentifier, List<string>>();

            foreach (var formItem in _formItems)
            {
                var result = formItem.ValidateFieldWithRules();
                if (result.Any())
                {
                    errors[formItem.GetFieldIdentifier()] = [.. result.Select(r => r.ErrorMessage)];
                }
            }

            _rulesValidator.DisplayErrors(errors);
        }

        /// <summary>
        /// Reset all the values in the form
        /// </summary>
        public void Reset()
        {
            _controls.ForEach(item => item.Reset());
            BuildEditContext();
        }

        void IForm.AddFormItem(IFormItem formItem)
        {
            _formItems.Add(formItem);
        }

        void IForm.RemoveFormItem(IFormItem formItem)
        {
            _formItems.Remove(formItem);
        }

        void IForm.AddControl(IControlValueAccessor valueAccessor)
        {
            this._controls.Add(valueAccessor);
        }

        void IForm.RemoveControl(IControlValueAccessor valueAccessor)
        {
            if (_controls.Contains(valueAccessor))
            {
                this._controls.Remove(valueAccessor);
            }
        }

        /// <summary>
        /// Submit the form. Will trigger validation and either <see cref="OnFinish"/> or <see cref="OnFinishFailed"/>.
        /// </summary>
        public void Submit()
        {
            var isValid = Validate();

            if (isValid)
            {
                if (OnFinish.HasDelegate)
                {
                    OnFinish.InvokeAsync(_editContext);
                }

                OnFinishEvent?.Invoke(this);
            }
            else
            {
                OnFinishFailed.InvokeAsync(_editContext);
            }
        }

        /// <summary>
        /// Execute validation   
        /// </summary>
        /// <returns> return <c>true</c> if all fields are valid </returns>
        public bool Validate()
        {
            var result = _editContext.Validate();

            return result;
        }


        /// <summary>
        /// Reset validation
        /// </summary>
        public void ValidationReset() => BuildEditContext();

        /// <summary>
        /// Get the <see cref="EditContext"/> instance inner the form
        /// </summary>
        public EditContext EditContext => _editContext;

        private bool UseLocaleValidateMessage => Locale.DefaultValidateMessages != null;

        bool IForm.UseLocaleValidateMessage => UseLocaleValidateMessage;

        private bool UseRulesValidator => UseLocaleValidateMessage && Validator == null;

        private void BuildEditContext()
        {
            if (_editContext == null)
                return;

            _isModelBuildFromNull = false;

            if (Model == null)
            {
                _model = (TModel)Expression.New(typeof(TModel)).Constructor.Invoke([]);
                _isModelBuildFromNull = true;
            }

            var newContext = new EditContext(Model);
            foreach (var kv in GetEventInfos())
            {
                FieldInfo fieldInfo = kv.Value.fi;
                EventInfo eventInfo = kv.Value.ei;
                if (fieldInfo.GetValue(_editContext) is Delegate mdel)
                {
                    foreach (Delegate del in mdel.GetInvocationList())
                    {
                        eventInfo.RemoveEventHandler(_editContext, del);
                        eventInfo.AddEventHandler(newContext, del);
                    }
                }
            }
            _editContext = newContext;

            // because EditForm's editcontext CascadingValue is fixed,so there need invoke StateHasChanged,
            // otherwise, the child component's(FormItem) EditContext will not update.
            InvokeAsync(StateHasChanged);
        }

        private static BindingFlags AllBindings
        {
            get { return BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance; }
        }

        private static Dictionary<string, (FieldInfo fi, EventInfo ei)> _eventInfos;

        private static Dictionary<string, (FieldInfo fi, EventInfo ei)> GetEventInfos()
        {
            if (_eventInfos is null)
            {
                _eventInfos = [];
                Type contextType = typeof(EditContext);
                foreach (EventInfo eventInfo in contextType.GetEvents(AllBindings))
                {
                    Type declaringType = eventInfo.DeclaringType;
                    FieldInfo fieldInfo = declaringType.GetField(eventInfo.Name, AllBindings);
                    if (fieldInfo is not null)
                    {
                        _eventInfos.Add(eventInfo.Name, (fieldInfo, eventInfo));
                    }
                }
            }
            return _eventInfos;
        }

        /// <summary>
        /// Set validation messages to a specific field.
        /// </summary>
        /// <param name="field">The field name</param>
        /// <param name="errorMessages">The error messages</param>
        public void SetValidationMessages(string field, string[] errorMessages)
        {
            var fieldIdentifier = _editContext.Field(field);
            var formItem = _formItems
              .FirstOrDefault(t => t.GetFieldIdentifier().Equals(fieldIdentifier));

            formItem?.SetValidationMessage(errorMessages);
        }
    }
}
