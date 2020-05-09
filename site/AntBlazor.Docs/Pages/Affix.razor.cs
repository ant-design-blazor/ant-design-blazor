using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor.Docs.Pages
{
    public partial class Affix
    {
        private uint _offsetTop = 10;
        private uint _offsetBottom = 10;

        private void AddTop()
        {
            _offsetTop += 10;
        }

        private void AddBottom()
        {
            _offsetBottom += 10;
        }

        private void OnAffixChange(bool affixed)
        {
            Debug.WriteLine(affixed);
        }

        private ElementReference _container;
    }
}
