using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace AntDesign
{
    public interface IRowSelection
    {
        public int Index { get; set; }

        public bool Checked { get; set; }

        public bool Disabled { get; set; }

        public void Check(bool @checked);

        public IList<IRowSelection> RowSelections { get; set; }
    }
}
