using Portal.Models.AuditModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Models.Helpers
{
    public static class Helper
    {
        public static string Splitter(this string name, char delimiter, int order)
        {
            var split = name.Split(delimiter)[order];
            return split;
        }
    }
}