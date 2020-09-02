using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DentistApp.Application.ViewModels
{
    public class CreateRoleVM
    {
        [Required]
        [Display(Name ="Role name")]
        public string RoleName { get; set; }
    }
}
