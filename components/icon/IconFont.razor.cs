using System.Threading.Tasks;

namespace AntDesign
{
    public class IconFont : Icon
    {
        protected override Task SetupSvgImg()
        {
            var svg = $"<svg><use xlink:href=#{Type} /></svg>";
            _svgImg = IconService.GetStyledSvg(svg, null, Width, Height, Fill, Rotate);

            StateHasChanged();

            return Task.CompletedTask;
        }
    }
}
