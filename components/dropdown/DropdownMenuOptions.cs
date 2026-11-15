// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign;

/// <summary>Data-driven menu configuration accepted by <see cref="Dropdown.Menu"/>.</summary>
public sealed class DropdownMenuOptions
{
    /// <summary>Menu entries, including nested submenus, groups, and dividers.</summary>
    public IReadOnlyList<DropdownMenuItem> Items { get; set; } = Array.Empty<DropdownMenuItem>();

    /// <summary>Menu color theme.</summary>
    public MenuTheme Theme { get; set; } = MenuTheme.Light;

    /// <summary>Menu layout mode.</summary>
    public MenuMode Mode { get; set; } = MenuMode.Vertical;

    /// <summary>Whether menu entries can be selected.</summary>
    public bool Selectable { get; set; } = true;

    /// <summary>Whether multiple entries can be selected.</summary>
    public bool Multiple { get; set; }

    /// <summary>Currently active menu item key, independent of selected keys.</summary>
    public string ActiveKey { get; set; }

    /// <summary>Indent size for each nested level when <see cref="Mode"/> is inline.</summary>
    public int InlineIndent { get; set; } = 24;

    /// <summary>Initially selected menu entry keys.</summary>
    public IReadOnlyList<string> DefaultSelectedKeys { get; set; } = Array.Empty<string>();

    /// <summary>Controlled selected menu entry keys.</summary>
    public string[] SelectedKeys { get; set; }

    /// <summary>Callback when selected menu entry keys change.</summary>
    public EventCallback<string[]> SelectedKeysChanged { get; set; }

    /// <summary>Initially expanded submenu keys.</summary>
    public IReadOnlyList<string> DefaultOpenKeys { get; set; } = Array.Empty<string>();

    /// <summary>Controlled expanded submenu keys.</summary>
    public string[] OpenKeys { get; set; }

    /// <summary>Callback when expanded submenu keys change.</summary>
    public EventCallback<string[]> OpenKeysChanged { get; set; }

    /// <summary>Callback when expanded submenu keys change.</summary>
    public EventCallback<string[]> OnOpenChange { get; set; }

    /// <summary>Callback when a menu entry is clicked.</summary>
    public EventCallback<DropdownMenuClickInfo> OnClick { get; set; }
}

/// <summary>A data-driven Dropdown menu entry.</summary>
public sealed class DropdownMenuItem
{
    /// <summary>Menu entry key.</summary>
    public string Key { get; set; }

    /// <summary>Plain text label.</summary>
    public string Label { get; set; }

    /// <summary>Rich label content; takes precedence over <see cref="Label"/>.</summary>
    public RenderFragment LabelTemplate { get; set; }

    /// <summary>Ant Design icon type.</summary>
    public string Icon { get; set; }

    /// <summary>Rich icon content; takes precedence over <see cref="Icon"/>.</summary>
    public RenderFragment IconTemplate { get; set; }

    /// <summary>Optional title attribute.</summary>
    public string Title { get; set; }

    /// <summary>Whether this entry is disabled.</summary>
    public bool Disabled { get; set; }

    /// <summary>Whether this entry uses the danger style.</summary>
    public bool Danger { get; set; }

    /// <summary>Entry kind. The default is a normal menu item.</summary>
    public DropdownMenuItemKind Type { get; set; }

    /// <summary>Additional CSS class for this menu entry.</summary>
    public string Class { get; set; }

    /// <summary>Inline style for this menu entry.</summary>
    public string Style { get; set; }

    /// <summary>Whether a divider uses a dashed border.</summary>
    public bool Dashed { get; set; }

    /// <summary>Nested entries for submenus and groups.</summary>
    public IReadOnlyList<DropdownMenuItem> Children { get; set; } = Array.Empty<DropdownMenuItem>();
}

/// <summary>Supported data-driven Dropdown menu entry kinds.</summary>
public enum DropdownMenuItemKind
{
    Item,
    SubMenu,
    Group,
    Divider,
}

/// <summary>Information about a clicked Dropdown menu entry.</summary>
public sealed class DropdownMenuClickInfo
{
    internal DropdownMenuClickInfo(string key, IReadOnlyList<string> keyPath, MouseEventArgs domEvent)
    {
        Key = key;
        KeyPath = keyPath;
        DomEvent = domEvent;
    }

    /// <summary>Clicked entry key.</summary>
    public string Key { get; }

    /// <summary>Clicked entry key followed by its submenu ancestor keys.</summary>
    public IReadOnlyList<string> KeyPath { get; }

    /// <summary>Original click event.</summary>
    public MouseEventArgs DomEvent { get; }
}
