using Portal.Models.Reports;
using System;
using System.Collections.Generic;
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
    }
}