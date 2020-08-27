using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public enum Office
    {
        Office1,
        Office2,
        Office3,
        Office4

    }
    public class Visit
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime VisitDate { get; set; }
        public Status VisitStatus { get; set; }
        public Office VisitOffice { get; set; }
        public string Diagnosis { get; set; }
        public string Procedure { get; set; }

        public int DentistId { get; set; }
        public Dentist Dentist { get; set; }
 
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
