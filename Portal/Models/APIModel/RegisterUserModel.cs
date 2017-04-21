using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models.APIModel
{
    public class RegisterUserModel
    {
        [Required]
        public string firstCredential { get; set; }
        [Range(1, 3)]
        public int userType { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required]
        public string email { get; set; }
    }
}