using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TComponentOptions"></typeparam>
    public abstract class TemplateComponentBase<TComponentOptions> : AntComponentBase
    {
        /// <summary>
        /// The options that allow you to pass in templates from the outside
        /// </summary>
        [Parameter]
        public TComponentOptions Options { get; set; }
    }
}
