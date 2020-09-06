using AutoMapper;
using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DentistApp.Application.Mapping.IMapFrom;

namespace DentistApp.Application.ViewModels
{
    public class VisitInfoForCancelVM : IMapFrom<Visit>
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Visit date")]
        public DateTime VisitDate { get; set; }

        [Display(Name = "Visit status")]
        public Status VisitStatus { get; set; }
        public int PatientId { get; set; }
        public PatientBasicInfoVM Patient { get; set; }
       


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Visit, VisitInfoForCancelVM>().ReverseMap();
        }
    }
}
