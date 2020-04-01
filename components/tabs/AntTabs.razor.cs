using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntBlazor
{
    public class AntTabs : AntComponentBase
    {
        /// <summary>
        /// Current <see cref="AntTabPane"/>'s <see cref="AntTabPane.Key"/>
        /// </summary>
        [Parameter]
        public string ActiveKey { get; set; }

        /// <summary>
        /// Whether to change tabs with animation. Only works while <see cref="TabPosition"/> = <see cref="AntTabPosition.Top"/> or <see cref="AntTabPosition.Bottom"/>
        /// </summary>
        [Parameter]
        public bool Animated { get; set; }

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
        public bool HideAdd { get; set; }

        /// <summary>
        /// Preset tab bar size
        /// </summary>
        [Parameter]
        public string Size { get; set; }

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
        public string TabPosition { get; set; }

        /// <summary>
        /// Basic style of tabs
        /// </summary>
        [Parameter]
        public string Type { get; set; }

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
        public bool Keyboard { get; set; }
    }
}