using Microsoft.AspNet.Identity;
using Portal.Models.IdentityDBModel_TemporaryLogin_;
using Portal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

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
        public ActionResult ValidatePassword(LaboratoryModel model, string id, string accessed_in)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.Find(User.Identity.Name, model.password);
                if (user != null)
                {
                    model.isValidated = true;
                    return RedirectToAction("LaboratoryResults", "Results", new { fileid = id, isvalidated = model.isValidated });
                }
                else
                {
                    model.isValidated = false;
                    ModelState.AddModelError("", "Invalid Password");
                }
            }
            return View(model);
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