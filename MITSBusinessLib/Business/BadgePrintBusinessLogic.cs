using eLime.GoogleCloudPrint;
using Microsoft.AspNetCore.Hosting;
using MITSDataLib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MITSBusinessLib.Business
{
    public class BadgePrintBusinessLogic : IBadgePrintBusinessLogic
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public BadgePrintBusinessLogic(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<bool> PrintBadges(List<PrintBadge> badges) {

            using (var printService = new GoogleCloudPrintService("Mits.badge.print")) 

            {
                Console.WriteLine(_hostingEnvironment.ContentRootPath);
                var p12Path = _hostingEnvironment.ContentRootPath + "\\Cloud Print-f133453408bb.p12";

                await printService.AuthorizeP12Async("root-828@cloud-print-240313.iam.gserviceaccount.com", p12Path, "notasecret");

                Console.WriteLine("Accepting invite...");

                var printerid = "95ffb34a-a2d0-d53c-f09f-f591206df145";

                var printer = await printService.GetPrinter(printerid, new List<string> { "connectionStatus", "queuedJobsCount" });



                var cjt = @"
                {
                    ""version"":""1.0"",
                    ""print"":{
                        ""color"":{ ""vendor_id"":""psk:Color"",""type"":""STANDARD_COLOR""},
                        ""duplex"":{ ""type"":""LONG_EDGE""},
                        ""vendor_ticket_item"":[
                            {""id"":""psk:PageInputBin"",""value"":""epns200:Front1""},
                        ]
                    }
                }";

                //print document. Set print in color, duplex printing and paper tray 2 as source

                var printjob = printService.PrintDocument(printerid, "example.pdf", cjt, "http://www.africau.edu/images/default/sample.pdf");
            }

            return true;
        }
    }
}
