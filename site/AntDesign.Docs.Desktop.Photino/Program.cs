using System;
using Photino.Blazor;

namespace AntDesign.Docs.Desktop.Photino
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ComponentsDesktop.Run<Startup>("Ant Design of Blazor"
                , "wwwroot/index.html"
                , x: 450
                , y: 100
                , width: 1000
                , height: 900);
        }
    }
}
