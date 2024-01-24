using Flurl.Http;
using Flurl.Http.Configuration;

using System.Net.NetworkInformation;
using System.Net;
using System.Threading.Tasks;
using System;

namespace nchsapp.Services
{
    public class GoogleService : IGoogleService, IDisposable
    {
        private readonly IFlurlClient _flurlCli;
        private readonly Pinger _pinger;
        public IPAddress IPAddress { get =>  IPAddress.Parse(_flurlCli.BaseUrl); }

        public GoogleService(IFlurlClientCache clients, Pinger pinger)
        {
            _flurlCli = clients.Get("GoogleClient");
            _pinger = pinger;
        }

        public async Task<IPStatus> Ping() => await _pinger.Ping(IPAddress);

        public void Dispose()
        {
            _flurlCli.Dispose();
            _pinger.Dispose();
        }
    }

}