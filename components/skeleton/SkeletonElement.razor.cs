using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class SkeletonElement : AntDomComponentBase
    {
        #region Parameters

        [Parameter]
        public bool Active { get; set; } = false;

        [Parameter]
        public string Type { get; set; }

        [Parameter]
        public OneOf<int?, string> Size { get; set; } = "default";

        [Parameter]
        public string Shape { get; set; } = SkeletonButtonShape.Default;

        #endregion Parameters

        private ClassMapper _spanClassMapper = new ClassMapper();

        private string _spanStyle = "";

        private void SetClassMap()
        {
            ClassMapper
                .Add("ant-skeleton")
                .If("ant-skeleton-element", () => true)
                .If("ant-skeleton-active", () => Active)
                .If("ant-skeleton-rtl", () => RTL);
        }

        protected override void OnInitialized()
        {
            SetClassMap();

            if (Type == SkeletonElementType.Button)
                SetButtonMap();
            else if (Type == SkeletonElementType.Avatar)
                SetAvatarMap();
            else if (Type == SkeletonElementType.Input)
                SetInputMap();
        }

        private void SetButtonMap()
        {
            _spanClassMapper.Clear().If("ant-skeleton-button", () => true)
                .If("ant-skeleton-button-round", () => Shape == SkeletonButtonShape.Round)
                .If("ant-skeleton-button-circle", () => Shape == SkeletonButtonShape.Circle)
                .If("ant-skeleton-button-lg", () => Size.AsT1 == SkeletonElementSize.Large)
                .If("ant-skeleton-button-sm", () => Size.AsT1 == SkeletonElementSize.Small);
        }

        private void SetAvatarMap()
        {
            _spanClassMapper.Clear().If("ant-skeleton-avatar", () => true)
                .If("ant-skeleton-avatar-square", () => Shape == SkeletonAvatarShape.Square)
                .If("ant-skeleton-avatar-circle", () => Shape == SkeletonAvatarShape.Circle)
                .If("ant-skeleton-avatar-lg", () => Size.AsT1 == SkeletonElementSize.Large)
                .If("ant-skeleton-avatar-sm", () => Size.AsT1 == SkeletonElementSize.Small);
        }

        private void SetInputMap()
        {
            _spanClassMapper.Clear().If("ant-skeleton-input", () => true)
               .If("ant-skeleton-input-lg", () => Size.AsT1 == SkeletonElementSize.Large)
               .If("ant-skeleton-input-sm", () => Size.AsT1 == SkeletonElementSize.Small);
        }

        protected override void OnParametersSet()
        {
            if (Type == SkeletonElementType.Avatar)
            {
                if (Size.IsT0 == true)
                {
                    var sideLength = $"{Size.IsT0}px";
                    Style = $"width: {sideLength}, height: {sideLength}, 'line-height': {sideLength} ";
                }
            }
            StateHasChanged();
            base.OnParametersSet();
        }
    }
}
