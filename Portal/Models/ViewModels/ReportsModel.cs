using Portal.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Models.ViewModels
{
    public class ReportsModel
    {
        public class PaymentRemittanceModel
        {
            public SelectList payment_period { get; set; }
            public DateTime? period_date { get; set; }

            public DateTime? start_date { get; set; }
            public DateTime? end_date { get; set; }

            public IEnumerable<payment_remittance> payment_remittance { get; set; }
            public IEnumerable<PaymentRemittanceHeader> payment_remittance_header { get; set; }
        }

        public class PaymentRemittanceHeader
        {
            public DateTime period_date { get; set; }
            public DateTime processed_date { get; set; }
            public int period_id { get; set; }
            public string dname { get; set; }
            public decimal tax_rate { get; set; }
        }
    }
}