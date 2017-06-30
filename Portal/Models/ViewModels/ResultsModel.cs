using PagedList;
using Portal.Models.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Models.ViewModels
{
    public class LaboratoryModel
    {
        public string password { get; set; }
        public string fileID { get; set; }
        public bool? isValidated { get; set; }

        public SelectList results_references { get; set; }
        public string lab_order_name { get; set; }

        public IPagedList<patient_lab_result_header> patient_lab_header { get; set; }
    }
}