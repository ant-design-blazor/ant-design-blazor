using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AntDesign.Docs.Services
{
    public class HttpClientCustom : DelegatingHandler
    {
        private const string UserAgentValue = "BlazorApp";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Add the User-Agent header if it's not already present
            if (!request.Headers.Contains("User-Agent"))
            {
                request.Headers.TryAddWithoutValidation("User-Agent", UserAgentValue);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
