using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Portal.Models.IdentityDBModel_TemporaryLogin_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Vereyon.Web;
using Portal.Models.Helpers;
using Portal.Models.ViewModels;
using Portal.Models.BusinessLogic;
using Portal.Models.FileRetentionModel;
using System.Net;
using System.ComponentModel.DataAnnotations;
using Portal.Models.AuditModel;
using PagedList;

namespace Portal.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private Files files = new Files();
        private file_retention_entities db = new file_retention_entities();
        private audit_entities audit = new audit_entities();
        private IdentityDB userdb = new IdentityDB();

        [Authorize(Roles="Administrator")]
        public ActionResult DeleteUsers(string search)
        {
            if (search == null)
            {
                return View(UserManager.Users);
            }
            else
            {
                var user = userdb.Users.Where(a => a.UserName == search).ToList();
                if (user.Count() != 0)
                {
                    ModelState.Clear();
                    return View(user);
                }
                else
                {
                    FlashMessage.Danger("User ID not found");
                    ModelState.Clear();
                    return RedirectToAction("DeleteUsers");
                }
            }
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult UserRoles()
        {
            return View(RoleManager.Roles.OrderBy(a => a.Name));
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult CreateRole()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult CreateRole([Required] string name)
        {
            if (ModelState.IsValid)
            {
                var isExists = userdb.Roles.Any(a => a.Name == name);

                if (isExists)
                {
                    FlashMessage.Danger("Role already exists");
                    return View();
                }

                IdentityResult result = RoleManager.Create(new roles(name));

                if (result.Succeeded)
                {
                    return RedirectToAction("UserRoles");
                }
                else
                {
                    AddErrorsFromResult(result);
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

        [Authorize(Roles = "Administrator")]
        public ActionResult EditRole(string id, RoleModel model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            roles role = (roles)userdb.Roles.Find(id);

            if (role == null)
            {
                return HttpNotFound();
            }
            else
            {
                model.name = role.Name;
                return View(model);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult EditRole(RoleModel model, string id)
        {
            if (ModelState.IsValid)
            {
                roles role = (roles)userdb.Roles.First(a => a.Id == id);
                role.Name = model.name;
                userdb.SaveChanges();
                return RedirectToAction("UserRoles");
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

        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteRole(string id)
        {
            roles role = (roles)userdb.Roles.Find(id);
            userdb.Roles.Remove(role);
            userdb.SaveChanges();
            return RedirectToAction("UserRoles");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult AddToRole(string id, string search)
        {
                roles role = RoleManager.FindById(id);
                string[] memberIDs = role.Users.Select(a => a.UserId).ToArray();
                IEnumerable<Users> members = UserManager.Users.Where(b => memberIDs.Any(c => c == b.Id));
                IEnumerable<Users> nonMembers = UserManager.Users.Except(members);
                return View(new RoleEditModel
                {
                    Role = role,
                    Members = members,
                    NonMembers = nonMembers
                });
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult AddToRole(ModifyRoleModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    result = UserManager.AddToRole(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        FlashMessage.Danger("Updating of User Roles failed");
                        return View();
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    result = UserManager.RemoveFromRole(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        FlashMessage.Danger("Updating of User Roles failed");
                        return View();
                    }
                }
                return RedirectToAction("UserRoles");
            }
            FlashMessage.Danger("Role not found");
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult FileRetention()
        {
            return View(db.file_retention.ToList());
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult AddFileRetention()
        {
            FileRetentionModel model = new FileRetentionModel();
            model.file_types = files.PopulateFileTypeDropdown();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult AddFileRetention(FileRetentionModel model)
        {
            if (ModelState.IsValid)
            {
                if ((model.retention_period <= 0) || (model.retention_period > 30))
                {
                    FlashMessage.Info("Please enter a value greater than 0 and less than 30 days.");
                    return RedirectToAction("AddFileRetention");
                }
                else if (files.isExists(int.Parse(model.file_type)))
                {
                    FlashMessage.Info("Retention period is already set for this file type.");
                    return RedirectToAction("AddFileRetention");
                }
                else
                {
                    int retention_period = model.retention_period * -1;

                    file_retention retention = new file_retention()
                    {
                        file_id = int.Parse(model.file_type),
                        retention_id = Guid.NewGuid(),
                        retention_period = retention_period
                    };

                    db.file_retention.Add(retention);
                    db.SaveChanges();
                    return RedirectToAction("FileRetention");
                }
            }
            model.file_types = files.PopulateFileTypeDropdown();
            var error = (from item in ModelState
                         where item.Value.Errors.Any()
                         select item.Value.Errors[0].ErrorMessage).ToList();

            foreach (var err in error)
            {
                FlashMessage.Danger(err);
            }
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult EditFileRetention(Guid? id, FileRetentionModel model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            file_retention retention = db.file_retention.Find(id);
            if (retention == null)
            {
                return HttpNotFound();
            }
            else
            {
                model.file_type = retention.file_id.GetFileTypeName();
                model.retention_period = retention.retention_period.ReturnAbsoluteValue();
                return View(model);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditFileRetention(FileRetentionModel model, Guid id)
        {
            if ((model.retention_period <= 0) || (model.retention_period > 30))
            {
                FlashMessage.Info("Please enter a value greater than 0 and less than 30 days.");
                return RedirectToAction("EditFileRetention");
            }
            else
            {
                file_retention module = db.file_retention.First(a => a.retention_id == id);
                module.retention_period = model.retention_period * -1;
                db.SaveChanges();
                return RedirectToAction("FileRetention");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteFileRetention(Guid? id)
        {
            file_retention retention = db.file_retention.Find(id);
            db.file_retention.Remove(retention);
            db.SaveChanges();
            return RedirectToAction("FileRetention");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Audit(string search, string filter,  int? page)
        {
            var auditTrail = from a in audit.audit_trail select a;

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = filter;
            }

            ViewBag.Filter = search;

            if (!String.IsNullOrEmpty(search))
            {
                auditTrail = auditTrail.Where(a => a.user_id.Contains(search)).OrderByDescending(a => a.date_time);
                if (auditTrail.Count() == 0)
                {
                    FlashMessage.Danger("User ID not found");
                    ModelState.Clear();
                    return RedirectToAction("Audit");
                }
            }
            else
            {
                auditTrail = auditTrail.OrderByDescending(a => a.date_time);
            }
           

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ModelState.Clear();
            return View(auditTrail.ToPagedList(pageNumber, pageSize));
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

        private RolesManager RoleManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<RolesManager>(); }
        }
    }
}