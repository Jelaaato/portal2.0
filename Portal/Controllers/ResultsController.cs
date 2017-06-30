using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Portal.Models.BusinessLogic;
using Portal.Models.Helpers;
using Portal.Models.Results;
using Portal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace Portal.Controllers
{
    public class ResultsController : Controller
    {
        // GET: Results
        private Laboratory lab = new Laboratory();
        private Files files = new Files();
        private CreatePDF createpdf = new CreatePDF();
        private Results results = new Results();

        [Authorize(Roles = "Patient, Doctor")]
        [Audit]
        public ActionResult LaboratoryResults(LaboratoryModel model, string currentfilter, string search, string lab_order_name, DateTime? minDate, int? page)
        {
            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentfilter;
            }

            ViewBag.CurrentFilter = search;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (lab_order_name == "All Laboratory Results" || string.IsNullOrEmpty(lab_order_name))
            {
                if (User.IsInRole("Patient"))
                {
                    if (!String.IsNullOrEmpty(search))
                    {
                        model.results_references = lab.PopulateResultsDropdown();
                        model.patient_lab_header = lab.GetAllPatientHeader(HttpContext.User.Identity.Name).Where(a => a.patient_name.IndexOf(search, StringComparison.OrdinalIgnoreCase) != -1).OrderByDescending(a => a.order_date_time).ToPagedList(pageNumber, pageSize);
                    }
                    else
                    {
                        model.results_references = lab.PopulateResultsDropdown();
                        model.patient_lab_header = lab.GetAllPatientHeader(HttpContext.User.Identity.Name).ToPagedList(pageNumber, pageSize);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(search))
                    {
                        model.results_references = lab.PopulateResultsDropdown();
                        model.patient_lab_header = lab.GetAllPatientHeaderForDoctor(HttpContext.User.Identity.Name).Where(a => a.patient_name.IndexOf(search, StringComparison.OrdinalIgnoreCase) != -1).OrderByDescending(a => a.order_date_time).ToPagedList(pageNumber, pageSize);
                    }
                    else
                    {
                        model.results_references = lab.PopulateResultsDropdown();
                        model.patient_lab_header = lab.GetAllPatientHeaderForDoctor(HttpContext.User.Identity.Name).ToPagedList(pageNumber, pageSize);
                    }
                }
            }
            else
            {
                ViewBag.CurrentLabOrder = lab_order_name;
                ModelState.Clear();
                if (User.IsInRole("Patient"))
                {
                    if (!String.IsNullOrEmpty(search))
                    {
                        model.results_references = lab.PopulateResultsDropdown();
                        model.patient_lab_header = lab.GetPatientHeader(HttpContext.User.Identity.Name, lab_order_name).Where(a => a.patient_name.Contains(search)).OrderByDescending(a => a.order_date_time).ToPagedList(pageNumber, pageSize);
                    }
                    else
                    {
                        model.results_references = lab.PopulateResultsDropdown();
                        model.patient_lab_header = lab.GetPatientHeader(HttpContext.User.Identity.Name, lab_order_name).ToPagedList(pageNumber, pageSize);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(search))
                    {
                        model.results_references = lab.PopulateResultsDropdown();
                        model.patient_lab_header = lab.GetPatientHeaderForDoctor(HttpContext.User.Identity.Name, lab_order_name).Where(a => a.patient_name.IndexOf(search, StringComparison.OrdinalIgnoreCase) != -1).OrderByDescending(a => a.order_date_time).ToPagedList(pageNumber, pageSize);
                    }
                    else
                    {
                        model.results_references = lab.PopulateResultsDropdown();
                        model.patient_lab_header = lab.GetPatientHeaderForDoctor(HttpContext.User.Identity.Name, lab_order_name).ToPagedList(pageNumber, pageSize);
                    }
                }
            }
            return View(model);
        }

        public ActionResult Radiology()
        {
            return View();
        }

        public ActionResult Diagnosis()
        {
            return View();
        }

        public ActionResult ViewResult(Guid lab_work_order_id, string service_category, string lab_order_name)
        {
            IEnumerable<patient_lab_result_header> patientHeader;
            IEnumerable<patient_lab_results> patientLabResult;

            if (User.IsInRole("Patient"))
            {
                patientHeader = results.GetPatientHeader(HttpContext.User.Identity.Name, lab_work_order_id);
                patientLabResult = results.GetPatientLabResult(HttpContext.User.Identity.Name, lab_work_order_id);
            }
            else
            {
                patientHeader = results.GetPatientHeaderForDoctor(HttpContext.User.Identity.Name, lab_work_order_id);
                patientLabResult = results.GetPatientLabResultForDoctor(lab_work_order_id);
            }

            var fileName = "Laboratory Result";

            Document doc = new Document();
            MemoryStream mst = new MemoryStream();

            createpdf.InitializePDF(doc, mst, PageSize.A4);

            Chunk hr = new Chunk(new LineSeparator());

            #region Fonts and Widths

            var defaultFont = FontFactory.GetFont("Arial", 9);
            var titleFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var valueFont = FontFactory.GetFont("Arial", 11);

            int[] headertblwidth = { 20, 30 };
            int[] labresultswidth = { 15, 10, 10, 10 };
            int[] labfooterwidth = { 50, 20 };

            #endregion

            #region tables

            //Patient Demographics Header
            PdfPTable tblHeader1 = createpdf.CreateTable(4);
            tblHeader1.DefaultCell.Border = Rectangle.NO_BORDER;
            tblHeader1.DefaultCell.Padding = 2;

            foreach (var item in patientHeader)
            {
                tblHeader1.AddCell(new Phrase("Name:", valueFont));
                tblHeader1.AddCell(new Phrase(item.patient_name, valueFont));
                tblHeader1.AddCell(new Phrase("HN:", valueFont));
                tblHeader1.AddCell(new Phrase(item.hospital_number, valueFont));
                tblHeader1.AddCell(new Phrase("Date of Birth:", valueFont));
                tblHeader1.AddCell(new Phrase(item.date_of_birth.ToString("yyyy-MM-dd"), valueFont));
                tblHeader1.AddCell(new Phrase("Age:", valueFont));
                tblHeader1.AddCell(new Phrase(item.age + " y.o", valueFont));
                tblHeader1.AddCell(new Phrase("Gender:", valueFont));
                tblHeader1.AddCell(new Phrase(item.sex, valueFont));
                tblHeader1.AddCell(new Phrase("Room:", valueFont));
                tblHeader1.AddCell(new Phrase(item.patient_area, valueFont));
                tblHeader1.AddCell(new Phrase("Physician:", valueFont));
                tblHeader1.AddCell(new Phrase(item.requesting_physician, valueFont));
                tblHeader1.AddCell(new Phrase("Order Group:", valueFont));
                tblHeader1.AddCell(new Phrase(item.order_group, valueFont));
                tblHeader1.AddCell(new Phrase("Order Date:", valueFont));
                tblHeader1.AddCell(new Phrase(item.order_date_time.ToString(), valueFont));
                tblHeader1.AddCell(new Phrase("Reported Date", valueFont));
                tblHeader1.AddCell(new Phrase(item.reported_date_time.ToString(), valueFont));
            }

            //Patient Lab Results
            PdfPTable labHeaderTbl = createpdf.CreateTable(4);
            labHeaderTbl.DefaultCell.Border = Rectangle.NO_BORDER;
            labHeaderTbl.SetWidths(labresultswidth);
            labHeaderTbl.AddCell(new Phrase("Test", titleFont));
            labHeaderTbl.AddCell(new Phrase("Result", titleFont));
            labHeaderTbl.AddCell(new Phrase("Unit", titleFont));
            labHeaderTbl.AddCell(new Phrase("Reference Range", titleFont));

            PdfPTable labResultsTbl = new PdfPTable(4);
            labResultsTbl.WidthPercentage = 100;
            labResultsTbl.DefaultCell.Border = Rectangle.NO_BORDER;
            labResultsTbl.SetWidths(labresultswidth);


            foreach (var item in patientLabResult.Where(a => a.result != null))
            {
                labResultsTbl.AddCell(new Phrase(item.test, defaultFont));
                labResultsTbl.AddCell(new Phrase(item.result.ToString(), defaultFont));
                labResultsTbl.AddCell(new Phrase(item.UOM, defaultFont));
                labResultsTbl.AddCell(new Phrase(item.reference_range, defaultFont));
            }

            PdfPTable labFooter = createpdf.CreateTable(2);
            labFooter.DefaultCell.Border = Rectangle.NO_BORDER;
            labFooter.SetWidths(labfooterwidth);
            labFooter.AddCell(new Phrase(patientHeader.Select(a => a.med_tech).First(), defaultFont));
            labFooter.AddCell(new Phrase(patientHeader.Select(a => a.chief_pathologist).First(), defaultFont));
            labFooter.AddCell(new Phrase("Medical Technologist", titleFont));
            labFooter.AddCell(new Phrase("Chief Pathologist", titleFont));

            #endregion

            #region Add Tables to Document

            doc.Add(createpdf.ImageHeader());
            doc.Add(new Paragraph("\n"));
            doc.Add(tblHeader1);
            doc.Add(hr);

            doc.Add(new Paragraph("\n"));
            doc.Add(new Phrase(patientHeader.Select(a => a.service_category).First() +" " + "|" + " " + patientHeader.Select(a => a.lab_orderable_name).First(), titleFont));

            doc.Add(new Paragraph("\n"));
            doc.Add(labHeaderTbl);
            doc.Add(hr);
            doc.Add(labResultsTbl);

            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));
            doc.Add(labFooter);
            #endregion 

            doc.Add(hr);
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Phrase("**This report has been electronically signed**", defaultFont));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Phrase(DateTime.Now.ToString(), defaultFont));
            doc.Close();

            mst.Flush();
            mst.Position = 0;

            Response.AddHeader("content-disposition", string.Format("inline; filename={0}", fileName));
            return File(mst, "application/pdf");
        }
    }
}