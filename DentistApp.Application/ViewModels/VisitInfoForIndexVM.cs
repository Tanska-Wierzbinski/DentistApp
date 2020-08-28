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
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime VisitDate { get; set; }
        public Status VisitStatus { get; set; }
        public string Diagnosis { get; set; }
        public string Procedure { get; set; }
        public DentistBasicInfoVM Dentist { get; set; }
        public PatientBasicInfoVM Patient { get; set; }

        public int DentistId { get; set; }
        public int PatientId { get; set; }




        public void Mapping(Profile profile)
        {
             profile.CreateMap<Visit, VisitInfoForIndexVM>()//.ForMember(d => d.Patient.Id, opt => opt.MapFrom(s=>s.PatientId))
                                                            //.ForMember(d => d.Dentist.Id, opt => opt.MapFrom(s => s.DentistId))
                                                            .ReverseMap();

        }
    }
}
