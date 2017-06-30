using Microsoft.AspNet.Identity;
using Portal.Models.APIModel;
using Portal.Models.IdentityDBModel_TemporaryLogin_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Portal.Models.BusinessLogic;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    public class ExternalAppLoginController : ApiController
    {
        [Route("externalapplogin")]
        [HttpPost]
        public async Task<IHttpActionResult> ExternalLogin(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = await UserManager.FindAsync(model.username, model.password);

                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    if (user.UserName.checkDumpingStatus() == HttpStatusCode.OK)
                    {
                        ClaimsIdentity claimsident = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                        await SignInAsync(user, isPersistent: false);
                        return Ok(claimsident.Claims);
                    }
                    else
                    {
                        return Conflict();
                    }
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private UsersManager UserManager
        {
            get { return HttpContext.Current.GetOwinContext().GetUserManager<UsersManager>(); }
        }

        private IAuthenticationManager AuthenticationMgr
        {
            get { return HttpContext.Current.GetOwinContext().Authentication; }
        }

        private async Task SignInAsync(Users user, bool isPersistent)
        {
            AuthenticationMgr.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationMgr.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }
    }
}
