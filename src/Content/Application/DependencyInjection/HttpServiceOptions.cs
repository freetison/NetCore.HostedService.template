using System.Collections.Generic;
using RestSharp.Authenticators;

namespace nchsapp.DependencyInjection
{
    public class HttpServiceOptions
    {
        public string BaseUrl { get; set; }

        public IAuthenticator Authenticator { get; set; }

        public IEnumerable<string> ContentTypes { get; set; } = new List<string>()
        {
            "application/json",
            "text/json",
            "text/x-json"
        };

    }
}
