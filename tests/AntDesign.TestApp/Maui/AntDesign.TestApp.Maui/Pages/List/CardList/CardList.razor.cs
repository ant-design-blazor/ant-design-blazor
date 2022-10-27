using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.TestApp.Maui.Models;
using AntDesign.TestApp.Maui.Services;
using Microsoft.AspNetCore.Components;
using AntDesign;

namespace AntDesign.TestApp.Maui.Pages.List
{
    public partial class CardList
    {
        private readonly ListGridType _listGridType = new ListGridType
        {
            Gutter = 16,
            Xs = 1,
            Sm = 2,
            Md = 3,
            Lg = 3,
            Xl = 4,
            Xxl = 4
        };

        private ListItemDataType[] _data = { };

        [Inject] protected IProjectService ProjectService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var list = new List<ListItemDataType> {new ListItemDataType()};
            var data = await ProjectService.GetFakeListAsync(8);
            list.AddRange(data);
            _data = list.ToArray();
        }
    }
}