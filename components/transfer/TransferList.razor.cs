using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class TransferList : AntDomComponentBase
    {
        [CascadingParameter]
        private Transfer Transfer { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public TransferRenderProps TransferRenderProps { get; set; }

        [Parameter]
        public IEnumerable<TransferItem> DataSource { get; set; }

        public bool Disabled => TransferRenderProps.Disabled;

        private const string DisabledClass = "ant-transfer-list-content-item-disabled";
        private const string FooterClass = "ant-transfer-list-with-footer";

        private readonly string _filterValue;

        private readonly List<string> _selectedKeys;

        private readonly string _countText = string.Empty;

        private readonly bool _buttonDisabled = true;

        private readonly List<string> _targetKeys;

        private readonly bool _leftCheckAllState;
        private readonly bool _leftCheckAllIndeterminate;
        private readonly bool _rightCheckAllState;
        private readonly bool _rightCheckAllIndeterminate;

        private record Stat(bool CheckAll, bool CheckHalf, int CheckCount, int ShownCount);

        private readonly Stat _stat = new(false, false, 0, 0);

        private async Task SelectItem(bool isCheck, string key)
        {

        }

        private async Task SelectAll(bool isCheck)
        {

        }

        private void HandleSearch(string searchValue)
        {

        }
    }
}
