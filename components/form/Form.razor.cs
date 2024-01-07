using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class Form<TModel> : AntDomComponentBase, IForm
    {
        private readonly string _prefixCls = "ant-form";

        /// <summary>
        /// Change how required/optional field labels are displayed on the form.
        /// 
        /// <list type="bullet">
        ///     <item>Required - Will mark required fields</item>
        ///     <item>Optional - Will mark optional fields</item>
        ///     <item>None - Will mark no fields, regardless of required/optional</item>
        /// </list>
        /// </summary>
        [Parameter]
        public FormRequiredMark RequiredMark { get; set; } = FormRequiredMark.Required;

        [Parameter]
        public string Layout { get; set; } = FormLayout.Horizontal;

        [Parameter]
        public RenderFragment<TModel> ChildContent { get; set; }

        [Parameter]
        public ColLayoutParam LabelCol { get; set; } = new ColLayoutParam();

        [Parameter]
        public AntLabelAlignType? LabelAlign { get; set; }

        [Parameter]
        public OneOf<string, int> LabelColSpan
        {
            get { return LabelCol.Span; }
            set { LabelCol.Span = value; }
        }

        [Parameter]
        public OneOf<string, int> LabelColOffset
        {
            get { return LabelCol.Offset; }
            set { LabelCol.Offset = value; }
        }

        [Parameter]
        public ColLayoutParam WrapperCol { get; set; } = new ColLayoutParam();

        [Parameter]
        public OneOf<string, int> WrapperColSpan
        {
            get { return WrapperCol.Span; }
            set { WrapperCol.Span = value; }
        }

        [Parameter]
        public OneOf<string, int> WrapperColOffset
        {
            get { return WrapperCol.Offset; }
            set { WrapperCol.Offset = value; }
        }

        [Parameter]
        public string Size { get; set; }

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
        public string Method { get; set; } = "get";

        [Parameter]
        public TModel Model
        {
            get { return _model; }
            set
            {
                if (!(_model?.Equals(value) ?? false))
                {
                    var wasNull = _model is null;
                    _model = value;
                    if (!wasNull)
                        BuildEditContext();
                }
            }
        }

        [Parameter]
        public bool Loading { get; set; }

        [Parameter]
        public EventCallback<EditContext> OnFinish { get; set; }

        [Parameter]
        public EventCallback<EditContext> OnFinishFailed { get; set; }

        [Parameter]
        public EventCallback<FieldChangedEventArgs> OnFieldChanged { get; set; }

        [Parameter]
        public EventCallback<ValidationRequestedEventArgs> OnValidationRequested { get; set; }

        [Parameter]
        public EventCallback<ValidationStateChangedEventArgs> OnValidationStateChanged { get; set; }

        [Parameter]
        public RenderFragment Validator { get; set; } = _defaultValidator;

        /// <summary>
        /// Enable validation when component values change
        /// </summary>
        [Parameter]
        public bool ValidateOnChange { get; set; }

        [Parameter]
        public FormValidateMode ValidateMode { get; set; } = FormValidateMode.Default;

        private static readonly RenderFragment _defaultValidator = builder =>
        {
            builder.OpenComponent<ObjectGraphDataAnnotationsValidator>(0);
            builder.CloseComponent();
        };

        [Parameter]
        public FormValidateErrorMessages ValidateMessages { get; set; }


        [CascadingParameter(Name = "FormProvider")]
        private IFormProvider FormProvider { get; set; }

        public bool IsModified => _editContext.IsModified();

        private EditContext _editContext;
        private IList<IFormItem> _formItems = new List<IFormItem>();
        private IList<IControlValueAccessor> _controls = new List<IControlValueAccessor>();
        private TModel _model;
        private FormRulesValidator _rulesValidator;

        ColLayoutParam IForm.WrapperCol => WrapperCol;

        ColLayoutParam IForm.LabelCol => LabelCol;

        EditContext IForm.EditContext => _editContext;

        AntLabelAlignType? IForm.LabelAlign => LabelAlign;
        string IForm.Size => Size;
        string IForm.Name => Name;
        object IForm.Model => Model;
        bool IForm.ValidateOnChange => ValidateOnChange;

        bool IForm.IsModified => _editContext.IsModified();

        FormValidateMode IForm.ValidateMode => ValidateMode;
        FormValidateErrorMessages IForm.ValidateMessages => ValidateMessages;

        public event Action<IForm> OnFinishEvent;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _editContext = new EditContext(Model);

            if (FormProvider != null)
            {
                FormProvider.AddForm(this);
            }

            if (OnFieldChanged.HasDelegate)
                _editContext.OnFieldChanged += OnFieldChangedHandler;
            if (OnValidationRequested.HasDelegate)
                _editContext.OnValidationRequested += OnValidationRequestedHandler;
            if (OnValidationStateChanged.HasDelegate)
                _editContext.OnValidationStateChanged += OnValidationStateChangedHandler;

            if (ValidateMode.IsIn(FormValidateMode.Rules, FormValidateMode.Complex))
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
            if (OnFieldChanged.HasDelegate)
                _editContext.OnFieldChanged -= OnFieldChangedHandler;
            if (OnValidationRequested.HasDelegate)
                _editContext.OnValidationRequested -= OnValidationRequestedHandler;
            if (OnValidationStateChanged.HasDelegate)
                _editContext.OnValidationStateChanged -= OnValidationStateChangedHandler;

            if (ValidateMode.IsIn(FormValidateMode.Rules, FormValidateMode.Complex))
            {
                _editContext.OnFieldChanged -= RulesModeOnFieldChanged;
                _editContext.OnValidationRequested -= RulesModeOnValidationRequested;
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
                .Get(() => $"{_prefixCls}-{Layout.ToLowerInvariant()}")
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
            if (!ValidateMode.IsIn(FormValidateMode.Rules, FormValidateMode.Complex))
            {
                return;
            }

            _rulesValidator.ClearError(args.FieldIdentifier);

            var formItem = _formItems
                .Single(t => t.GetFieldIdentifier().Equals(args.FieldIdentifier));

            var result = formItem.ValidateField();

            if (result.Length > 0)
            {
                var errors = new Dictionary<FieldIdentifier, List<string>>();
                errors[args.FieldIdentifier] = result.Select(r => r.ErrorMessage).ToList();

                _rulesValidator.DisplayErrors(errors);
            }
        }

        private void RulesModeOnValidationRequested(object sender, ValidationRequestedEventArgs args)
        {
            if (!ValidateMode.IsIn(FormValidateMode.Rules, FormValidateMode.Complex))
            {
                return;
            }

            _rulesValidator.ClearErrors();

            var errors = new Dictionary<FieldIdentifier, List<string>>();

            foreach (var formItem in _formItems)
            {
                var result = formItem.ValidateField();
                if (result.Length > 0)
                {
                    errors[formItem.GetFieldIdentifier()] = result.Select(r => r.ErrorMessage).ToList();
                }
            }

            _rulesValidator.DisplayErrors(errors);
        }

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

        public void Submit()
        {
            var isValid = _editContext.Validate();

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

        public bool Validate() => _editContext.Validate();

        public void ValidationReset() => BuildEditContext();

        public EditContext EditContext => _editContext;

        public void BuildEditContext()
        {
            if (_editContext == null)
                return;

            var newContext = new EditContext(Model);
            foreach (var kv in GetEventInfos())
            {
                FieldInfo fieldInfo = kv.Value.fi;
                EventInfo eventInfo = kv.Value.ei;
                Delegate mdel = fieldInfo.GetValue(_editContext) as Delegate;
                if (mdel != null)
                {
                    foreach (Delegate del in mdel.GetInvocationList())
                    {
                        eventInfo.RemoveEventHandler(_editContext, del);
                        eventInfo.AddEventHandler(newContext, del);
                    }
                }
            }
            _editContext = newContext;
        }

        static BindingFlags AllBindings
        {
            get { return BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance; }
        }

        static Dictionary<string, (FieldInfo fi, EventInfo ei)> _eventInfos;

        static Dictionary<string, (FieldInfo fi, EventInfo ei)> GetEventInfos()
        {
            if (_eventInfos is null)
            {
                _eventInfos = new();
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
    }
}
