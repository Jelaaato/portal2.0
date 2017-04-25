using Portal.Models.AuditModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Models.Helpers
{
    public class AuditAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext auditContext)
        {
            var request = auditContext.HttpContext.Request;

            audit_trail audit = new audit_trail()
            {
                id = Guid.NewGuid(),
                application_id = 2,
                date_time = DateTime.Now,
                ip_address = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                user_id = (request.IsAuthenticated) ? auditContext.HttpContext.User.Identity.Name : "Anonymous",
                action = request.RawUrl
            };

            audit_entities ctx = new audit_entities();
            ctx.audit_trail.Add(audit);
            ctx.SaveChanges();

            base.OnResultExecuted(auditContext);
        }
    }
}