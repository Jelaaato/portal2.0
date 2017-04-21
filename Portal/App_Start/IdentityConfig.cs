using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Portal.Models.IdentityDBModel_TemporaryLogin_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

[assembly: OwinStartup(typeof(Portal.App_Start.IdentityConfig))]

namespace Portal.App_Start
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IdentityDB>(IdentityDB.Create);
            app.CreatePerOwinContext<UsersManager>(UsersManager.Create);
            app.CreatePerOwinContext<RolesManager>(RolesManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                ExpireTimeSpan = TimeSpan.FromMinutes(60),
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
        }

        public class EmailService : IIdentityMessageService
        {
            public Task SendAsync(IdentityMessage message)
            {

                var username = "AKIAJ4MXPL5MJQW637LA";
                var pass = "AowC0YgorqxuWSt/88M+LF7fXnSfyhRk4dz7t9PrGs0l";
                //var subj = "Asian Hospital Account Confirmation";

                var sender = "elabetoria@asianhospital.com";
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("email-smtp.us-west-2.amazonaws.com");
                client.Port = 587;
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;

                System.Net.NetworkCredential cred = new System.Net.NetworkCredential(username, pass);

                client.EnableSsl = true;
                client.Credentials = cred;

                var mail = new System.Net.Mail.MailMessage(sender, message.Destination);

                mail.Subject = message.Subject;
                mail.IsBodyHtml = true;
                mail.Body = message.Body;

                return client.SendMailAsync(mail);

            }
        }
    }
}