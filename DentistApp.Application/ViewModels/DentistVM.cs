using AutoMapper;
using DentistApp.Application.Mapping;
using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DentistApp.Application.Mapping.IMapFrom;

namespace DentistApp.Application.ViewModels
{
    public class DentistVM : DentistBasicInfoVM, IMapFrom<Dentist>
    {
        [Phone]
        [Display(Name ="Phone number")]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<Dentist, DentistVM>().ForMember(p => p.FullName, opt => opt.Ignore()).ReverseMap();
        }
    }
}
