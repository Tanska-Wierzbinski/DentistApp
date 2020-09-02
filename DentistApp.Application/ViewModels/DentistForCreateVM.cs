using AutoMapper;
using DentistApp.Application.Mapping;
using DentistApp.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using static DentistApp.Application.Mapping.IMapFrom;

namespace DentistApp.Application.ViewModels
{
    public class DentistForCreateVM : DentistVM, IMapFrom<Dentist>
    {
        public List<SelectListItem> AvailableEmails { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<Dentist, DentistForCreateVM>().ForMember(p => p.AvailableEmails, opt => opt.Ignore())
                                                            .ForMember(p => p.FullName, opt => opt.Ignore())
                                                            .ReverseMap();
        }
    }
}
