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

        private string currentuser = HttpContext.Current.User.Identity.Name.ToString();

        private DirectoryInfo dir;
        private IEnumerable<FileInfo> files;

        #region Methods

        private IEnumerable<FileInfo> GetSearchResults(string path, string search, DateTime? minDate)
        {
            dir = new DirectoryInfo(path);

            files = dir.GetFiles("*.pdf").Where(a => a.Name.Contains(currentuser) && a.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) != -1 && (a.CreationTime > minDate)).OrderByDescending(a => a.CreationTime);

            return files;
        }

        private IEnumerable<FileInfo> GetSearchResultsFilterByLabOrder(string path, string lab_order_name, string search, DateTime? minDate)
        {
            dir = new DirectoryInfo(path);

            files = dir.GetFiles("*.pdf").Where(a => a.Name.Contains(currentuser) && a.Name.Contains(lab_order_name) && a.Name.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) == 3 && (a.CreationTime > minDate)).OrderByDescending(a => a.CreationTime);

            return files;
        }

        private IEnumerable<FileInfo> GetResultsFilterByLabOrder(string path, string lab_order_name, DateTime? minDate)
        {
            dir = new DirectoryInfo(path);

            files = dir.GetFiles("*.pdf").Where(a => a.Name.Contains(currentuser) && a.Name.Contains(lab_order_name) && (a.CreationTime > minDate)).OrderByDescending(a => a.CreationTime);

            return files;
        }

        public IEnumerable<FileInfo> GetAllResults(string path, DateTime? minDate)
        {
            dir = new DirectoryInfo(path);

            files = dir.GetFiles("*.pdf").Where(a => a.Name.Contains(currentuser) && (a.CreationTime > minDate)).OrderByDescending(a => a.CreationTime);

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
    }
}