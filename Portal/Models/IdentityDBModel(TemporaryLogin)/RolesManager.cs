using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.IdentityDBModel_TemporaryLogin_
{
    public class RolesManager : RoleManager<roles>, IDisposable
    {
        public RolesManager(RoleStore<roles> store)
            : base(store) { }

        public static RolesManager Create(IdentityFactoryOptions<RolesManager> options, IOwinContext context)
        {
            return new RolesManager(new RoleStore<roles>(context.Get<IdentityDB>()));
        }
    }
}