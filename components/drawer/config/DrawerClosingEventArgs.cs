using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class DrawerClosingEventArgs
    {
        private bool _rejected;

        public DrawerClosingEventArgs() { }

        public DrawerClosingEventArgs(bool reject)
        {
            _rejected = reject;
        }

        internal bool Rejected { get => _rejected; }

        /// <summary>
        /// Reject to close 
        /// </summary>
        public void Reject()
        {
            _rejected = true;
        }
    }
}
