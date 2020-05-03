using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class AntList : AntDomComponentBase
    {
        public string _prefixName = "ant-list";

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public RenderFragment Header { get; set; }

        [Parameter] public RenderFragment Footer { get; set; }

        [Parameter] public IEnumerable<object> DataSource { get; set; }

        [Parameter] public AntDirectionVHType ItemLayout { get; set; } = AntDirectionVHType.horizontal;

        [Parameter] public bool Loading { get; set; } = false;

        [Parameter] public string LoadMore { get; set; }

        [Parameter] public string NoResult { get; set; }

        [Parameter] public string Pagination { get; set; }

        [Parameter] public string Size { get; set; } = AntSizeLDSType.Default;

        [Parameter] public bool Split { get; set; } = true;

        [Parameter] public string ClassName { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            SetClassMap();
        }

        protected void SetClassMap()
        {
            // large => lg
            // small => sm
            string sizeCls = string.Empty;
            switch (Size)
            {
                case "large":
                    sizeCls = "lg";
                    break;
                case "small":
                    sizeCls = "sm";
                    break;
                default:
                    break;
            }

            ClassMapper.Clear()
                .Add(_prefixName)
                .Add(ClassName)
                .Add($"{_prefixName}-split")
                .Add($"{_prefixName}-bordered")
                .If($"{_prefixName}-{sizeCls}", () => !string.IsNullOrEmpty(sizeCls))
                .If($"{_prefixName}-vertical", () => ItemLayout == AntDirectionVHType.vertical)
                .If($"{_prefixName}-loading", () => (Loading))
                .If($"{_prefixName}-grid", () => true)
                .If($"{_prefixName}-something-after-last-item", () => false);
        }
    }
}
