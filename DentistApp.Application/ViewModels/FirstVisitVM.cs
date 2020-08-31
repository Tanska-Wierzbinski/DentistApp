using System;
using System.Collections.Generic;
using System.Text;

namespace DentistApp.Application.ViewModels
{
    public class FirstVisitVM
    {
        public PatientForEditVM Patient { get; set; }
       // public AddressVM Address { get; set; }
        public TemporaryVisitVM Visit { get; set; }
    }
}
