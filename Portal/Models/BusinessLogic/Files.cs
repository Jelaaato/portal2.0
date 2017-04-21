using Portal.Models.FileRetentionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Models.BusinessLogic
{
    public class Files
    {
        private file_retention_entities db = new file_retention_entities();

        public SelectList PopulateFileTypeDropdown()
        {
            var file_types = db.files.ToList();

            var file_types_list = new SelectList(file_types, "file_id", "file_type");

            return file_types_list;
        }

        public bool isExists(int file_id)
        {
            var exists = db.file_retention.Any(a => a.file_id == file_id);

            return exists;
        }
    }
}