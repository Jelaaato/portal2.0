//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.Models.Reports
{
    using System;
    using System.Collections.Generic;
    
    public partial class payment_remittance
    {
        public System.Guid payment_period_detail_id { get; set; }
        public System.DateTime processed_datetime { get; set; }
        public int period_id { get; set; }
        public System.Guid charge_id { get; set; }
        public string employee_nr { get; set; }
        public string dname { get; set; }
        public string department_name { get; set; }
        public System.DateTime period_date { get; set; }
        public decimal vat_rate { get; set; }
        public decimal tax_rate { get; set; }
        public Nullable<decimal> charge_amount { get; set; }
        public decimal gross_amount { get; set; }
        public Nullable<decimal> prev_paid_amount { get; set; }
        public decimal split_discount_amount_scd { get; set; }
        public decimal split_discount_amount_oth { get; set; }
        public decimal split_adjustment_amount { get; set; }
        public decimal split_net_amount { get; set; }
        public decimal split_vat_amount { get; set; }
        public decimal split_tax_base_amount { get; set; }
        public decimal split_tax_rate { get; set; }
        public decimal split_tax_amount { get; set; }
        public decimal split_merchant_discount { get; set; }
        public decimal split_credited_amount { get; set; }
        public string policy_group { get; set; }
        public string pname { get; set; }
        public Nullable<byte> scd_flag { get; set; }
        public string item_desc { get; set; }
        public Nullable<System.DateTime> charge_date { get; set; }
    }
}
