using AutoMapper;
using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DentistApp.Application.Mapping.IMapFrom;

namespace DentistApp.Application.ViewModels
{
    public class VisitInfoForIndexVM : IMapFrom<Visit>
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Visit date")]
        public DateTime VisitDate { get; set; }

        [Display(Name = "Visit status")]
        public Status VisitStatus { get; set; }

        public string Diagnosis { get; set; }
        public string Procedure { get; set; }
        public DentistBasicInfoVM Dentist { get; set; }
        public PatientBasicInfoVM Patient { get; set; }

        public int DentistId { get; set; }
        public int PatientId { get; set; }




        public void Mapping(Profile profile)
        {
             profile.CreateMap<Visit, VisitInfoForIndexVM>().ReverseMap();

        }
    }
}
