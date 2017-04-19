﻿using Portal.Models.BusinessLogic;
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

namespace Portal.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports

        private OMCP omcp = new OMCP();

        private int pageSize;
        private int pageNumber;
        private string searchstring;
        private Guid patient_id;

        [Authorize]
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
                        ModelState.Clear();
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

                omcp.InitializePDF(doc, mst);

                Chunk hr = new Chunk(new LineSeparator());

                #region Fonts

                var defaultFont = FontFactory.GetFont("Arial", 9);
                var titleFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                var valueFont = FontFactory.GetFont("Arial", 11);

                #endregion

                #region tables

                //Patient Demographics Header
                PdfPTable tblHeader1 = omcp.CreateTable(4);
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
                    tblHeader1.AddCell(new Phrase("21" + " y.o", valueFont));
                    tblHeader1.AddCell(new Phrase("Gender:", valueFont));
                    tblHeader1.AddCell(new Phrase(item.gender, valueFont));
                    tblHeader1.AddCell(new Phrase(" "));
                    tblHeader1.AddCell(new Phrase(" "));
                }

                //Patient Allergies
                PdfPTable tblAllergies = omcp.CreateTable(3);

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
                PdfPTable tblDiagnosis = omcp.CreateTable(5);

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
                PdfPTable tblMedications = omcp.CreateTable(2);

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
                PdfPTable tblHospitalizations = omcp.CreateTable(3);

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
                PdfPTable tblSurgeries = omcp.CreateTable(1);

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

                doc.Add(omcp.ImageHeader());
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
    }
}