//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.Models.Reports
{
    using System;
    using System.Collections.Generic;
    
    public partial class patient
    {
        public patient()
        {
            this.patient_allergies = new HashSet<patient_allergies>();
            this.patient_diagnosis = new HashSet<patient_diagnosis>();
            this.patient_medications = new HashSet<patient_medications>();
            this.patient_previous_hospitalizations = new HashSet<patient_previous_hospitalizations>();
            this.patient_previous_surgeries = new HashSet<patient_previous_surgeries>();
        }
    
        public System.Guid patient_id { get; set; }
        public string hospital_number { get; set; }
        public Nullable<System.DateTime> registration_date { get; set; }
        public string last_name { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string display_name { get; set; }
        public System.DateTime date_of_birth { get; set; }
        public string gender { get; set; }
        public string marital_status { get; set; }
        public string nationality { get; set; }
        public string highest_education_level { get; set; }
        public string occupation { get; set; }
        public int age { get; set; }
    
        public virtual ICollection<patient_allergies> patient_allergies { get; set; }
        public virtual ICollection<patient_diagnosis> patient_diagnosis { get; set; }
        public virtual ICollection<patient_medications> patient_medications { get; set; }
        public virtual ICollection<patient_previous_hospitalizations> patient_previous_hospitalizations { get; set; }
        public virtual ICollection<patient_previous_surgeries> patient_previous_surgeries { get; set; }
    }
}
