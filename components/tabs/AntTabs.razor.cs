using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class AntTabs : AntDomComponentBase
    {
        private const string PrefixCls = "ant-tabs";
        private ClassMapper _barClassMapper = new ClassMapper();
        private List<AntTabPane> _panes = new List<AntTabPane>();
        private AntTabPane _activePane;

        #region Parameters

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Current <see cref="AntTabPane"/>'s <see cref="AntTabPane.Key"/>
        /// </summary>
        [Parameter]
        public string ActiveKey { get; set; }

        /// <summary>
        /// Whether to change tabs with animation. Only works while <see cref="TabPosition"/> = <see cref="AntTabPosition.Top"/> or <see cref="AntTabPosition.Bottom"/>
        /// </summary>
        [Parameter]
        public bool Animated { get; set; } = true;

        /// <summary>
        /// Replace the TabBar
        /// </summary>
        [Parameter]
        public object RenderTabBar { get; set; }

        /// <summary>
        /// Initial active <see cref="AntTabPane"/>'s <see cref="AntTabPane.Key"/>, if <see cref="ActiveKey"/> is not set
        /// </summary>
        [Parameter]
        public string DefaultActiveKey { get; set; }

        /// <summary>
        /// Hide plus icon or not. Only works while <see cref="Type"/> = <see cref="AntTabType.EditableCard"/>
        /// </summary>
        [Parameter]
        public bool HideAdd { get; set; } = false;

        /// <summary>
        /// Preset tab bar size
        /// </summary>
        [Parameter]
        public string Size { get; set; } = AntTabSize.Default;

        /// <summary>
        /// Extra content in tab bar
        /// </summary>
        [Parameter]
        public RenderFragment TabBarExtraContent { get; set; }

        /// <summary>
        /// The gap between tabs
        /// </summary>
        [Parameter]
        public int TabBarGutter { get; set; }

        /// <summary>
        /// Tab bar style object
        /// </summary>
        [Parameter]
        public string TabBarStyle { get; set; }

        /// <summary>
        /// Position of tabs
        /// </summary>
        [Parameter]
        public string TabPosition { get; set; } = AntTabPosition.Top;

        /// <summary>
        /// Basic style of tabs
        /// </summary>
        [Parameter]
        public string Type { get; set; } = AntTabType.Line;

        /// <summary>
        /// Callback executed when active tab is changed
        /// </summary>
        [Parameter]
        public EventCallback<object> OnChange { get; set; }

        /// <summary>
        /// Callback executed when tab is added or removed. Only works while <see cref="Type"/> = <see cref="AntTabType.EditableCard"/>
        /// </summary>
        [Parameter]
        public EventCallback<object> OnEdit { get; set; }

        /// <summary>
        /// Callback executed when next button is clicked
        /// </summary>
        [Parameter]
        public EventCallback<object> OnNextClick { get; set; }

        /// <summary>
        /// Callback executed when prev button is clicked
        /// </summary>
        [Parameter]
        public EventCallback<object> OnPrevClick { get; set; }

        /// <summary>
        /// Callback executed when tab is clicked
        /// </summary>
        [Parameter]
        public EventCallback<object> OnTabClick { get; set; }

        /// <summary>
        /// Whether to turn on keyboard navigation
        /// </summary>
        [Parameter]
        public bool Keyboard { get; set; } = true;

        #endregion

        public override Task SetParametersAsync(ParameterView parameters)
        {
            string type = parameters.GetValueOrDefault<string>(nameof(Type));

            if (type == AntTabType.Card)
            {
                // according to ant design documents,
                // Animated default to false when type="card"
                Animated = false;
            }

            return base.SetParametersAsync(parameters);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            ClassMapper.Clear()
                .Add(PrefixCls)
                .Add($"{PrefixCls}-{TabPosition}")
                .Add($"{PrefixCls}-{Type}")
                .If($"{PrefixCls}-{AntTabType.Card}", () => Type == AntTabType.EditableCard)
                .If($"{PrefixCls}-no-animation", () => !Animated);

            _barClassMapper.Clear()
                .Add($"{PrefixCls}-bar")
                .Add($"{PrefixCls}-{TabPosition}-bar");

        }

        internal void AddTabPane(AntTabPane tabPane)
        {
            _panes.Add(tabPane);
            if (_activePane == null)
            {
                ActivatePane(tabPane);
            }
            StateHasChanged();
        }

        private void ActivatePane(AntTabPane tabPane)
        {
            if (_activePane != null)
            {
                _activePane.IsActive = false;
            }
            tabPane.IsActive = true;
            _activePane = tabPane;
            StateHasChanged();
        }
    }
}