using DentistApp.Application.ViewModels;
using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DentistApp.Application.ViewModels
{
    public class PatientCardVM
    {
        public PatientDetailsVM Patient { get; set; }
        public AddressVM Address { get; set; }
        public List<VisitBasicInfoVM> Visits { get; set; }

    }
}
