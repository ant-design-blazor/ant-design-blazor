using Microsoft.AspNetCore.Components;
using OneOf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    public class ListGridType
    {
        public int Gutter { get; set; }
        public int Column { get; set; }
        public int Xs { get; set; }
        public int Sm { get; set; }
        public int Md { get; set; }
        public int Lg { get; set; }
        public int Xl { get; set; }
        public int Xxl { get; set; }
    }

    public partial class AntList<TItem> : AntDomComponentBase
    {
        public string PrefixName { get; set; } = "ant-list";

        [Parameter] public RenderFragment<TItem> Item { get; set; }

        [Parameter] public IEnumerable<TItem> DataSource { get; set; }

        [Parameter] public bool Bordered { get; set; } = false;

        [Parameter] public RenderFragment Header { get; set; }

        [Parameter] public RenderFragment Footer { get; set; }

        [Parameter] public RenderFragment LoadMore { get; set; }

        [Parameter] public AntDirectionVHType ItemLayout { get; set; }

        [Parameter] public bool Loading { get; set; } = false;

        [Parameter] public string NoResult { get; set; }

        [Parameter] public string Size { get; set; } = AntSizeLDSType.Default;

        [Parameter] public bool Split { get; set; } = true;

        [Parameter] public string ClassName { get; set; }

        [Parameter] public EventCallback<TItem> OnItemClick { get; set; }

        [Parameter] public ListGridType Grid { get; set; }

        [Parameter] public List<RenderFragment> Actions { get; set; } = new List<RenderFragment>();

        [Parameter] public RenderFragment Extra { get; set; }

        [Parameter] public PaginationOptions Pagination { get; set; }

        private static readonly EventCallbackFactory CallbackFactory = new EventCallbackFactory();

        private bool IsSomethingAfterLastItem
        {
            get
            {
                return LoadMore != null || Footer != null || Pagination != null;
            }
        }

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
                .Add(PrefixName)
                .Add(ClassName)
                .If($"{PrefixName}-split", () => Split)
                .If($"{PrefixName}-bordered", () => Bordered)
                .If($"{PrefixName}-{sizeCls}", () => !string.IsNullOrEmpty(sizeCls))
                .If($"{PrefixName}-vertical", () => ItemLayout == AntDirectionVHType.Vertical)
                .If($"{PrefixName}-loading", () => (Loading))
                .If($"{PrefixName}-grid", () => Grid != null)
                .If($"{PrefixName}-something-after-last-item", () => IsSomethingAfterLastItem);
        }

        private void HandleItemClick(TItem item)
        {
            if (OnItemClick.HasDelegate)
            {
                OnItemClick.InvokeAsync(item);
            }
        }
    }
}
