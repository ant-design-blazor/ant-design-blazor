using System;

namespace AntDesign.JsInterop
{
    internal class DomEventSubscription
    {
        internal Delegate Delegate { get; set; }
        internal Type Type { get; set; }

        public DomEventSubscription(Delegate @delegate, Type type)
        {
            Delegate = @delegate;
            Type = type;
        }
    }
}
