using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
#pragma warning disable 1591
// ReSharper disable once CheckNamespace

namespace AntBlazor
{
    public partial class SelectOptGroup : AntDomComponentBase
    {
        #region Private
        private const string ClassNamePrefix = "ant-select-opt-group";
        #endregion

        #region Protected
        #region Properties
        protected IList<SelectOption> SelectOptions { get; set; } = new List<SelectOption>();
        #endregion

        #region Methods
        protected override void OnInitialized()
        {
            SetClassMap();
            base.OnInitialized();
        }

        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add(ClassNamePrefix);
        }
        #endregion
        #endregion

        #region Public
        #region Properties
        #region Parameters
        [CascadingParameter]
        public Select Parent { get; set; }

        [Parameter]
        public string Key { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }
        #endregion
        #endregion

        #region Methods
        public void AddOption(SelectOption option)
        {
            SelectOptions.Add(option);
            Parent.AddOption(option);
        }
        #endregion
        #endregion
    }
}
