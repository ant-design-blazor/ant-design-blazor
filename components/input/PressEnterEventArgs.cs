using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class PressEnterEventArgs : KeyboardEventArgs
    {
        private bool _preventLineBreak = false;

        public bool ShouldPreventLineBreak => _preventLineBreak;

        public PressEnterEventArgs()
        {
        }

        public PressEnterEventArgs(KeyboardEventArgs args)
        {
            Key = args.Key;
            Code = args.Code;
            ShiftKey = args.ShiftKey;
            CtrlKey = args.CtrlKey;
            MetaKey = args.MetaKey;
            AltKey = args.AltKey;
            Repeat = args.Repeat;
            Location = args.Location;
            Type = args.Type;
        }

        /// <summary>
        /// Prevent the line break from being added to the input value
        /// </summary>
        public void PreventLineBreak()
        {
            _preventLineBreak = true;
        }
    }
}
