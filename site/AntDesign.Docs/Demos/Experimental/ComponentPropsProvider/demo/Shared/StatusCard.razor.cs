using Microsoft.AspNetCore.Components;

namespace AntDesign.Docs.Demos.Experimental.ComponentPropsProvider.demo.Shared;

public partial class StatusCard : AntComponentBase
{
    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public string Description { get; set; }

    [Parameter]
    public bool Highlighted { get; set; }
}
