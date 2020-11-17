using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public interface IAutoCompleteInputRef
    {
        void SetInputComponent(IAutoCompleteInput input);

        void InputFocus(FocusEventArgs e);

        Task InputInput(ChangeEventArgs args);

        Task InputKeyDown(KeyboardEventArgs args);

    }

    public interface IAutoCompleteRef<TOption>
    {
        void AddOption(AutoCompleteOption<TOption> option);

        void RemoveOption(AutoCompleteOption<TOption> option);

        void SetActiveItem(AutoCompleteOption<TOption> item);

        Task SetSelectedItem(AutoCompleteOption<TOption> item);

        Func<TOption, TOption, bool> CompareWith { get; set; }

        TOption SelectedValue { get; set; }

        TOption ActiveValue { get; set; }
    }

    public interface IAutoCompleteValue<TOption>
    {

    }
}
