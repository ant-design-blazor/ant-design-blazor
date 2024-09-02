﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class AvatarGroup : AntDomComponentBase
    {
        /// <summary>
        /// The child content
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Max avatars to show
        /// </summary>
        [Parameter]
        public int MaxCount { get; set; }

        /// <summary>
        /// The style of excess avatar style
        /// </summary>
        [Parameter]
        public string MaxStyle { get; set; }

        /// <summary>
        /// The placement of excess avatar Popover
        /// </summary>
        [Parameter]
        public Placement MaxPopoverPlacement { get; set; } = Placement.Top;

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
