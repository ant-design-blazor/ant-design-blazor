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
        public GlobalThemeMode GlobalThemeMode
        {
            get => _globalthememode;
            set
            {
                if (_globalthememode != value)
                {
                    _globalthememode = value;
                    _waitingGlobalThemeModeUpdate = true;
                }
            }
        }

        [Parameter]
        public FormConfig Form { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Inject] public ConfigService ConfigService { get; set; }

        [Inject] public GlobalThemeService GlobalThemeService { get; set; }

        private string _direction;

        public GlobalThemeMode _globalthememode;

        private bool _waitingDirectionUpdate;
        private bool _waitingGlobalThemeModeUpdate;
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

                if (_waitingGlobalThemeModeUpdate)
                {
                    _waitingGlobalThemeModeUpdate = false;
                    await ChangeDirection(_direction);
                }
            }
        }

        public async Task ChangeDirection(string direction)
        {
            _direction = direction?.ToUpperInvariant();
            await ConfigService.ChangeDirection(_direction);
            await InvokeAsync(StateHasChanged);
        }

        public async Task ChangeGlobalTheme(GlobalThemeMode mode)
        {
            _globalthememode = mode;
            await GlobalThemeService.UseTheme(mode);
            await InvokeAsync(StateHasChanged);
        }
    }
}
