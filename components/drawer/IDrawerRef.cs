using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    public interface IDrawerRef
    {
        DrawerOptions Config { get; set; }
        Drawer Drawer { get; set; }

        Task CloseAsync();

        Func<DrawerClosingEventArgs, Task> OnClosing { get; set; }
    }
}
