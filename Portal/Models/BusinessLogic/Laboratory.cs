using Portal.Models.FileRetentionModel;
using Portal.Models.Results;
using Portal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Models.BusinessLogic
{
    public class Laboratory
    {
        private resultsref_entities resultsref = new resultsref_entities();
        private file_retention_entities retention = new file_retention_entities();
        private results_entities results = new results_entities();

        private string currentuser = HttpContext.Current.User.Identity.Name.ToString();

        private DirectoryInfo dir;
        private IEnumerable<FileInfo> files;

        public string Currentuser { get => currentuser; set => currentuser = value; }

        #region PDF Methods

        // These are used when results are already in PDF Files

        private IEnumerable<FileInfo> GetSearchResults(string path, string search, DateTime? minDate)
        {
            dir = new DirectoryInfo(path);

            files = dir.GetFiles("*.pdf").Where(a => a.Name.Contains(Currentuser) && a.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) != -1 && (a.CreationTime > minDate)).OrderByDescending(a => a.CreationTime);

            return files;
        }

        private IEnumerable<FileInfo> GetSearchResultsFilterByLabOrder(string path, string lab_order_name, string search, DateTime? minDate)
        {
            dir = new DirectoryInfo(path);

            files = dir.GetFiles("*.pdf").Where(a => a.Name.Contains(Currentuser) && a.Name.Contains(lab_order_name) && a.Name.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) == 3 && (a.CreationTime > minDate)).OrderByDescending(a => a.CreationTime);

            return files;
        }

        private IEnumerable<FileInfo> GetResultsFilterByLabOrder(string path, string lab_order_name, DateTime? minDate)
        {
            dir = new DirectoryInfo(path);

            files = dir.GetFiles("*.pdf").Where(a => a.Name.Contains(Currentuser) && a.Name.Contains(lab_order_name) && (a.CreationTime > minDate)).OrderByDescending(a => a.CreationTime);

            return files;
        }

        public IEnumerable<FileInfo> GetAllResults(string path, DateTime? minDate)
        {
            dir = new DirectoryInfo(path);

            files = dir.GetFiles("*.pdf").Where(a => a.Name.Contains(Currentuser) && (a.CreationTime > minDate)).OrderByDescending(a => a.CreationTime);

            return files;
        }

        public IEnumerable<FileInfo> GetResults(string path, string lab_order_name, string search, DateTime? minDate)
        {
            if (search != null && lab_order_name == "All Laboratory Results")
            {
                files = this.GetSearchResults(path, search, minDate);
                return files;
            }
            else if (search != null && lab_order_name != "All Laboratory Results")
            {
                files = this.GetSearchResultsFilterByLabOrder(path, search, lab_order_name, minDate);
                return files;
            }
            else if (search == null && lab_order_name != "All Laboratory Results")
            {
                files = this.GetResultsFilterByLabOrder(path, lab_order_name, minDate);
                return files;
            }
            else 
            {
                files = this.GetAllResults(path, minDate);
                return files;
            }
        }

        public SelectList PopulateResultsDropdown()
        {
            var references = resultsref.results_reference.ToList();

            var reference_list = new SelectList(references, "lab_order_name", "lab_order");

            return reference_list;
        }

        public int GetRetentionPeriod(int file_id)
        {
            int period;
            try 
            {
                period = retention.file_retention.Where(a => a.file_id == file_id).Select(a => a.retention_period).First();
                return period;
            }
            catch(Exception)
            {
                return period = -30;
            }
        }

        #endregion

        #region Client Side PDF Generation Methods for Patient

        // These are used when results are built on the fly

        public bool hasResults(string HN)
        {
            var isExists = results.patient_lab_result_header.Any(a => a.hospital_number == HN);
            return isExists;
        }

        public IEnumerable<patient_lab_result_header> GetAllPatientHeader(string HN)
        {
            var allPatientHeader = results.patient_lab_result_header.Where(a => a.hospital_number == HN).GroupBy(a => a.lab_work_order_id).Select(b => b.FirstOrDefault()).ToList();

            return allPatientHeader;
        }

        public IEnumerable<patient_lab_result_header> GetPatientHeader(string HN, string lab_order_name)
        {
            var allPatientHeader = results.patient_lab_result_header.Where(a => a.hospital_number == HN && a.service_category == lab_order_name).GroupBy(a => a.lab_work_order_id).Select(b => b.FirstOrDefault()).ToList();

            return allPatientHeader;
        }

        public IEnumerable<patient_lab_result_header> GetSearchResults(string search, string HN)
        {
            var searchResults = results.patient_lab_result_header.Where(a => a.hospital_number == HN && a.patient_name.IndexOf(search, StringComparison.OrdinalIgnoreCase) != -1).GroupBy(a => a.lab_work_order_id).Select(b => b.FirstOrDefault()).OrderByDescending(a => a.order_date_time).ToList();

            return searchResults;
        }

        public IEnumerable<patient_lab_result_header> GetSearchResultsByLabOrder(string search, string HN, string lab_order_name)
        {
            var searchResults = results.patient_lab_result_header.Where(a => a.hospital_number == HN && a.lab_orderable_name == lab_order_name).GroupBy(a => a.lab_work_order_id).Select(b => b.FirstOrDefault()).OrderByDescending(a => a.order_date_time).ToList();

            return searchResults;
        }

        #endregion

        #region Client Side PDF Generation Methods for Doctors

        public IEnumerable<patient_lab_result_header> GetAllPatientHeaderForDoctor(string doc_emp_nr)
        {
            var allPatientHeaderForDoctor = results.patient_lab_result_header.Where(a => a.doc_emp_nr == doc_emp_nr).GroupBy(a => a.lab_work_order_id).Select(b => b.FirstOrDefault()).OrderByDescending(a => a.reported_date_time);
            //.ToList();

            return allPatientHeaderForDoctor;
        }

        public IEnumerable<patient_lab_result_header> GetPatientHeaderForDoctor(string doc_emp_nr, string lab_order_name)
        {
            var allPatientHeaderForDoctor = results.patient_lab_result_header.Where(a => a.doc_emp_nr == doc_emp_nr && a.service_category == lab_order_name).GroupBy(a => a.lab_work_order_id).Select(b => b.FirstOrDefault()).OrderByDescending(a => a.reported_date_time);
            //.ToList();

            return allPatientHeaderForDoctor;
        }

        //public IEnumerable<patient_lab_result_header> GetSearchResultsForDoctor(string search, string doc_emp_nr)
        //{
        //    var searchResults = results.patient_lab_result_header.Where(a => a.doc_emp_nr == doc_emp_nr && a.patient_name.IndexOf(search, StringComparison.OrdinalIgnoreCase) != -1).GroupBy(a => a.lab_work_order_id).Select(b => b.FirstOrDefault()).OrderByDescending(a => a.order_date_time).ToList();

        //    return searchResults;
        //}

        //public IEnumerable<patient_lab_result_header> GetSearchResultsByLabOrderForDoctor(string search, string doc_emp_nr, string lab_order_name)
        //{
        //    var searchResults = results.patient_lab_result_header.Where(a => a.doc_emp_nr == doc_emp_nr && a.lab_orderable_name == lab_order_name).GroupBy(a => a.lab_work_order_id).Select(b => b.FirstOrDefault()).OrderByDescending(a => a.order_date_time).ToList();

        //    return searchResults;
        //}

        #endregion
    }
}