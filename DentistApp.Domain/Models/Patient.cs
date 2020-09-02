using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DentistApp.Domain.Models
{
    public class Patient : Person
    {
        public string PESEL { get; set; }

        [Display(Name = "Birth date")]
        public DateTime BirthDate { get; set; }
    }
}
