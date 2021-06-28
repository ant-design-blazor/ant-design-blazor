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

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Inject] public ConfigService ConfigService { get; set; }

        private string _direction;

        private bool _waitingDirectionUpdate;
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
            }
        }

        public async Task ChangeDirection(string direction)
        {
            _direction = direction?.ToUpperInvariant();
            await ConfigService.ChangeDirection(_direction);
            await InvokeAsync(StateHasChanged);
        }
    }
}
