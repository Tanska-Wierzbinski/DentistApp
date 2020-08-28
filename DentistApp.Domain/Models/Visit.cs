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
        InProgress,
        Canceled,
        Done
    }
    //public enum Office
    //{
    //    Office1,
    //    Office2,
    //    Office3,
    //    Office4

    //}
    public class Visit
    {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }
        public Status VisitStatus { get; set; }
        public string Diagnosis { get; set; }
        public string Procedure { get; set; }
        [ForeignKey("DentistId")]
        public int DentistId { get; set; }
        [ForeignKey("PatientId")]
        public int PatientId { get; set; }
    }
}
