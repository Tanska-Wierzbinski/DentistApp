using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DentistApp.Domain.Models
{
    public class Dentist : Person
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
