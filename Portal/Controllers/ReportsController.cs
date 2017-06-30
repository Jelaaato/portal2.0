using Portal.Models.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vereyon.Web;
using PagedList;
using PagedList.Mvc;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf;
using Portal.Models.ViewModels;
using Portal.Models.Reports;

namespace Portal.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports

        private OMCP omcp = new OMCP();
        private PaymentRemittance paymentRemittance = new PaymentRemittance();
        private CreatePDF createpdf = new CreatePDF();

        private int pageSize;
        private int pageNumber;
        private string searchstring;
        private Guid patient_id;

        [Authorize(Roles="Employee")]
        public ActionResult OMCP(string search)
        {
            try
            {
                if (search != null)
                {
                    if (omcp.isValidHN(search))
                    {
                        Session["searchstring"] = search;
                        Session["patient_id"] = omcp.GetPatientId(search);

                        var patient = omcp.GetPatient(search);

                        ViewBag.DisplayPartialViews = "true";
                        //ModelState.Clear();
                        return View(patient);
                    }
                    else
                    {
                        FlashMessage.Info("You have entered an invalid hospital number");
                        ModelState.Clear();
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Authorize(Roles = "Employee")]
        public ActionResult OMCPAllergies(int? page, string currentfilter)
        {
            try
            {
                searchstring = Session["searchstring"].ToString();
                patient_id = new Guid(Session["patient_id"].ToString());

                if (searchstring != null && page < 1)
                {
                    page = 1;
                }
                else
                {
                    searchstring = currentfilter;
                }

                ViewBag.CurrentFilter = searchstring;
                pageSize = 10;
                pageNumber = (page ?? 1);

                var allergies = omcp.GetAllergies(patient_id).ToPagedList(pageNumber, pageSize);
                return PartialView("_allergies", allergies);
            }
            catch (Exception)
            {
                return PartialView("_allergies");
            }
        }

        [Authorize(Roles = "Employee")]
        public ActionResult OMCPDiagnosis(int? page, string currentfilter)
        {
            try
            {
                searchstring = Session["searchstring"].ToString();
                patient_id = new Guid(Session["patient_id"].ToString());

                if (searchstring != null && page < 1)
                {
                    page = 1;
                }
                else
                {
                    searchstring = currentfilter;
                }

                ViewBag.CurrentFilter = searchstring;
                pageSize = 10;
                pageNumber = (page ?? 1);

                var diagnosis = omcp.GetDiagnosis(patient_id).ToPagedList(pageNumber, pageSize);
                return PartialView("_diagnosis", diagnosis);
            }
            catch (Exception)
            {
                return PartialView("_diagnosis");
            }
        }

        [Authorize(Roles = "Employee")]
        public ActionResult OMCPHospitalizations(int? page, string currentfilter)
        {
            try
            {
                searchstring = Session["searchstring"].ToString();
                patient_id = new Guid(Session["patient_id"].ToString());

                if (searchstring != null && page < 1)
                {
                    page = 1;
                }
                else
                {
                    searchstring = currentfilter;
                }

                ViewBag.CurrentFilter = searchstring;
                pageSize = 10;
                pageNumber = (page ?? 1);

                var hospitalizations = omcp.GetHospitalizations(patient_id).ToPagedList(pageNumber, pageSize);
                return PartialView("_hospitalizations", hospitalizations);
            }
            catch (Exception)
            {
                return PartialView("_hospitalizations");
            }
        }

        [Authorize(Roles = "Employee")]
        public ActionResult OMCPMedications(int? page, string currentfilter)
        {
            try
            {
                searchstring = Session["searchstring"].ToString();
                patient_id = new Guid(Session["patient_id"].ToString());

                if (searchstring != null && page < 1)
                {
                    page = 1;
                }
                else
                {
                    searchstring = currentfilter;
                }

                ViewBag.CurrentFilter = searchstring;
                pageSize = 10;
                pageNumber = (page ?? 1);

                var medications = omcp.GetMedications(patient_id).ToPagedList(pageNumber, pageSize);
                return PartialView("_medications", medications);
            }
            catch (Exception)
            {
                return PartialView("_medications");
            }
        }

        [Authorize(Roles = "Employee")]
        public ActionResult OMCPSurgeries(int? page, string currentfilter)
        {
            try
            {
                searchstring = Session["searchstring"].ToString();
                patient_id = new Guid(Session["patient_id"].ToString());

                if (searchstring != null && page < 1)
                {
                    page = 1;
                }
                else
                {
                    searchstring = currentfilter;
                }

                ViewBag.CurrentFilter = searchstring;
                pageSize = 10;
                pageNumber = (page ?? 1);

                var surgeries = omcp.GetSurgeries(patient_id).ToPagedList(pageNumber, pageSize);
                return PartialView("_surgeries", surgeries);
            }
            catch (Exception)
            {
                return PartialView("_surgeries");
            }
        }

        [Authorize(Roles = "Employee")]
        public ActionResult GenerateOMCPReport()
        {
            if (Session["searchstring"] != null || Session["patient_id"] != null)
            {
                searchstring = Session["searchstring"].ToString();
                patient_id = new Guid(Session["patient_id"].ToString());

                var patient = omcp.GetPatient(searchstring);
                var allergies = omcp.GetAllergies(patient_id);
                var diagnosis = omcp.GetDiagnosis(patient_id);
                var medications = omcp.GetMedications(patient_id);
                var previous_surgeries = omcp.GetSurgeries(patient_id);
                var previous_hospitalizations = omcp.GetHospitalizations(patient_id);

                var fileName = "Outpatient Medical Care Profile - " + searchstring;

                Document doc = new Document();
                MemoryStream mst = new MemoryStream();

                createpdf.InitializePDF(doc, mst, PageSize.A4);

                Chunk hr = new Chunk(new LineSeparator());

                #region Fonts

                var defaultFont = FontFactory.GetFont("Arial", 9);
                var titleFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                var valueFont = FontFactory.GetFont("Arial", 11);

                #endregion

                #region tables

                //Patient Demographics Header
                PdfPTable tblHeader1 = createpdf.CreateTable(4);
                tblHeader1.DefaultCell.Border = Rectangle.NO_BORDER;
                tblHeader1.DefaultCell.Padding = 2;

                foreach (var item in patient)
                {
                    tblHeader1.AddCell(new Phrase("Name:", valueFont));
                    tblHeader1.AddCell(new Phrase(item.display_name, valueFont));
                    tblHeader1.AddCell(new Phrase("HN:", valueFont));
                    tblHeader1.AddCell(new Phrase(item.hospital_number, valueFont));
                    tblHeader1.AddCell(new Phrase("Date of Birth:", valueFont));
                    tblHeader1.AddCell(new Phrase(item.date_of_birth.ToString("yyyy-MM-dd"), valueFont));
                    tblHeader1.AddCell(new Phrase("Age:", valueFont));
                    tblHeader1.AddCell(new Phrase(item.age + " y.o", valueFont));
                    tblHeader1.AddCell(new Phrase("Gender:", valueFont));
                    tblHeader1.AddCell(new Phrase(item.gender, valueFont));
                    tblHeader1.AddCell(new Phrase(" "));
                    tblHeader1.AddCell(new Phrase(" "));
                }

                //Patient Allergies
                PdfPTable tblAllergies = createpdf.CreateTable(3);

                if (allergies.Count() != 0)
                {
                    tblAllergies.AddCell(new Phrase("Allergen", titleFont));
                    tblAllergies.AddCell(new Phrase("Allergy Status", titleFont));
                    tblAllergies.AddCell(new Phrase("Adverse Reaction", titleFont));

                    foreach (var item in allergies)
                    {
                        tblAllergies.AddCell(new Phrase(item.cause, defaultFont));
                        tblAllergies.AddCell(new Phrase(item.reaction_cause_status, defaultFont));
                        tblAllergies.AddCell(new Phrase(item.reaction, defaultFont));
                    }
                }
                else
                {
                    tblAllergies.ResetColumnCount(1);
                    tblAllergies.DefaultCell.Border = Rectangle.NO_BORDER;
                    tblAllergies.AddCell(new Phrase("No records available", defaultFont));
                }
                

                //Patient Diagnosis
                PdfPTable tblDiagnosis = createpdf.CreateTable(5);

                if (diagnosis.Count() != 0)
                {
                    tblDiagnosis.AddCell(new Phrase("Date Recorded", titleFont));
                    tblDiagnosis.AddCell(new Phrase("Diagnosis", titleFont));
                    tblDiagnosis.AddCell(new Phrase("Code", titleFont));
                    tblDiagnosis.AddCell(new Phrase("Coding System", titleFont));
                    tblDiagnosis.AddCell(new Phrase("Coding Type", titleFont));

                    foreach (var item in diagnosis)
                    {
                        tblDiagnosis.AddCell(new Phrase(item.recorded_date_time.ToString(), defaultFont));
                        tblDiagnosis.AddCell(new Phrase(item.diagnosis, defaultFont));
                        tblDiagnosis.AddCell(new Phrase(item.code, defaultFont));
                        tblDiagnosis.AddCell(new Phrase(item.coding_system, defaultFont));
                        tblDiagnosis.AddCell(new Phrase(item.coding_type, defaultFont));
                    }
                }
                else
                {
                    tblDiagnosis.ResetColumnCount(1);
                    tblDiagnosis.DefaultCell.Border = Rectangle.NO_BORDER;
                    tblDiagnosis.AddCell(new Phrase("No records available", defaultFont));
                }
                

                //Patient Medications
                PdfPTable tblMedications = createpdf.CreateTable(2);

                if (medications.Count() != 0)
                {
                    tblMedications.AddCell(new Phrase("Date", titleFont));
                    tblMedications.AddCell(new Phrase("Details", titleFont));

                    foreach (var item in medications)
                    {
                        tblMedications.AddCell(new Phrase(item.note_date.ToString(), defaultFont));
                        tblMedications.AddCell(new Phrase(item.details, defaultFont));
                    }
                }
                else
                {
                    tblMedications.ResetColumnCount(1);
                    tblMedications.DefaultCell.Border = Rectangle.NO_BORDER;
                    tblMedications.AddCell(new Phrase("No records available", defaultFont));
                }

                //Patient Hospitalizations
                PdfPTable tblHospitalizations = createpdf.CreateTable(3);

                if (previous_hospitalizations.Count() != 0)
                {
                    tblHospitalizations.AddCell(new Phrase("Visit Date", titleFont));
                    tblHospitalizations.AddCell(new Phrase("Visit Type", titleFont));
                    tblHospitalizations.AddCell(new Phrase("Primary Service", titleFont));

                    foreach (var item in previous_hospitalizations)
                    {
                        tblHospitalizations.AddCell(new Phrase(item.actual_visit_date_time.ToString(), defaultFont));
                        tblHospitalizations.AddCell(new Phrase(item.visit_type, defaultFont));
                        tblHospitalizations.AddCell(new Phrase(item.primary_service, defaultFont));
                    }
                }
                else
                {
                    tblHospitalizations.ResetColumnCount(1);
                    tblHospitalizations.DefaultCell.Border = Rectangle.NO_BORDER;
                    tblHospitalizations.AddCell(new Phrase("No records available", defaultFont));
                }

                //Patient Surgeries
                PdfPTable tblSurgeries = createpdf.CreateTable(1);

                if (previous_surgeries.Count() != 0)
                {
                    tblSurgeries.AddCell(new Phrase("Procedure", titleFont));

                    foreach (var item in previous_surgeries)
                    {
                        tblSurgeries.AddCell(new Phrase(item.previous_surgeries, defaultFont));
                    }
                }
                else
                {
                    tblSurgeries.ResetColumnCount(1);
                    tblSurgeries.DefaultCell.Border = Rectangle.NO_BORDER;
                    tblSurgeries.AddCell(new Phrase("No records available", defaultFont));
                }
                

                #endregion

                #region Add Tables to Document

                doc.Add(createpdf.ImageHeader());
                doc.Add(new Paragraph("\n"));
                doc.Add(tblHeader1);
                doc.Add(hr);

                doc.Add(new Paragraph("\n"));
                doc.Add(new Phrase("OUTPATIENT MEDICAL CARE PROFILE", valueFont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Phrase("Allergies", titleFont));
                doc.Add(new Paragraph("\n"));
                doc.Add(tblAllergies);
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Phrase("Diagnosis", titleFont));
                doc.Add(new Paragraph("\n"));
                doc.Add(tblDiagnosis);
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Phrase("Medications", titleFont));
                doc.Add(new Paragraph("\n"));
                doc.Add(tblMedications);
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Phrase("Previous Hospitalizations", titleFont));
                doc.Add(new Paragraph("\n"));
                doc.Add(tblHospitalizations);
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Phrase("Previous Surgeries", titleFont));
                doc.Add(new Paragraph("\n"));
                doc.Add(tblSurgeries);
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("\n"));

                #endregion

                doc.Add(hr);
                doc.Add(new Paragraph("\n"));
                doc.Add(new Phrase(DateTime.Now.ToString(), defaultFont));
                doc.Close();

                mst.Flush();
                mst.Position = 0;

                Response.AddHeader("content-disposition", string.Format("inline; filename={0}", fileName));
                return File(mst, "application/pdf");
            }
            else 
            {
                FlashMessage.Info("There is no report to be generated, please search for a hospital number first.");
                return RedirectToAction("OMCP");
            }
        }

        [Authorize(Roles = "Doctor")]
        public ActionResult PaymentRemittance(ReportsModel.PaymentRemittanceModel model)
        {
                model.payment_remittance_header = paymentRemittance.GetLatestPaymentRemittanceHeader(HttpContext.User.Identity.Name).Take(1);
                model.payment_period = paymentRemittance.PopulatePaymentPeriodDropdown();
                return View(model);
        }

        [HttpPost]
        public ActionResult PaymentRemittance(int? page, ReportsModel.PaymentRemittanceModel model)
        {
            model.start_date = new DateTime(model.period_date.Value.Year, model.period_date.Value.Month, 1);
            model.end_date = model.start_date.Value.AddMonths(1).AddDays(-1);

            model.payment_remittance_header = paymentRemittance.GetPaymentRemittanceHeaderByDate(HttpContext.User.Identity.Name, model.start_date, model.end_date).ToList();

            return PartialView("_PaymentRemittance", model);
        }

        [Authorize(Roles = "Doctor")]
        public ActionResult GeneratePaymentRemittanceReport(int id, DateTime payout_date, string name, decimal tax_rate)
        {
            IEnumerable<payment_remittance> payment_remittance = paymentRemittance.GetPaymentRemittance(HttpContext.User.Identity.Name, id);

            Document doc = new Document();
            MemoryStream mst = new MemoryStream();

            createpdf.InitializePDF(doc, mst, PageSize.A4.Rotate());

            Chunk hr = new Chunk(new LineSeparator());

            int[] headerwidth = { 50, 20 };
            int[] contentwidth = { 18, 16, 9, 8, 8, 8, 9, 8, 8, 8, 5, 8, 8, 8 };

            #region Fonts

            var headerFont = FontFactory.GetFont("Arial", 9);
            var contentFont = FontFactory.GetFont("Arial", 7);
            var defaultFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
            var titleFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            var valueFont = FontFactory.GetFont("Arial", 11);
            var amountFont = FontFactory.GetFont("Arial", 8, Font.BOLD);

            #endregion

            #region tables

            PdfPTable headerTbl = createpdf.CreateTable(2);
            headerTbl.DefaultCell.Border = Rectangle.NO_BORDER;
            headerTbl.SetWidths(headerwidth);
            headerTbl.DefaultCell.Padding = 5;

            headerTbl.AddCell(new Phrase("Employee ID : " + HttpContext.User.Identity.Name, headerFont));
            headerTbl.AddCell(new Phrase("Payout / Withholding Date: " + payout_date.ToShortDateString(), headerFont));
            headerTbl.AddCell(new Phrase(name, titleFont));
            headerTbl.AddCell(new Phrase(""));
            headerTbl.AddCell(new Phrase("TIN: " + tax_rate.ToString("N") + " / " + ((tax_rate > 1) ? "VAT" : "NON-VAT"), headerFont));
            headerTbl.AddCell(new Phrase(""));

            PdfPTable mainTbl = createpdf.CreateTable(14);
            mainTbl.DefaultCell.Border = Rectangle.NO_BORDER;
            mainTbl.DefaultCell.Padding = 2;
            mainTbl.SetWidths(contentwidth);

            mainTbl.AddCell(new Phrase("Patient Name", defaultFont));
            mainTbl.AddCell(new Phrase("Description", defaultFont));
            mainTbl.AddCell(new Phrase("Date Charged", defaultFont));
            mainTbl.AddCell(new Phrase("Amounts", defaultFont));
            mainTbl.AddCell(new Phrase("SCD   Discounts", defaultFont));
            mainTbl.AddCell(new Phrase("Other Discounts", defaultFont));
            mainTbl.AddCell(new Phrase("Adjustment", defaultFont));
            mainTbl.AddCell(new Phrase("PF Bill", defaultFont));
            mainTbl.AddCell(new Phrase("Amount  of VAT", defaultFont));
            mainTbl.AddCell(new Phrase("Taxable Amount", defaultFont));
            mainTbl.AddCell(new Phrase("WH      Tax Rate", defaultFont));
            mainTbl.AddCell(new Phrase("WH       Amount", defaultFont));
            mainTbl.AddCell(new Phrase("Merchant Discount", defaultFont));
            mainTbl.AddCell(new Phrase("Credited Amount", defaultFont));

            PdfPTable contentTbl = createpdf.CreateTable(14);
            contentTbl.DefaultCell.Border = Rectangle.NO_BORDER;
            contentTbl.DefaultCell.Padding = 3;
            contentTbl.SetWidths(contentwidth);

            foreach (var payment in payment_remittance)
            {
                contentTbl.AddCell(new Phrase(payment.pname, createpdf.SetFont("Arial", 7, Font.BOLD)));
                contentTbl.AddCell(new Phrase(payment.item_desc, createpdf.SetFont("Arial", 7, Font.BOLD)));
                contentTbl.AddCell(new Phrase((payment.charge_date == null) ? "" : payment.charge_date.Value.ToShortDateString(), contentFont));
                contentTbl.AddCell(new Phrase(payment.charge_amount.Value.ToString("N"), createpdf.SetFont("Arial", 7, Font.BOLD)));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("Previous Credits", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase(payment.prev_paid_amount.Value.ToString("N"), contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("       " + payment.policy_group, createpdf.SetFont("Arial", 7, Font.BOLD)));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase(payment.gross_amount.ToString("N"), contentFont));
                contentTbl.AddCell(new Phrase(payment.split_discount_amount_scd.ToString("N"), contentFont));
                contentTbl.AddCell(new Phrase(payment.split_discount_amount_oth.ToString("N"), contentFont));
                contentTbl.AddCell(new Phrase(payment.split_adjustment_amount.ToString("N"), contentFont));
                contentTbl.AddCell(new Phrase(payment.split_net_amount.ToString("N"), contentFont));
                contentTbl.AddCell(new Phrase(payment.split_vat_amount.ToString("N"), contentFont));
                contentTbl.AddCell(new Phrase(payment.split_tax_base_amount.ToString("N"), contentFont));
                contentTbl.AddCell(new Phrase((int)(payment.split_tax_rate * 100) + "%", contentFont));
                contentTbl.AddCell(new Phrase(payment.split_tax_amount.ToString("N"), contentFont));
                contentTbl.AddCell(new Phrase(payment.split_merchant_discount.ToString("N"), contentFont));
                contentTbl.AddCell(new Phrase(payment.split_credited_amount.ToString("N"), contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("Remaining Balance", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont)); //Remaining Balance 0.00
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
                contentTbl.AddCell(new Phrase("", contentFont));
            }

            

            PdfPTable totalTbl = createpdf.CreateTable(14);
            totalTbl.DefaultCell.Border = Rectangle.NO_BORDER;
            totalTbl.DefaultCell.Padding = 2;
            totalTbl.SetWidths(contentwidth);

            totalTbl.AddCell(new Phrase("GRAND TOTAL: ", defaultFont));
            totalTbl.AddCell(new Phrase("Count: " + paymentRemittance.GetRemittanceCount(HttpContext.User.Identity.Name, payout_date), defaultFont));
            totalTbl.AddCell(new Phrase("", defaultFont));
            totalTbl.AddCell(new Phrase(paymentRemittance.GetTotalAmountofCharges(HttpContext.User.Identity.Name, id).Value.ToString("N"), amountFont));
            totalTbl.AddCell(new Phrase(paymentRemittance.GetTotalAmountofSCDdiscount(HttpContext.User.Identity.Name, id).ToString("N"), amountFont));
            totalTbl.AddCell(new Phrase(paymentRemittance.GetTotalAmountofOthdiscount(HttpContext.User.Identity.Name, id).ToString("N"), amountFont));
            totalTbl.AddCell(new Phrase(paymentRemittance.GetTotalAmountofAdjustment(HttpContext.User.Identity.Name, id).ToString("N"), amountFont));
            totalTbl.AddCell(new Phrase(paymentRemittance.GetTotalAmountofPF(HttpContext.User.Identity.Name, id).ToString("N"), amountFont));
            totalTbl.AddCell(new Phrase(paymentRemittance.GetTotalAmountofVatAmount(HttpContext.User.Identity.Name, id).ToString("N"), amountFont));
            totalTbl.AddCell(new Phrase(paymentRemittance.GetTotalAmountofTaxBaseAmount(HttpContext.User.Identity.Name, id).ToString("N"), amountFont));
            totalTbl.AddCell(new Phrase("", defaultFont));
            totalTbl.AddCell(new Phrase(paymentRemittance.GetTotalAmountofTaxAmount(HttpContext.User.Identity.Name, id).ToString("N"), amountFont));
            totalTbl.AddCell(new Phrase(paymentRemittance.GetTotalAmountofMerchantDiscount(HttpContext.User.Identity.Name, id).ToString("N"), amountFont));
            totalTbl.AddCell(new Phrase(paymentRemittance.GetTotalAmountofCreditedAmount(HttpContext.User.Identity.Name, id).ToString("N"), amountFont));

            #endregion

            doc.Add(createpdf.ImageHeader());
            doc.Add(new Paragraph("\n"));
            doc.Add(new Phrase("PAYMENT REMITTANCE ADVICE", valueFont));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));
            doc.Add(headerTbl);
            doc.Add(new Paragraph("\n"));
            doc.Add(mainTbl);
            doc.Add(new Paragraph("\n"));
            doc.Add(contentTbl);
            doc.Add(hr);
            doc.Add(new Paragraph("\n"));
            doc.Add(totalTbl);
            doc.Add(new Paragraph("\n"));

            doc.Close();

            mst.Flush();
            mst.Position = 0;

            Response.AddHeader("content-disposition", string.Format("inline; filename={0}", "Payment Remittance Advice"));
            return File(mst, "application/pdf");
        }
    }
}