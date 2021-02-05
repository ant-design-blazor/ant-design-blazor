using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        [Parameter]
        public string Layout { get; set; } = FormLayout.Horizontal;

        [Parameter]
        public RenderFragment<TModel> ChildContent { get; set; }

        [Parameter]
        public ColLayoutParam LabelCol { get; set; } = new ColLayoutParam();

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

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public TModel Model
        {
            get { return _model; }
            set
            {
                if (!(_model?.Equals(value) ?? false))
                {
                    _model = value;
                    _editContext = new EditContext(Model);
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
        public RenderFragment Validator { get; set; } = _defaultValidator;

        /// <summary>
        /// Enable validation when component values change
        /// </summary>
        [Parameter]
        public bool ValidateOnChange { get; set; }

        private static readonly RenderFragment _defaultValidator = builder =>
        {
            builder.OpenComponent<ObjectGraphDataAnnotationsValidator>(0);
            builder.CloseComponent();
        };

        [CascadingParameter(Name = "FormProvider")]
        private IFormProvider FormProvider { get; set; }

        public bool IsModified => _editContext.IsModified();

        private EditContext _editContext;
        private IList<IFormItem> _formItems = new List<IFormItem>();
        private IList<IControlValueAccessor> _controls = new List<IControlValueAccessor>();
        private TModel _model;

        ColLayoutParam IForm.WrapperCol => WrapperCol;

        ColLayoutParam IForm.LabelCol => LabelCol;

        EditContext IForm.EditContext => _editContext;

        string IForm.Size => Size;
        string IForm.Name => Name;
        object IForm.Model => Model;
        bool IForm.ValidateOnChange => ValidateOnChange;

        bool IForm.IsModified => _editContext.IsModified();

        public event Action<IForm> OnFinishEvent;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _editContext = new EditContext(Model);

            if (FormProvider != null)
            {
                FormProvider.AddForm(this);
            }
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
                .Add($"{_prefixCls}-{Layout.ToLower()}")
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

        public void Reset()
        {
            _controls.ForEach(item => item.Reset());
            _editContext = new EditContext(Model);
        }

        void IForm.AddFormItem(IFormItem formItem)
        {
            _formItems.Add(formItem);
        }

        void IForm.AddControl(IControlValueAccessor valueAccessor)
        {
            this._controls.Add(valueAccessor);
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

        public bool Validate()
        {
            return _editContext.Validate();
        }

        public void ValidationReset()
        {
            _editContext = new EditContext(Model);
        }
    }
}
