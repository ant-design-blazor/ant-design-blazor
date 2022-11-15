using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
#if NET7_0_OR_GREATER
#else
    [EventHandler("onmouseleave", typeof(EventArgs))]
    [EventHandler("onmouseenter", typeof(EventArgs))]

    public static class EventHandlers
    {
    }
#endif
}
