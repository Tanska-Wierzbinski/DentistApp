using DentistApp.Application.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DentistApp.Application.Mapping.IMapFrom;

namespace DentistApp.Application.ViewModels
{
    public class EditRoleVM
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Role name is required.")]
        public string Name { get; set; }
        public List<string> Users;

        public EditRoleVM()
        {
            Users = new List<string>();
        }
    }
}
