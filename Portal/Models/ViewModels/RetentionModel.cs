using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Models.ViewModels
{
        public class FileRetentionModel
        {
            public SelectList file_types { get; set; }
            public int file_id { get; set; }
            public string file_type { get; set; }
            [Required]
            public int retention_period { get; set; }
        }
}