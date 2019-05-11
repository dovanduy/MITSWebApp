using MITSDataLib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MITSBusinessLib.Business
{
    public interface IBadgePrintBusinessLogic
    {
        Task<bool> PrintBadges(List<PrintBadge> badges);
    }
}
