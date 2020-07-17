using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace vintriTechnologies.Helper
{
    public class RequestGenerator
    {
        private readonly IHttpClientFactory _clientFactory;
        public RequestGenerator(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;
        }
        public async Task<HttpResponseMessage> Get(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("x-ratelimit-limit", "3600");
            request.Headers.Add("x-ratelimit-remaining", "3587");

            var client = _clientFactory.CreateClient();

            return await client.SendAsync(request);

        }
    }
}
