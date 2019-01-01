using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.ResponseModels.WildApricot
{
    public class NewInvoice
    {
        public decimal Value { get; set; }
        public List<Invoice> Invoices { get; set; }
        public InvoiceContact Contact { get; set; }
        public string Comment { get; set; }
        public string PaymentType { get; set; }
    }
}
