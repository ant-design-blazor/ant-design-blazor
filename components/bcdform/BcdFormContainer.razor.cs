using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class BcdFormContainer
    {
        [Inject]
        internal IJSRuntime JsRuntime { get; set; }

        internal static int MinFormCount { get; set; } = 0;

        internal static BcdFormContainer BcdFormContainerInstance { get; private set; }

        /// <summary>
        /// close the form
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        internal static async Task CloseFormAsync(BcdForm form)
        {
            await form.CloseAsync();
        }


        private readonly HashSet<BcdForm> _forms;
        private readonly Dictionary<BcdForm, RenderFragment> _form2Compontents;
        private readonly FieldInfo _innerRenderFragmentFieldInfo;
        private readonly FieldInfo _renderHandleFieldInfo;
        private readonly RenderHandle _renderHandle;


        /// <summary>
        /// 
        /// </summary>
        [Inject]
        internal IJSRuntime BcdJsRuntime { get; set; }

        public BcdFormContainer()
        {
            var type = typeof(ComponentBase);
            _innerRenderFragmentFieldInfo = type.GetField("_renderFragment", BindingFlags.NonPublic | BindingFlags.Instance);
            _renderHandleFieldInfo = type.GetField("_renderHandle", BindingFlags.NonPublic | BindingFlags.Instance);
            var thisRenderHandle = _renderHandleFieldInfo.GetValue(this);
            if (thisRenderHandle is RenderHandle)
            {
                _renderHandle =(RenderHandle) thisRenderHandle;
            }
            BcdFormContainerInstance = this;
            _forms = new HashSet<BcdForm>();
            _form2Compontents = new Dictionary<BcdForm, RenderFragment>();
        }

        /// <summary>
        /// remove form from DOM
        /// </summary>
        /// <param name="bcdForm"></param>
        /// <returns></returns>
        internal async Task RemoveFormAsync(BcdForm bcdForm)
        {
            if (_forms.Contains(bcdForm))
            {
                _forms.Remove(bcdForm);
                _form2Compontents.Remove(bcdForm);
                await bcdForm.AfterRenderAsync();
                bcdForm.Dispose();
                await InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// remove form to DOM
        /// </summary>
        /// <param name="bcdForm"></param>
        /// <returns></returns>
        internal async Task AppendFormAsync(BcdForm bcdForm)
        {
            if (!_forms.Contains(bcdForm))
            {
                _forms.Add(bcdForm);
                RenderFragment value = _innerRenderFragmentFieldInfo.GetValue(bcdForm) as RenderFragment;
                _form2Compontents.Add(bcdForm, value);
                await InvokeAsync(StateHasChanged);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            foreach (var form in _forms)
            {
                if (form.ShouldReRender)
                {
                    await form.AfterRenderAsync();
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        internal void InvokeStateHasChanged()
        {
            StateHasChanged();
        }

        internal Task InvokeStateHasChangedAsync()
        {
            return InvokeAsync(StateHasChanged);
        }
    }
}
