// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AntDesign.Docs.Demos.Components.TreeSelect.Demo
{
    public class WebMenu
    {

        public string MenuId { get; set; }

        public string MenuHref { get; set; }

        public string MenuName { get; set; }

        public string Target { get; set; }

        public string ParentId { get; set; }

        public string Right { get; set; }

        public int? OrderBy { get; set; } = 9999;

    }

    public static class LocalJsonData
    {
        private static readonly IList<WebMenu> _data;

        static LocalJsonData()
        {
            _data = new List<WebMenu>() {
                new WebMenu(){ MenuId="1", MenuName="parent 1", ParentId="0", OrderBy=10},
                new WebMenu(){ MenuId="2", MenuName="parent 1-0", ParentId="1", OrderBy=10},
                new WebMenu(){ MenuId="3", MenuName="leaf1", ParentId="2", OrderBy=10},
                new WebMenu(){ MenuId="4", MenuName="leaf2", ParentId="0", OrderBy=10},
                new WebMenu(){ MenuId="5", MenuName="parent1-1", ParentId="0", OrderBy=10},
                new WebMenu(){ MenuId="6", MenuName="leaf3", ParentId="5", OrderBy=10}
            };
        }

        public static IList<WebMenu> Data => _data;


        public static IList<WebMenu> GetMenusByParentId(string parentId)
        {
            return _data.Where(m => m.ParentId == parentId).ToList();
        }

        public static bool HasChild(string parentId)
        {
            return _data.All(m => m.ParentId != parentId);
        }
    }

}
