using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    public class DrawerRef
    {
        private DrawerConfig _config;
        private DrawerService _service;

        internal DrawerRef(DrawerConfig config)
        {
            _config = config;
        }

        internal DrawerRef(DrawerConfig config, DrawerService service)
        {
            _config = config;
            _service = service;
        }

        /// <summary>
        /// clsoe the drawer
        /// </summary>
        /// <returns></returns>
        public Task CloseAsync()
        {
            return _config.HandleOnClose();
        }

    }
}
