﻿using Portal.Models.AuditModel;
using Portal.Models.FileRetentionModel;
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

        public static string ResetPasswordEmailMessage(string callbackUrl)
        {
            string message = "<b>Good Day!</b><br/><br/><p>You have requested to change your password. <br/><br/> Please reset your password by clicking this </p><br/><a href=\"" + callbackUrl + "\">link</a><br/><br/><b>Thank you.</b>";
            return message;
        }

        public static string GetFileTypeName(this int file_id)
        {
            file_retention_entities db = new file_retention_entities();
            var file_name = db.files.Where(a => a.file_id == file_id).Select(a => a.file_type).First();

            return file_name;
        }
    }
}