using MITSDataLib.Contexts;
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

        //getToken

        //setToken

        //replaceToken

        //deleteToken
    }
}