using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MITSDataLib.Contexts;
using MITSDataLib.Models;
using MITSDataLib.Repositories.Interfaces;

namespace MITSDataLib.Repositories
{
    public class WaRepository : IWaRepository
    {
        private readonly MITSContext _context;

        public WaRepository(MITSContext context)
        {
            _context = context;
        }

        public async Task<WildApricotToken> GetTokenAsync()
        {
            return await  _context.WaTokens.SingleOrDefaultAsync();
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
            else {
                token.AccessToken = respToken.AccessToken;
                token.TokenExpires = respToken.TokenExpires;
                await _context.SaveChangesAsync();
                return token;
            } 

        }




        //setToken

        //replaceToken

        //deleteToken
    }
}