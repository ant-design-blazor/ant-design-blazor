using System;
using System.Collections.Generic;
using System.Text;
using AntDesign.Docs.Localization;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Docs.Shared
{
    public partial class FixedWidgets
    {
        [Inject] public ILanguageService Language { get; set; }

        private void ChangeTheme(string theme)
        {
        }
    }
}
