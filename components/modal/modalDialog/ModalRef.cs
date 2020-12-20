using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    public class ModalRef
    {
        internal readonly ModalOptions Config;
        private readonly ModalService _service;

        internal ModalRef(ModalOptions config, ModalService modalService)
        {
            Config = config;
            _service = modalService;
        }

        /// <summary>
        /// open the Modal dialog
        /// </summary>
        /// <returns></returns>
        public async Task OpenAsync()
        {
            if (!Config.Visible)
            {
                Config.Visible = true;
            }

            await _service.CreateOrOpenModalAsync(this);
        }

        /// <summary>
        /// close the Modal dialog
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync()
        {
            await _service.CloseModalAsync(this);
        }
    }
}
