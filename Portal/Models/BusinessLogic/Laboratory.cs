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
        private string currentuser = HttpContext.Current.User.Identity.Name.ToString();

        private DirectoryInfo dir;
        private IEnumerable<FileInfo> files;

        #region Methods

        private IEnumerable<FileInfo> GetSearchResults(string path, string search)
        {
            dir = new DirectoryInfo(path);

            files = dir.GetFiles("*.pdf").Where(a => a.Name.Contains(currentuser) && a.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) != -1).OrderByDescending(a => a.CreationTime);

            return files;
        }

        private IEnumerable<FileInfo> GetSearchResultsFilterByLabOrder(string path, string lab_order_name, string search)
        {
            dir = new DirectoryInfo(path);

            files = dir.GetFiles("*.pdf").Where(a => a.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) != -1 && a.Name.Contains(currentuser) && a.Name.Contains(lab_order_name)).OrderByDescending(a => a.CreationTime);

            return files;
        }

        private IEnumerable<FileInfo> GetResultsFilterByLabOrder(string path, string lab_order_name)
        {
            dir = new DirectoryInfo(path);

            files = dir.GetFiles("*.pdf").Where(a => a.Name.Contains(currentuser) && a.Name.Contains(lab_order_name)).OrderByDescending(a => a.CreationTime);

            return files;
        }

        public IEnumerable<FileInfo> GetAllResults(string path)
        {
            dir = new DirectoryInfo(path);

            files = dir.GetFiles("*.pdf").Where(a => a.Name.Contains(currentuser)).OrderByDescending(a => a.CreationTime);

            return files;
        }

        public IEnumerable<FileInfo> GetResults(string path, string lab_order_name, string search)
        {
            if (search != null && lab_order_name == "All Laboratory Results")
            {
                files = this.GetSearchResults(path, search);
                return files;
            }
            else if (search != null && lab_order_name != "All Laboratory Results")
            {
                files = this.GetSearchResultsFilterByLabOrder(path, search, lab_order_name);
                return files;
            }
            else if (search == null && lab_order_name != "All Laboratory Results")
            {
                files = this.GetResultsFilterByLabOrder(path, lab_order_name);
                return files;
            }
            else 
            {
                files = this.GetAllResults(path);
                return files;
            }
        }

        public SelectList PopulateResultsDropdown()
        {
            var references = resultsref.results_reference.ToList();

            var reference_list = new SelectList(references, "lab_order_name", "lab_order");

            return reference_list;
        }

        #endregion
    }
}