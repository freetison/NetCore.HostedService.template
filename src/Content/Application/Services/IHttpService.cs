using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace nchsapp.Services
{
    public interface IHttpService
    {
        Task<IPStatus> Ping(IPAddress address);
        Task<string> GetData(Uri uri);
    }
}