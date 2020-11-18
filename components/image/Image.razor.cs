using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Image : AntDomComponentBase
    {
        [Parameter]
        public string Alt { get; set; }

        [Parameter]
        public string Fallback { get; set; }

        [Parameter]
        public string Height { get; set; }

        [Parameter]
        public string Width { get; set; }

        [Parameter]
        public RenderFragment Placeholder { get; set; }

        [Parameter]
        public bool Preview { get; set; }

        [Parameter]
        public string Src { get; set; }

        private bool _isError;

        private string _imgStyle;

        private bool _loaded;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!string.IsNullOrWhiteSpace(Width))
            {
                Style = $"width:{(CssSizeLength)Width};" + Style;
            }

            if (!string.IsNullOrWhiteSpace(Height))
            {
                Style = $"height:{(CssSizeLength)Height};" + Style;
                _imgStyle = $"height:{(CssSizeLength)Height};" + _imgStyle;
            }

            ClassMapper.Add("ant-image")
                .If("ant-image-error", () => _isError);
        }

        private void HandleOnError()
        {
            _isError = true;
            Src = Fallback;
        }

        private void HandleOnLoad()
        {
            _loaded = true;
        }

        private void HandleOnLoadStart()
        {
            _loaded = false;
        }
    }
}
