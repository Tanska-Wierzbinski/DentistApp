using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DentistApp.Domain.Models
{
    public class Dentist : Person
    {
        [Email]
        public string Email { get; set; }
    }
}
