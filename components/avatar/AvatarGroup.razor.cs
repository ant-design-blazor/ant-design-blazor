using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class AvatarGroup : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public int MaxCount { get; set; }

        [Parameter]
        public string MaxStyle { get; set; }

        [Parameter]
        public PlacementType MaxPopoverPlacement { get; set; } = PlacementType.Top;

        private ClassMapper _popoverClassMapper = new ClassMapper();

        private bool _overflow;
        private string _prefixCls = "ant-avatar-group";

        private IList<Avatar> _shownAvatarList = new List<Avatar>();
        private IList<Avatar> _hiddenAvatarList = new List<Avatar>();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper
                .Add(_prefixCls)
                .If($"{_prefixCls}-rtl", () => RTL);

            _popoverClassMapper
                .Add($"{_prefixCls}-popover")
                .If($"{_prefixCls}-popover-rtl", () => RTL);
        }

        internal void AddAvatar(Avatar item)
        {
            if (item.Position == null)
                return;

            var avatarList = item.Position == "shown" ? _shownAvatarList : _hiddenAvatarList;

            avatarList.Add(item);

            if (MaxCount > 0 && avatarList.Count > MaxCount)
            {
                _overflow = true;
                item.Overflow = true;
            }

            StateHasChanged();
        }

        internal void RemoveAvatar(Avatar item)
        {
            if (item.Position == null)
                return;

            var avatarList = item.Position == "shown" ? _shownAvatarList : _hiddenAvatarList;

            avatarList.Remove(item);
        }
    }
}
