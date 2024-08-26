using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    internal interface IAutoCompleteInput
    {
        IAutoCompleteRef Component { get; set; }
        void SetValue(object value);
    }
}
