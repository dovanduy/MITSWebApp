using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Models;

namespace MITSBusinessLib.Business
{
    public class WildApricotBusinessLogic : IWildApricotBusinessLogic
    {
        private readonly IWaRepository _waRepo;

        //Method for getting Wild Apricot event and associated types
        public WildApricotBusinessLogic(IWaRepository waRepo)
        {
            _waRepo = waRepo;
        }

       

    }
}
