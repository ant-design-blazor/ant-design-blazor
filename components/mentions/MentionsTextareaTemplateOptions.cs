using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class MentionsTextareaTemplateOptions
    {
        public string Value { get; set; }
        public ForwardRef RefBack { get; set; }
        public Action<KeyboardEventArgs> OnKeyDown { get; set; }
        public Func<ChangeEventArgs, Task> OnInput { get; set; }
    }
}
