﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.IdentityDBModel_TemporaryLogin_
{
    public class UsersManager : UserManager<Users>
    {
        public UsersManager(IUserStore<Users> store)
            : base(store) { }

        public static UsersManager Create(IdentityFactoryOptions<UsersManager> options, IOwinContext context)
        {
            IdentityDB db = context.Get<IdentityDB>();
            UsersManager manager = new UsersManager(new UserStore<Users>(db));

            manager.UserValidator = new UserValidator<Users>(manager) { AllowOnlyAlphanumericUserNames = false };
            manager.EmailService = new Portal.App_Start.IdentityConfig.EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<Users>(dataProtectionProvider.Create("Portal"));
            }

            return manager;
        }
    }
}