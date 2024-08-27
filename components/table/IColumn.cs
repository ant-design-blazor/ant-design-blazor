// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign.TableModels;

namespace AntDesign
{
    public interface IColumn
    {
        public ITable Table { get; set; }

        public bool IsInitialize { get; set; }

        public bool IsHeader { get; set; }

        public bool IsPlaceholder { get; set; }

        public bool IsColGroup { get; set; }

        public int ColIndex { get; set; }

        public string Fixed { get; set; }

        public string Title { get; set; }

        public string Width { get; set; }

        public RowData RowData { get; set; }

        public int ColSpan { get; set; }

        public int RowSpan { get; set; }

        public int HeaderColSpan { get; set; }

        internal void UpdateFixedStyle();
    }
}
