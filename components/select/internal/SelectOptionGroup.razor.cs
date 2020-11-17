using Microsoft.AspNetCore.Components;

namespace AntDesign.Select.Internal
{
    public partial class SelectOptionGroup<TItemValue, TItem>
    {
        private const string ClassNamePrefix = "ant-select-item-group";
        [CascadingParameter] internal Select<TItemValue, TItem> SelectParent { get; set; }
        string _oldGroupName = string.Empty;

        protected override void OnInitialized()
        {
            SetClassMap();
            base.OnInitialized();
        }

        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add("ant-select-item")
                .Add(ClassNamePrefix);
        }
    }
}
