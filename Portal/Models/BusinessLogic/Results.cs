using Portal.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.BusinessLogic
{
    public class Results
    {
        private results_entities db = new results_entities();

        public IEnumerable<patient_lab_result_header> GetPatientHeader(string HN, Guid lab_work_order_id)
        {
            if (duplicateLabWorkOrderId(lab_work_order_id))
            {
                var patientHeader = db.patient_lab_result_header.Where(a => a.hospital_number == HN && a.lab_work_order_id == lab_work_order_id).OrderByDescending(a => a.reported_date_time).ToList();

                return patientHeader.Take(1);
            }
            else
            {
                var patientHeader = db.patient_lab_result_header.Where(a => a.hospital_number == HN && a.lab_work_order_id == lab_work_order_id).ToList();

                return patientHeader;
            }
        }

        public IEnumerable<patient_lab_results> GetPatientLabResult(string HN, Guid lab_work_order_id)
        {
            if (duplicateLabWorkOrderId(lab_work_order_id))
            {
                var patientLabResult = db.patient_lab_results.Where(a => a.hospital_number == HN && a.lab_work_order_id == lab_work_order_id).GroupBy(a => a.test).Select(b => b.FirstOrDefault()).OrderBy(a => a.seq_num).ToList();

                return patientLabResult;
            }
            else
            {
                var patientLabResult = db.patient_lab_results.Where(a => a.hospital_number == HN && a.lab_work_order_id == lab_work_order_id).OrderBy(a => a.seq_num).ToList();

                return patientLabResult;
            }
        }

        public IEnumerable<patient_lab_result_header> GetPatientHeaderForDoctor(string doc_emp_nr, Guid lab_work_order_id)
        {
            if (duplicateLabWorkOrderId(lab_work_order_id))
            {
                var patientHeader = db.patient_lab_result_header.Where(a => a.doc_emp_nr == doc_emp_nr && a.lab_work_order_id == lab_work_order_id).OrderByDescending(a => a.reported_date_time).ToList();
               
                return patientHeader.Take(1);
            }
            else
            {
                var patientHeader = db.patient_lab_result_header.Where(a => a.doc_emp_nr == doc_emp_nr && a.lab_work_order_id == lab_work_order_id).ToList();

                return patientHeader;
            }
        }

        public IEnumerable<patient_lab_results> GetPatientLabResultForDoctor(Guid lab_work_order_id)
        {
            if (duplicateLabWorkOrderId(lab_work_order_id))
            {
                var patientLabResult = db.patient_lab_results.Where(a => a.lab_work_order_id == lab_work_order_id).GroupBy(a => a.test).Select(b => b.FirstOrDefault()).OrderBy(a => a.seq_num).ToList();

                return patientLabResult;
            }
            else
            {
                var patientLabResult = db.patient_lab_results.Where(a => a.lab_work_order_id == lab_work_order_id).OrderBy(a => a.seq_num).ToList();

                return patientLabResult; 
            }
        }

        private bool duplicateLabWorkOrderId(Guid lab_work_order_id)
        {
            var countID = db.patient_lab_result_header.Where(a => a.lab_work_order_id == lab_work_order_id).ToList();

            if (countID.Count() > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}