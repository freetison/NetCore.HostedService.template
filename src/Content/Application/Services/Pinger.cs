using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace nchsapp.Services
{
    public class Pinger
    {
        public string Result = String.Empty;

        private readonly Ping _pinger;
        private readonly AutoResetEvent _waiter;
        private readonly PingOptions _options;

        const string Data = "01234567890123456789012345678901";
        const int Timeout = 12000;
        const int Ttl = 64;


        public Pinger()
        {
            _pinger = new Ping();
            _waiter = new AutoResetEvent(false);
            _options = new PingOptions(Ttl, true);
        }

        public async Task<IPStatus> Ping(IPAddress address)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(Data);
            return await _pinger.SendPingAsync(address, Timeout, buffer, _options).ContinueWith(replay => replay.Result.Status);
        }

    }

}