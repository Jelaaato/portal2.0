using Microsoft.AspNet.Identity;
using Portal.Models.IdentityDBModel_TemporaryLogin_;
using Portal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using Vereyon.Web;

namespace Portal.Controllers
{
    public class ValidateController : Controller
    {
        // GET: Validate
        [Authorize]
        public ActionResult ValidatePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ValidatePassword(LaboratoryModel model, Guid id, string accessed_in)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.Find(User.Identity.Name, model.password);
                if (user != null && accessed_in == "mobile")
                {
                    return RedirectToAction("ViewResult", "Results", new { lab_work_order_id = id, isvalidated = model.isValidated });

                }
                else if (user != null && accessed_in == "browser")
                {
                    model.isValidated = true;
                    return RedirectToAction("ViewResult", "Results", new { lab_work_order_id = id, isvalidated = model.isValidated });
                }
                else
                {
                    model.isValidated = false;
                    FlashMessage.Danger("Invalid Password");
                }
            }
            var error = (from item in ModelState
                         where item.Value.Errors.Any()
                         select item.Value.Errors[0].ErrorMessage).ToList();

            foreach (var err in error)
            {
                FlashMessage.Danger(err);
            }
            return View();
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private UsersManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UsersManager>(); }
        }
    }
}