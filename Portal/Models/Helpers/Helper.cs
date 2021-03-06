﻿using Portal.Models.AuditModel;
using Portal.Models.FileRetentionModel;
using Portal.Models.IdentityDBModel_TemporaryLogin_;
using Portal.Models.UserDisplayNameModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vereyon.Web;

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

        public static int ReturnAbsoluteValue(this int num)
        {
            var absoluteValue = (num * -1);

            return absoluteValue;
        }

        public static string GetUsername(this string Id)
        {
            IdentityDB db = new IdentityDB();

            var username = db.Users.Where(a => a.Id == Id).Select(a => a.UserName).First();

            return username;
        }

        public static string GetApplicationName(this int application_id)
        {
            audit_entities db = new audit_entities();

            var app_type = db.applications.Where(a => a.application_id == application_id).Select(a => a.application_name).First();

            return app_type;
        }

        public static string GetApplicationType(this int application_id)
        {
            audit_entities db = new audit_entities();

            var app_type = db.applications.Where(a => a.application_id == application_id).Select(a => a.application_type).First();

            return app_type;
        }

        public static string GetActionBasedOnUrl(this string url)
        {
            if (url.Contains("LaboratoryResults?fileid"))
            {
                return "Viewed Result";
            }
            else if (url.Contains("LaboratoryResults"))
            {
                return "Browsed Results";
            }
            else
            {
                return "";
            }
        }

        public static string GetUserDisplayName(this string userid)
        {
            user_entities db = new user_entities();

            var display_name = db.user_display_name.Where(a => a.user_id == userid).Select(a => a.username).First();

            return display_name;
        }

        public static bool checkUserDisplayName(this string userid)
        {
            user_entities db = new user_entities();

            var isExists = db.user_display_name.Any(a => a.user_id == userid);

            return isExists;
        }

        public static string RemoveNonNumericChar(this string value)
        {
            string transformedString = value.Replace("-", string.Empty);

            return transformedString;
        }
    }
}