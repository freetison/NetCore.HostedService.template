using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ncwsapp.DependencyInjection;
using Polly;
using RestSharp;

namespace ncwsapp.Services
{
    public class HttpService : IHttpService
    {
        private readonly IRestClient _restClient;
        private readonly Pinger _pinger;


        public HttpService(IRestClient restClient, IOptions<HttpServiceOptions> options, Pinger pinger)
        {
            _restClient = restClient;
            _pinger = pinger;
            _restClient.BaseUrl = new Uri(options.Value.BaseUrl);
        }

        public async Task<string> GetData(Uri uri = null)
        {
            if (uri != null) { _restClient.BaseUrl = uri; }

            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = await _restClient.ExecuteAsync(request);

            Console.WriteLine(response.Content);

            return response.Content;
        }

        public async Task<IPStatus> Ping(IPAddress address) => await _pinger.Ping(address);
    }
}
