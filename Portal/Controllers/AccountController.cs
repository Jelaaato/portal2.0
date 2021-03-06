﻿using Microsoft.AspNet.Identity;
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
using Portal.Models.BusinessLogic;
using System.Net;

namespace Portal.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Users user = await UserManager.FindAsync(model.username, model.password);

                if (user == null)
                {
                    FlashMessage.Danger("Invalid User ID or Password");
                }
                else
                {
                    if (user.UserName.checkDumpingStatus() == HttpStatusCode.OK)
                    {
                        ClaimsIdentity claimsident = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        FlashMessage.Danger("User Activation is currently on process.");
                    }
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

        [Authorize]
        public ActionResult Logout()
        {
            Session.Abandon();
            AuthenticationMgr.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = UserManager.ChangePassword(User.Identity.GetUserId(), model.oldPassword, model.newPassword);

                if (result.Succeeded)
                {
                    var user = UserManager.FindById(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }
                    FlashMessage.Info("You have successfully changed your password. Please Login again.");
                    return RedirectToAction("Logout", "Account");
                }
                AddErrorsFromResult(result);
                return View(model);
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

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindByEmail(model.email);

                if (user == null)
                {
                    ModelState.AddModelError("", "You have entered an invalid email");
                    return View(model);
                }
                else
                {
                    string code = UserManager.GeneratePasswordResetToken(user.Id);
                    code = HttpUtility.UrlEncode(code);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    UserManager.SendEmail(user.Id, "ResetPassword", Helper.ResetPasswordEmailMessage(callbackUrl));
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
            }
            var error = (from item in ModelState
                         where item.Value.Errors.Any()
                         select item.Value.Errors[0].ErrorMessage).ToList();

            foreach (var err1 in error)
            {
                FlashMessage.Danger(err1);
            }
            return View();
        }

        public ActionResult ForgotPasswordConfirmation()
        {
            return View();    
        }

        public ActionResult ResetPassword(string code)
        {
            if (code != null)
            {
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Invalid token");
                return View(ModelState);
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindByName(model.username);
                if (user == null)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    var result = UserManager.ResetPassword(user.Id, HttpUtility.UrlDecode(model.code), model.password);
                    if (result.Succeeded)
                    {
                        FlashMessage.Info("You have successfully reset your password. Please Login.");
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Resetting your password failed.");
                        return View(model);
                    }
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

        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        //Helpers

        private UsersManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UsersManager>(); }
        }

        private IAuthenticationManager AuthenticationMgr
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private async Task SignInAsync(Users user, bool isPersistent)
        {
            AuthenticationMgr.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationMgr.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}