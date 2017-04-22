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
        private Laboratory lab = new Laboratory();
        private Files files = new Files();

        [Authorize(Roles = "Patient, Doctor, Administrator")]
        public ActionResult LaboratoryResults(string fileid, bool? isvalidated, LaboratoryModel model, DateTime? minDate)
        {
            string path = Server.MapPath("~/Results/Laboratory");
            var retention_period = lab.GetRetentionPeriod(1);

            if (retention_period == 0)
            {
                minDate = DateTime.Today.AddDays(-30);
            }
            else
            {
                minDate = DateTime.Today.AddDays(retention_period);
            }

            model.results_references = lab.PopulateResultsDropdown();
            model.allLabResults = lab.GetAllResults(path, minDate);

            model.fileID = fileid;
            model.isValidated = isvalidated;

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Patient, Doctor, Administrator")]
        public ActionResult LaboratoryResults(string fileid, bool? isvalidated, string search, string lab_order_name, LaboratoryModel model, DateTime? minDate)
        {

            string path = Server.MapPath("~/Results/Laboratory");
            var retention_period = lab.GetRetentionPeriod(2);

            if (retention_period == 0)
            {
                minDate = DateTime.Today.AddDays(30);
            }
            else
            {
                minDate = DateTime.Today.AddDays(retention_period);
            }

            if (Request.IsAjaxRequest())
            {
                model.results_references = lab.PopulateResultsDropdown();
                model.allLabResults = lab.GetResults(path, lab_order_name, search, minDate);

                model.fileID = fileid;
                model.isValidated = isvalidated;
                return PartialView("_LabResult", model);
            }

            model.results_references = lab.PopulateResultsDropdown();
            model.allLabResults = lab.GetResults(path, lab_order_name, search, minDate);

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