// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class PaginationEventArgs
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public PaginationEventArgs(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public void Deconstruct(out int page, out int pageSize)
        {
            page = Page;
            pageSize = PageSize;
        }
    }
}
