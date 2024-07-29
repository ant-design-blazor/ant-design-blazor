using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// A component that can exist in the DOM tree for a long time, unless you destroy it on your own initiative
    /// </summary>
    public partial class DialogWrapper
    {
        #region Parameters

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public DialogOptions Config { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }

        /// <summary>
        /// Before destroy the component from the DOM tree. And you can cancel the destroy by set CancelEventArgs.Cancel = true
        /// </summary>
        [Parameter]
        public EventCallback<CancelEventArgs> OnBeforeDestroy { get; set; }

        /// <summary>
        /// trigger when visible is true on OnAfterRenderAsync method
        /// </summary>
        [Parameter]
        public EventCallback OnAfterShow { get; set; }

        /// <summary>
        /// trigger when visible is false on OnAfterRenderAsync method
        /// </summary>
        [Parameter]
        public EventCallback OnAfterHide { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Dialog Dialog => _dialog;

#pragma warning disable 649
        private Dialog _dialog;
#pragma warning restore 649

        private bool _hasAdd;

        private bool _showDone;
        private bool _hideDone;
        private bool _hasDestroy;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            if (Visible && !_hasAdd)
            {
                _hasAdd = true;
            }

            await base.OnParametersSetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (_hasAdd)
            {
                if (Visible && !_showDone)
                {
                    _showDone = true;
                    _hideDone = false;
                    _hasDestroy = false;

                    await _dialog.CleanShowAnimationAsync();
                    if (OnAfterShow.HasDelegate)
                    {
                        await OnAfterShow.InvokeAsync(null);
                    }

                }
                else if (!Visible)
                {

                    if (!_hideDone)
                    {
                        _hideDone = true;
                        _showDone = false;

                        if (_dialog != null)
                        {
                            await _dialog.Hide();
                            await Task.Delay(250);
                            await _dialog.TryResetModalStyle();
                        }
                        await OnAfterHide.InvokeAsync(null);
                    }

                    if (Config.DestroyOnClose && !_hasDestroy)
                    {
                        _hasDestroy = true;
                        await DestroyAsync();
                    }
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// Destroy the component from the DOM tree
        /// </summary>
        public async Task DestroyAsync()
        {
            var cancel = new CancelEventArgs();
            if (OnBeforeDestroy.HasDelegate)
            {
                await OnBeforeDestroy.InvokeAsync(cancel);
            }

            _hasAdd = cancel.Cancel;
            await InvokeStateHasChangedAsync();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
