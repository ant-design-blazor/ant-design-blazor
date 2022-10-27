using System.Collections.Generic;
using AntDesign.TestApp.Maui.Models;
using Microsoft.AspNetCore.Components;
using AntDesign;

namespace AntDesign.TestApp.Maui.Pages.Account.Center
{
    public partial class Projects
    {
        private readonly ListGridType _listGridType = new ListGridType
        {
            Gutter = 24,
            Column = 4
        };

        [Parameter]
        public IList<ListItemDataType> List { get; set; }
    }
}