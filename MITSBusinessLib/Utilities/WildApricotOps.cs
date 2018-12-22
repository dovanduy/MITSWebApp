using Microsoft.Extensions.Configuration;
using MITSBusinessLib.Models;
using MITSDataLib.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MITSBusinessLib.Utilities
{
    public static class WildApricotOps
    {
        private static readonly string WildApricotApiUrl = "https://api.wildapricot.org/v2/";
        private static readonly string WildApricotTokenUrl = "https://oauth.wildapricot.org/auth/token";

  
        

        public static async Task<TokenResponse> GenerateNewAccessToken(string apiKey) {
            var client = new HttpClient();
            var authAddr = new Uri(WildApricotTokenUrl);
            byte[] apiKeyBytes = System.Text.Encoding.UTF8.GetBytes(apiKey);
            var encodedApiKey = Convert.ToBase64String(apiKeyBytes);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Basic" + encodedApiKey);
            
       
            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", "auto")
            });

            var response = await client.PostAsync(authAddr, content);

            if (!response.IsSuccessStatusCode) {
                
                throw new HttpRequestException(response.ReasonPhrase);
               
            }

            var respContent = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(respContent);

            

        }

        public static async Task<HttpResponseMessage> GetResponse(int facultyId, string apiResource, WildApricotToken token)
        {
         
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.AccessToken);

            var apiAddr = new Uri(WildApricotApiUrl + apiResource, UriKind.Absolute);

            try
            {
                
                return await client.GetAsync(apiAddr);

            }

            catch (Exception e)
            {
                var message = e.Message + " " + e.InnerException;
                throw new Exception(message);
            }

        }



    }
}