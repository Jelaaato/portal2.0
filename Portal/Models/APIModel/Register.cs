using Portal.Models.IdentityDBModel_TemporaryLogin_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.APIModel
{
    public class Register
    {
        private static IdentityDB db = new IdentityDB();

        public static string GetUserId(string userName)
        {
            var userId = db.Users.Where(u => u.UserName == userName).Select(u => u.Id).First();

            return userId;
        }

        public static string GetRole(int userType)
        {
            switch (userType)
            {
                case 1:
                    return "Patient";
                case 2:
                    return "Doctor";
                case 3:
                    return "Employee";
                default:
                    return "Invalid Role";
            }
        }
    }
}