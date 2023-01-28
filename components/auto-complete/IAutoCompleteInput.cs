using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public interface IAutoCompleteInput
    {
        [CascadingParameter]
        public IAutoCompleteRef Component { get; set; }
        public void SetValue(object value);
    }
}
