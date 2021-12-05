using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AntDesign.Docs.Desktop.Photino
{
    public class HttpHandler : DelegatingHandler
    {
        Uri _baseUri = new("http://app/");

        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var requestUri = request.RequestUri;
            if (_baseUri.IsBaseOf(requestUri))
            {
                HttpResponseMessage responseMessage = new(System.Net.HttpStatusCode.OK);
                responseMessage.Content = new StreamContent(File.OpenRead("wwwroot" + requestUri.PathAndQuery));
                return responseMessage;
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
