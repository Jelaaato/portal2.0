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
        public ActionResult ValidatePassword(LaboratoryModel model, string id, string accessed_in)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.Find(User.Identity.Name, model.password);
                if (user != null && accessed_in == "mobile")
                {
                    string path = Server.MapPath("~/Results/Laboratory/");
                    DirectoryInfo dir = new DirectoryInfo(path);

                    var filename = dir.GetFiles("*.pdf*").Where(a => a.Name.Contains(id)).Select(b => b.Name).First();
                    return File(path + filename, "application/pdf");

                }
                else if (user != null && accessed_in == "browser")
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