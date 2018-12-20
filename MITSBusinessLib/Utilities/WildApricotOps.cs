using Microsoft.Extensions.Configuration;
using MITSBusinessLib.Models;
using MITSDataLib.Models;
using MITSDataLib.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MITSBusinessLib.Utilities
{
    public class WildApricotOps : IWildApricotOps
    {
        private static readonly string WildApricotApiUrl = "https://api.wildapricot.org/v2/";
        private static readonly string WildApricotTokenUrl = "https://oauth.wildapricot.org/auth/token";

        private readonly string _apiKey;
        public IWaRepository _waRepo { get; }


        public WildApricotOps(IConfiguration config, IWaRepository waRepo)
        {
            _apiKey = config["APIKEY"];
            _waRepo = waRepo;
            //GetAccessToken

            //IsTokenExpired
        }

        

        public async Task<string> GetAccessToken() {
            var token = await _waRepo.GetTokenAsync();

            if (token == null) {
                //generate and return new Access token
                
            }

            var tokenExpires = token.TokenExpires ?? DateTime.Now;

            if (DateTime.Compare(tokenExpires, DateTime.Now.AddMinutes(3)) > 0) {
                //generate and return new access token
            }

            return token.AccessToken;
        }

        public async Task<WildApricotToken> GenerateNewAccessToken() {
            var client = new HttpClient();
            var authAddr = new Uri(WildApricotTokenUrl);
            byte[] APIKeyBytes = System.Text.Encoding.UTF8.GetBytes($"API:{_apiKey}");
            var encodedAPIKey = Convert.ToBase64String(APIKeyBytes);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Basic" + encodedAPIKey);
            
       
            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", "auto")
            });

            var response = await client.PostAsync(authAddr, content);

            if (!response.IsSuccessStatusCode) {
                
                throw new HttpRequestException(response.ReasonPhrase);
               
            }

            var respContent = await response.Content.ReadAsStringAsync();
            var respToken = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(respContent);

            var newToken = new WildApricotToken
            {
                AccessToken = respToken.access_token,
                TokenExpires = DateTime.Now.AddSeconds(respToken.expires_in)
            };

            try
            {
                return await _waRepo.SetTokenAsync(newToken);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }



        }



    }
}