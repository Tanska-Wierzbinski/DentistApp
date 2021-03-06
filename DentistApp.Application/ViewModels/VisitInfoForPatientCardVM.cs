﻿using AutoMapper;
using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DentistApp.Application.Mapping.IMapFrom;

namespace DentistApp.Application.ViewModels
{
    public class VisitInfoForPatientCardVM : IMapFrom<Visit>
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Visit date")]
        public DateTime VisitDate { get; set; }

        [Display(Name = "Visit status")]
        public Status VisitStatus { get; set; }

        public int DentistId { get; set; }
        public DentistBasicInfoVM Dentist { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Visit, VisitInfoForPatientCardVM>().ReverseMap();

        }
    }
}
