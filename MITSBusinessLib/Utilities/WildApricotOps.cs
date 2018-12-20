using Microsoft.Extensions.Configuration;

namespace MITSBusinessLib.Utilities
{
    public class WildApricotOps : IWildApricotOps
    {
        private static readonly string WildApricotApiUrl = "https://api.wildapricot.org/v2/";
        private static readonly string WildApricotTokenUrl = "https://oauth.wildapricot.org/auth/token";

        private readonly string _apiKey;


        public WildApricotOps(IConfiguration config)
        {
            _apiKey = config["APIKEY"];
            //GetAccessToken

            //IsTokenExpired
        }

    }
}