//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.Models.AuditModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class audit_trail
    {
        public System.Guid id { get; set; }
        public int application_id { get; set; }
        public string user_id { get; set; }
        public System.DateTime date_time { get; set; }
        public string device_name { get; set; }
        public string os_version { get; set; }
        public string location { get; set; }
        public string ip_address { get; set; }
        public string action { get; set; }
    }
}