using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DentistApp.Application.ViewModels
{
    public class CreateRoleVM
    {
        [Required]
        public string RoleName { get; set; }
    }
}
