﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using Portal.Models.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Portal.Models.BusinessLogic
{
    public class OMCP
    {
        private reports_entities db = new reports_entities();

        public bool isValidHN(string hn)
        {
            var isValid = db.patients.Any(a => a.hospital_number == hn);

            return isValid;
        }

        public Guid GetPatientId(string hn)
        {
            var patientId = db.patients.Where(a => a.hospital_number == hn).Select(a => a.patient_id).First();

            return patientId;
        }

        public IEnumerable<patient> GetPatient(string hn)
        {
            var patient = db.patients.Where(a => a.hospital_number == hn).ToList();

            return patient;
        }

        public IEnumerable<patient_allergies> GetAllergies(Guid patient_id)
        {
            var allergies = db.patient_allergies.Where(a => a.patient_id == patient_id).ToList().OrderBy(a => a.cause);

            return allergies;
        }

        public IEnumerable<patient_diagnosis> GetDiagnosis(Guid patient_id)
        {
            var diagnosis = db.patient_diagnosis.Where(a => a.patient_id == patient_id).OrderByDescending(a => a.recorded_date_time);

            return diagnosis;
        }

        public IEnumerable<patient_previous_hospitalizations> GetHospitalizations(Guid patient_id)
        {
            var hospitalizations = db.patient_previous_hospitalizations.Where(a => a.patient_id == patient_id).OrderByDescending(a => a.actual_visit_date_time);

            return hospitalizations;
        }

        public IEnumerable<patient_medications> GetMedications(Guid patient_id)
        {
            var medications = db.patient_medications.Where(a => a.patient_id == patient_id).OrderByDescending(a => a.note_date);

            return medications;
        }

        public IEnumerable<patient_previous_surgeries> GetSurgeries(Guid patient_id)
        {
            var surgeries = db.patient_previous_surgeries.Where(a => a.patient_id == patient_id).OrderByDescending(a => a.previous_surgeries);

            return surgeries;
        }

        public void InitializePDF(Document doc, MemoryStream mst)
        {
            PdfWriter writer = PdfWriter.GetInstance(doc, mst);
            writer.CloseStream = false;

            doc.SetPageSize(PageSize.A4);

            doc.Open();
        }

        public PdfPTable CreateTable(int column_size)
        {
            PdfPTable table = new PdfPTable(column_size);
            table.WidthPercentage = 100;
            table.DefaultCell.Padding = 8;

            return table;
        }

        public PdfPTable ImageHeader()
        {
            Image logo = Image.GetInstance(@"C:\Users\Administrator\Documents\Erjel Project Files (Web APP)\WEBAPP\Portal\Images\pdf_logo.png");
            //Image logo = Image.GetInstance(@"C:\inetpub\wwwroot\pdf_logo.png");

            PdfPTable headerImg = new PdfPTable(1);
            headerImg.WidthPercentage = 30;
            headerImg.DefaultCell.Border = Rectangle.NO_BORDER;
            headerImg.AddCell(logo);

            return headerImg;
        }

    }
}