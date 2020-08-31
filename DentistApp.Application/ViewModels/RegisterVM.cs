using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DentistApp.Application.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
