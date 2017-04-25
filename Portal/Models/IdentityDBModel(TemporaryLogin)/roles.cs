using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models.IdentityDBModel_TemporaryLogin_
{
    public class roles : IdentityRole
    {
        public roles() : base() { }
        public roles(string name) : base(name) { }
    }

    public class RoleModel 
    {
        [Required]
        public string name { get; set; }
    }

    public class RoleEditModel
    {
        public roles Role { get; set; }
        public IEnumerable<Users> Members { get; set; }
        public IEnumerable<Users> NonMembers { get; set; }
    }

    public class ModifyRoleModel
    {
        [Required]
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}