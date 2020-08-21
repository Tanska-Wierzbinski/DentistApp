using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DentistApp.Domain.Models
{
    public class Address
    {
        [Key]
        public int PatientId { get; set; }
        public int Building { get; set; }
        public int? Apartment { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public Patient Patient { get; set; }
    }
}
