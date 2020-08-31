using AutoMapper;
using DentistApp.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using static DentistApp.Application.Mapping.IMapFrom;

namespace DentistApp.Application.ViewModels
{
    public class RoleForDeleteVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
