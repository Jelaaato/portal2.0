using Portal.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Portal.Models.ViewModels;
using System.Web.Mvc;

namespace Portal.Models.BusinessLogic
{
    public class PaymentRemittance
    {
        private payment_remittance_entities payment_remittance = new payment_remittance_entities();

        private DateTime GetLatestPeriodDate(string employee_nr)
        {
            DateTime latestPeriodDate;
            latestPeriodDate = payment_remittance.payment_remittance.Where(a => a.employee_nr == employee_nr).OrderByDescending(a => a.period_date).Select(a => a.period_date).FirstOrDefault();

            if (latestPeriodDate != null)
            {
                return latestPeriodDate;
            }
            else
            {
                latestPeriodDate = payment_remittance.payment_period.OrderByDescending(a => a.period_date).Select(a => a.period_date).FirstOrDefault();
                return latestPeriodDate;
            }
        }

        public IEnumerable<payment_remittance> GetPaymentRemittance(string employee_nr, int period_id)
        {
            //DateTime period_date = this.GetLatestPeriodDate(employee_nr);

            var payment_remittances = payment_remittance.payment_remittance.Where(a => a.employee_nr == employee_nr && a.period_id == period_id).OrderByDescending(a => a.period_date).ToList();                                      

            return payment_remittances;
        }

        public IEnumerable<ReportsModel.PaymentRemittanceHeader> GetPaymentRemittanceHeaderByDate(string employee_nr, DateTime? datefrom, DateTime? dateto)
        {
            var payment_remittance_bydate = (from p in payment_remittance.payment_remittance
                                              where (p.employee_nr == employee_nr) &&
                                              (p.period_date >= datefrom && p.period_date <= dateto)
                                              select new ReportsModel.PaymentRemittanceHeader
                                              {
                                                  period_date = p.period_date,
                                                  processed_date = p.processed_datetime,
                                                  period_id = p.period_id,
                                                  dname = p.dname,
                                                  tax_rate = p.tax_rate
                                              }).GroupBy(a => a.period_date).Select(b => b.FirstOrDefault()).ToList();

            return payment_remittance_bydate;
        }

        public IEnumerable<ReportsModel.PaymentRemittanceHeader> GetLatestPaymentRemittanceHeader(string employee_nr)
        {
            DateTime pdate = this.GetLatestPeriodDate(employee_nr);

            var latest_payment_remittances = (from p in payment_remittance.payment_remittance
                                              where p.employee_nr == employee_nr &&
                                              p.period_date == pdate
                                              select new ReportsModel.PaymentRemittanceHeader
                                              {
                                                  period_date = p.period_date,
                                                  processed_date = p.processed_datetime,
                                                  period_id = p.period_id,
                                                  dname = p.dname,
                                                  tax_rate = p.tax_rate
                                              }).ToList();

            return latest_payment_remittances;
        }

        public int GetRemittanceCount(string employee_nr, DateTime period_date)
        {
            var count = payment_remittance.payment_remittance.Where(a => a.employee_nr == employee_nr && a.period_date == period_date).Count();

            return count;
        }

        public decimal? GetTotalAmountofCharges(string employee_nr, int period_id)
        {
            var charges = (from p in payment_remittance.payment_remittance
                           where p.employee_nr == employee_nr && p.period_id == period_id
                           select p.charge_amount).Sum();

            var previous_credits = (from p in payment_remittance.payment_remittance
                                    where p.employee_nr == employee_nr && p.period_id == period_id
                                    select p.prev_paid_amount).Sum();

            var total_charges = charges - previous_credits;

            return total_charges;
        }

        public decimal GetTotalAmountofSCDdiscount(string employee_nr, int period_id)
        {
            var scd = (from p in payment_remittance.payment_remittance
                       where p.employee_nr == employee_nr && p.period_id == period_id
                       select p.split_discount_amount_scd).Sum();

            return scd;
        }

        public decimal GetTotalAmountofOthdiscount(string employee_nr, int period_id)
        {
            var oth = (from p in payment_remittance.payment_remittance
                       where p.employee_nr == employee_nr && p.period_id == period_id
                       select p.split_discount_amount_oth).Sum();

            return oth;
        }

        public decimal GetTotalAmountofAdjustment(string employee_nr, int period_id)
        {
            var adjustment = (from p in payment_remittance.payment_remittance
                       where p.employee_nr == employee_nr && p.period_id == period_id
                       select p.split_adjustment_amount).Sum();

            return adjustment;
        }

        public decimal GetTotalAmountofPF(string employee_nr, int period_id)
        {
            var pf = (from p in payment_remittance.payment_remittance
                       where p.employee_nr == employee_nr && p.period_id == period_id
                       select p.split_net_amount).Sum();

            return pf;
        }

        public decimal GetTotalAmountofVatAmount(string employee_nr, int period_id)
        {
            var vat_amount = (from p in payment_remittance.payment_remittance
                       where p.employee_nr == employee_nr && p.period_id == period_id
                       select p.split_vat_amount).Sum();

            return vat_amount;
        }

        public decimal GetTotalAmountofTaxBaseAmount(string employee_nr, int period_id)
        {
            var tax_base_amount = (from p in payment_remittance.payment_remittance
                       where p.employee_nr == employee_nr && p.period_id == period_id
                       select p.split_tax_base_amount).Sum();

            return tax_base_amount;
        }

        public decimal GetTotalAmountofTaxAmount(string employee_nr, int period_id)
        {
            var tax_amount = (from p in payment_remittance.payment_remittance
                       where p.employee_nr == employee_nr && p.period_id == period_id
                       select p.split_tax_amount).Sum();

            return tax_amount;
        }

        public decimal GetTotalAmountofMerchantDiscount(string employee_nr, int period_id)
        {
            var merchant_discount = (from p in payment_remittance.payment_remittance
                       where p.employee_nr == employee_nr && p.period_id == period_id
                       select p.split_merchant_discount).Sum();

            return merchant_discount;
        }

        public decimal GetTotalAmountofCreditedAmount(string employee_nr, int period_id)
        {
            var credited_amount = (from p in payment_remittance.payment_remittance
                       where p.employee_nr == employee_nr && p.period_id == period_id
                       select p.split_credited_amount).Sum();

            return credited_amount;
        }

        public SelectList PopulatePaymentPeriodDropdown()
        {
            var period = payment_remittance.payment_period.OrderByDescending(a => a.period_date).ToList();

            var period_list = new SelectList(period, "period_id", "period_date");

            return period_list;
        }
    }
}