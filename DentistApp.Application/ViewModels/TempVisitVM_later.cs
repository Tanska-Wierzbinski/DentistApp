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
    public class TempVisitVM_later : IMapFrom<Visit>
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy/HH/mm}", ApplyFormatInEditMode = true)]
        public DateTime VisitDate { get; set; }
        public Status VisitStatus { get; set; }
        public Office VisitOffice { get; set; }

        public void Mapping(Profile profile)
        {
            //profile.CreateMap<Patient, VisitForDateVM>().ForMember(d=>d.Id,opt => opt.Ignore()).ForMember(d=>d.PatientId, opt=>opt.MapFrom(s=>s.Id)).ReverseMap();
            profile.CreateMap<Visit, TempVisitVM_later>().ReverseMap();
        }
    }
}
