using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MITSBusinessLib.Repositories.Interfaces;
using MITSBusinessLib.Utilities;
using MITSDataLib.Contexts;
using MITSDataLib.Models;

namespace MITSBusinessLib.Repositories
{
    public class WaRepository : IWaRepository
    {
        private readonly MITSContext _context;
        private readonly string _apiKey;

        public WaRepository(MITSContext context, IConfiguration config)
        {
            _context = context;
            _apiKey = config["APIKEY"];
        }

        public async Task<WildApricotToken> GetTokenAsync()
        {
            var token = await  _context.WaTokens.SingleOrDefaultAsync();

            var tokenExpires = token.TokenExpires ?? DateTime.Now;


            if (token == null || DateTime.Compare(tokenExpires, DateTime.Now.AddMinutes(3)) > 0)
            {
                var tokenResponse = await WildApricotOps.GenerateNewAccessToken(_apiKey);

                var newToken = new WildApricotToken
                {
                    AccessToken = tokenResponse.access_token,
                    TokenExpires = DateTime.Now.AddSeconds(tokenResponse.expires_in)
                };

                return await SetTokenAsync(newToken);
            }

            return token;

        }

        public async Task<WildApricotToken> SetTokenAsync(WildApricotToken respToken)
        {
            var token = await GetTokenAsync();

            if (token == null)
            {
                _context.WaTokens.Add(respToken);
                await _context.SaveChangesAsync();
                return respToken;

            }

            token.AccessToken = respToken.AccessToken;
            token.TokenExpires = respToken.TokenExpires;
            await _context.SaveChangesAsync();
            return token;

        }


        //setToken

        //replaceToken

        //deleteToken
    }
}