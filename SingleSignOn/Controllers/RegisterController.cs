using Microsoft.AspNet.Identity;
using SingleSignOn.Models.API;
using SingleSignOn.Models.IdentityDbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using SingleSignOn.Methods;

namespace SingleSignOn.Controllers
{
    [RoutePrefix("api")]
    public class RegisterController : ApiController
    {
        [Route("registeruser")]
        [HttpPost]
        public IHttpActionResult RegisterUser(RegisterUserModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users { UserName = model.firstCredential };
                IdentityResult result = UserManager.Create(user, model.password);
                if (result.Succeeded)
                {
                    var userId = Register.GetUserId(model.firstCredential);
                    var role = Register.GetRole(model.userType);
                    try
                    {
                        UserManager.AddToRole(userId, role);
                        return Ok();
                    }
                    catch (Exception e)
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, String.Format("An error has occured : {0}", e.Message)));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, String.Format("Error : User was not created.")));
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
    }
}
