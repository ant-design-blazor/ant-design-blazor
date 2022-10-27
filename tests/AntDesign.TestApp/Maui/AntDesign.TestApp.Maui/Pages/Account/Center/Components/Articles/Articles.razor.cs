using System.Collections.Generic;
using AntDesign.TestApp.Maui.Models;
using Microsoft.AspNetCore.Components;

namespace AntDesign.TestApp.Maui.Pages.Account.Center
{
    public partial class Articles
    {
        [Parameter] public IList<ListItemDataType> List { get; set; }
    }
}