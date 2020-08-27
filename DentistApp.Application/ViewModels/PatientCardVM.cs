using DentistApp.Application.ViewModels;
using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DentistApp.Application.ViewModels
{
    public class PatientCardVM
    {
        public PatientInfoForPatientCardVM Patient { get; set; }
        public List<VisitInfoForPatientCardVM> Visits { get; set; }

    }
}
