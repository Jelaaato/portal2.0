using Portal.Models.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vereyon.Web;
using PagedList;
using PagedList.Mvc;

namespace Portal.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports

        private OMCP omcp = new OMCP();

        private int pageSize;
        private int pageNumber;
        private string searchstring;
        private Guid patient_id;

        [Authorize]
        public ActionResult OMCP(string search)
        {
            try
            {
                if (search != null)
                {
                    if (omcp.isValidHN(search))
                    {
                        Session["searchstring"] = search;
                        Session["patient_id"] = omcp.GetPatientId(search);
                        ViewBag.DisplayPartialViews = "true";
                        return View();
                    }
                    else
                    {
                        FlashMessage.Info("You have entered an invalid hospital number");
                        ModelState.Clear();
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult OMCPAllergies(int? page, string currentfilter)
        {
            try
            {
                searchstring = Session["searchstring"].ToString();
                patient_id = new Guid(Session["patient_id"].ToString());

                if (searchstring != null && page < 1)
                {
                    page = 1;
                }
                else
                {
                    searchstring = currentfilter;
                }

                ViewBag.CurrentFilter = searchstring;
                pageSize = 10;
                pageNumber = (page ?? 1);

                var allergies = omcp.GetAllergies(patient_id).ToPagedList(pageNumber, pageSize);
                return PartialView("_allergies", allergies);
            }
            catch (Exception)
            {
                return PartialView("_allergies");
            }
        }

        [Authorize]
        public ActionResult OMCPDiagnosis(int? page, string currentfilter)
        {
            try
            {
                searchstring = Session["searchstring"].ToString();
                patient_id = new Guid(Session["patient_id"].ToString());

                if (searchstring != null && page < 1)
                {
                    page = 1;
                }
                else
                {
                    searchstring = currentfilter;
                }

                ViewBag.CurrentFilter = searchstring;
                pageSize = 10;
                pageNumber = (page ?? 1);

                var diagnosis = omcp.GetDiagnosis(patient_id).ToPagedList(pageNumber, pageSize);
                return PartialView("_diagnosis", diagnosis);
            }
            catch (Exception)
            {
                return PartialView("_diagnosis");
            }
        }

        [Authorize]
        public ActionResult OMCPHospitalizations(int? page, string currentfilter)
        {
            try
            {
                searchstring = Session["searchstring"].ToString();
                patient_id = new Guid(Session["patient_id"].ToString());

                if (searchstring != null && page < 1)
                {
                    page = 1;
                }
                else
                {
                    searchstring = currentfilter;
                }

                ViewBag.CurrentFilter = searchstring;
                pageSize = 10;
                pageNumber = (page ?? 1);

                var hospitalizations = omcp.GetHospitalizations(patient_id).ToPagedList(pageNumber, pageSize);
                return PartialView("_hospitalizations", hospitalizations);
            }
            catch (Exception)
            {
                return PartialView("_hospitalizations");
            }
        }

        [Authorize]
        public ActionResult OMCPMedications(int? page, string currentfilter)
        {
            try
            {
                searchstring = Session["searchstring"].ToString();
                patient_id = new Guid(Session["patient_id"].ToString());

                if (searchstring != null && page < 1)
                {
                    page = 1;
                }
                else
                {
                    searchstring = currentfilter;
                }

                ViewBag.CurrentFilter = searchstring;
                pageSize = 10;
                pageNumber = (page ?? 1);

                var medications = omcp.GetMedications(patient_id).ToPagedList(pageNumber, pageSize);
                return PartialView("_medications", medications);
            }
            catch (Exception)
            {
                return PartialView("_medications");
            }
        }

        [Authorize]
        public ActionResult OMCPSurgeries(int? page, string currentfilter)
        {
            try
            {
                searchstring = Session["searchstring"].ToString();
                patient_id = new Guid(Session["patient_id"].ToString());

                if (searchstring != null && page < 1)
                {
                    page = 1;
                }
                else
                {
                    searchstring = currentfilter;
                }

                ViewBag.CurrentFilter = searchstring;
                pageSize = 10;
                pageNumber = (page ?? 1);

                var surgeries = omcp.GetSurgeries(patient_id).ToPagedList(pageNumber, pageSize);
                return PartialView("_surgeries", surgeries);
            }
            catch (Exception)
            {
                return PartialView("_surgeries");
            }
        }
    }
}