﻿using AutoMapper;
using DentistApp.Application.Mapping;
using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DentistApp.Application.Mapping.IMapFrom;

namespace DentistApp.Application.ViewModels
{
    public class PatientBasicInfoVM : IMapFrom<Patient>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name ="Last name")]
        public string LastName { get; set; }

        [Display(Name = "Full name")]
        public string FullName
        {
            get { return Name + " " + LastName; }
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Patient, PatientBasicInfoVM>().ForMember(p => p.FullName, opt => opt.Ignore()).ReverseMap();
        }
    }
}
