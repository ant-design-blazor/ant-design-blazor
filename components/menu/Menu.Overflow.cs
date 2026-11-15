using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class Menu : IAsyncDisposable
    {
        /// <summary>Disable automatic item overflow in horizontal mode.</summary>
        [Parameter] public bool DisabledOverflow { get; set; }

        /// <summary>Content of the horizontal menu's overflow indicator.</summary>
        [Parameter] public RenderFragment OverflowedIndicator { get; set; }

        /// <summary>CSS class of the overflow submenu popup.</summary>
        [Parameter] public string OverflowedIndicatorPopupClassName { get; set; }

        private sealed class OverflowItem
        {
            public string Id;
            public RenderFragment Content;
            public Action Refresh;
        }

        private readonly List<OverflowItem> _overflowItems = new();
        private readonly Dictionary<string, string[]> _menuKeyPaths = new(StringComparer.Ordinal);
        private readonly Dictionary<string, string> _overflowOwners = new(StringComparer.Ordinal);
        private HashSet<string> _overflowIds = new(StringComparer.Ordinal);
        private string[] _overflowOrder = Array.Empty<string>();
        private DotNetObjectReference<Menu> _overflowReference;
        private SubMenu _overflowSubmenu;
        internal bool OverflowEnabled => InternalMode == MenuMode.Horizontal && !DisabledOverflow;
        internal bool HasOverflowItems => OverflowEnabled && _overflowIds.Count > 0;
        internal bool OverflowHasSelected => _selectedKeys.Any(key => _overflowOwners.TryGetValue(key, out var owner) && IsOverflowed(owner));
        internal bool HasSelectedSubmenu(string key) => _selectedKeys.Any(selected =>
            _menuKeyPaths.TryGetValue(selected, out var path) && path.Contains(key));
        internal string OverflowKey => $"{Id}-overflow-indicator";
        internal string GetMenuDataId(string key) => $"rc-menu-uuid-{Id}-{(key == OverflowKey ? "rc-menu-more" : key)}";
        internal string GetSubMenuPopupId(string key) => $"{GetMenuDataId(key)}-popup";
        private string OverflowRootStyle => Style;

        internal void RememberMenuPath(MenuItem item)
        {
            var path = new List<string>();
            var parent = item.ParentMenu;
            while (parent != null)
            {
                if (!parent.IsOverflowIndicator) path.Add(parent.Key);
                parent = parent.Parent;
            }
            _menuKeyPaths[item.Key] = path.ToArray();
            _overflowOwners[item.Key] = item.OverflowSourceId ?? item.OverflowGroupId ?? item.Id;
        }

        internal void RegisterOverflowItem(string id, RenderFragment content, Action refresh)
        {
            if (_overflowItems.Any(item => item.Id == id)) return;
            _overflowItems.Add(new OverflowItem { Id = id, Content = content, Refresh = refresh });
        }

        internal void UnregisterOverflowItem(string id)
        {
            _overflowItems.RemoveAll(item => item.Id == id);
            _overflowIds.Remove(id);
            foreach (var key in _overflowOwners.Where(pair => pair.Value == id).Select(pair => pair.Key).ToArray())
            {
                _overflowOwners.Remove(key);
                _menuKeyPaths.Remove(key);
            }
        }

        internal bool IsOverflowed(string id) => id != null && OverflowEnabled && _overflowIds.Contains(id);

        internal string OverflowItemId(string id, bool topLevel) => OverflowEnabled && topLevel ? id : null;

        internal string OverflowItemStyle(string id, string style, bool indicator = false)
        {
            if (!OverflowEnabled) return style;
            var hidden = indicator ? _overflowIds.Count == 0 : _overflowIds.Contains(id);
            var index = _overflowItems.FindIndex(item => item.Id == id);
            if (!indicator && index < 0) return style;
            var order = indicator
                ? hidden ? int.MaxValue : Math.Max(0, _overflowItems.Count - _overflowIds.Count - 1)
                : index;
            var baseStyle = style?.Trim().TrimEnd(';');
            var visibilityStyle = hidden
                ? $"opacity:0;height:0px;overflow-y:hidden;order:{order};pointer-events:none;position:absolute;"
                : $"opacity:1;order:{order};";
            // Keep the real item measurable after it moves into the rest menu.
            // Blazor retains ownership; JavaScript never reparents its DOM.
            return string.IsNullOrEmpty(baseStyle) ? visibilityStyle : $"{baseStyle};{visibilityStyle}";
        }

        /// <summary>Receives the measured overflow keys from the browser.</summary>
        [JSInvokable]
        public Task SetOverflowItems(string[] ids) => InvokeAsync(() =>
        {
            if (IsDisposed || !OverflowEnabled) return;
            var knownIds = _overflowItems.Select(item => item.Id).ToHashSet(StringComparer.Ordinal);
            var next = new HashSet<string>((ids ?? Array.Empty<string>()).Where(knownIds.Contains), StringComparer.Ordinal);
            var order = (ids ?? Array.Empty<string>()).Where(knownIds.Contains).Distinct().ToArray();
            if (_overflowIds.SetEquals(next) && _overflowOrder.SequenceEqual(order)) return;
            _overflowIds = next;
            _overflowOrder = order;
            foreach (var item in _overflowItems.ToArray()) item.Refresh();
            StateHasChanged();
        });

        internal Task CloseOverflowAsync() => _overflowSubmenu == null ? Task.CompletedTask : _overflowSubmenu.Collapse();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (IsDisposed) return;
            if (OverflowEnabled) _overflowReference ??= DotNetObjectReference.Create(this);
            if (_overflowReference != null)
                await JsInvokeAsync("AntDesign.interop.menuOverflowHelper.observe", Ref, _overflowReference, OverflowEnabled);
        }

        /// <summary>Disconnect the measurement observer before releasing its .NET callback.</summary>
        public async ValueTask DisposeAsync()
        {
            if (IsDisposed) return;
            Dispose();
            try
            {
                if (_overflowReference != null)
                    await JsInvokeAsync("AntDesign.interop.menuOverflowHelper.unobserve", Ref);
            }
            catch (JSDisconnectedException) { }
            catch (TaskCanceledException) { }
            finally { _overflowReference?.Dispose(); }
        }
    }
}
