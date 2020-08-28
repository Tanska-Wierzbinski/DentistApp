using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DentistApp.Infrastructure.DTOs
{
    public class VisitForDateDTO
    {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }
        public Status VisitStatus { get; set; }
        //public Office VisitOffice { get; set; }
        public int PatientId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
