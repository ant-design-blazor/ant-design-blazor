// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ConfigProvider : AntComponentBase
    {
        [Parameter]
        public string Direction
        {
            get => _direction;
            set
            {
                if (_direction != value)
                {
                    _direction = value;
                    _waitingDirectionUpdate = true;
                }
            }
        }

        [Parameter]
        public GlobalTheme Theme
        {
            get => _globalTheme;
            set
            {
                if (_globalTheme != value)
                {
                    _globalTheme = value;
                    _waitingGlobalThemeUpdate = true;
                }
            }
        }

        [Parameter]
        public FormConfig Form { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        private ConfigService ConfigService { get; set; }

        private string _direction;

        private GlobalTheme _globalTheme;

        private bool _waitingDirectionUpdate;
        private bool _waitingGlobalThemeUpdate;
        private bool _afterFirstRender;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                _afterFirstRender = true;
            }

            if (_afterFirstRender)
            {
                if (_waitingDirectionUpdate)
                {
                    _waitingDirectionUpdate = false;
                    await ChangeDirection(_direction);
                }

                if (_waitingGlobalThemeUpdate)
                {
                    _waitingGlobalThemeUpdate = false;
                    await ChangeGlobalTheme(_globalTheme);
                }
            }
        }

        public async Task ChangeDirection(string direction)
        {
            _direction = direction?.ToUpperInvariant();
            await ConfigService.ChangeDirection(_direction);
            await InvokeAsync(StateHasChanged);
        }

        public async Task ChangeGlobalTheme(GlobalTheme mode)
        {
            _globalTheme = mode;
            await ConfigService.SetTheme(mode);
            await InvokeAsync(StateHasChanged);
        }
    }
}
