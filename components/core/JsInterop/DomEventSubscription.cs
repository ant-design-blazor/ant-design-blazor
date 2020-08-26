using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign.JsInterop
{
    internal class DomEventSubscription
    {
        internal Delegate Delegate { get; set; }

        public DomEventSubscription(Delegate @delegate)
        {
            Delegate = @delegate;
        }
    }
}
