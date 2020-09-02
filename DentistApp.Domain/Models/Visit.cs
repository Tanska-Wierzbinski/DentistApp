using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DentistApp.Domain.Models
{
    public enum Status
    {
        Booked,

        [Display(Name = "In progress")]
        InProgress,

        Canceled,
        Done
    }

    public class Visit
    {
        public int Id { get; set; }

        [Display(Name = "Visit date")]
        public DateTime VisitDate { get; set; }

        [Display(Name = "Visit status")]
        public Status VisitStatus { get; set; }

        public string Diagnosis { get; set; }
        public string Procedure { get; set; }

        [ForeignKey("DentistId")]
        public int DentistId { get; set; }

        [ForeignKey("PatientId")]
        public int PatientId { get; set; }
    }
}
