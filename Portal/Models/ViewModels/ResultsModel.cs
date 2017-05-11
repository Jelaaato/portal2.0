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

        public IEnumerable<FileInfo> allLabResults { get; set; }
        public IEnumerable<FileInfo> hematology { get; set; }
        public IEnumerable<FileInfo> chemistry { get; set; }
        public IEnumerable<FileInfo> clinicalMicroscopy { get; set; }

        public IEnumerable<patient_lab_result_header> patient_lab_header { get; set; }
    }
}