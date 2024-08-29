// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public interface IAutoCompleteRef
    {
        void SetInputComponent(IAutoCompleteInput input);

        Task InputFocus(FocusEventArgs e);

        Task InputInput(ChangeEventArgs args);

        Task InputValueChange(string value);

        Task InputKeyDown(KeyboardEventArgs args);

        void AddOption(AutoCompleteOption option);

        void RemoveOption(AutoCompleteOption option);

        void SetActiveItem(AutoCompleteOption item);

        Task SetSelectedItem(AutoCompleteOption item);

        Func<object, object, bool> CompareWith { get; set; }

        object SelectedValue { get; set; }

        object ActiveValue { get; set; }
    }
}
