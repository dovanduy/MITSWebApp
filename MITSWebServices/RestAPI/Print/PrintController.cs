using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MITSBusinessLib.Business;
using MITSDataLib.Models;

namespace MITSWebServices.RestAPI.Print
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintController : ControllerBase
    {
        private IBadgePrintBusinessLogic _printLogic;

        public PrintController(IBadgePrintBusinessLogic printLogic)
        {
            _printLogic = printLogic;
        }

        [HttpPost]
        public async Task<IActionResult> BadgesAsync([FromBody] List<PrintBadge> printBadges) {

            var result = await _printLogic.PrintBadges(printBadges);

            return Ok();
        }
    }
}