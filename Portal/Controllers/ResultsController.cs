using Portal.Models.BusinessLogic;
using Portal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ResultsController : Controller
    {
        // GET: Results
        [Authorize(Roles = "Patient, Doctor, Administrator")]
        public ActionResult LaboratoryResults(string fileid, bool? isvalidated)
        {
            Laboratory lab = new Laboratory();
            LaboratoryModel model = new LaboratoryModel();

            string path = Server.MapPath("~/Results/Laboratory");

            model.results_references = lab.PopulateResultsDropdown();
            model.allLabResults = lab.GetAllResults(path);

            model.fileID = fileid;
            model.isValidated = isvalidated;

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Patient, Doctor, Administrator")]
        public ActionResult LaboratoryResults(string fileid, bool? isvalidated, string search, string lab_order_name)
        {
            Laboratory lab = new Laboratory();
            LaboratoryModel model = new LaboratoryModel();

            string path = Server.MapPath("~/Results/Laboratory");

            if (Request.IsAjaxRequest())
            {
                model.results_references = lab.PopulateResultsDropdown();
                model.allLabResults = lab.GetResults(path, lab_order_name, search);

                model.fileID = fileid;
                model.isValidated = isvalidated;
                return PartialView("_LabResult", model);
            }

            model.results_references = lab.PopulateResultsDropdown();
            model.allLabResults = lab.GetResults(path, lab_order_name, search);

            model.fileID = fileid;
            model.isValidated = isvalidated;

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
    }
}