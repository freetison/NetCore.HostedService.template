namespace nchsapp.Services
{
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Threading.Tasks;

    public interface IGoogleService
    {
        IPAddress IPAddress {get; }
        Task<IPStatus> Ping();
        void Dispose();
    }
}