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
    public class VisitForDateVM : IMapFrom<Visit> //, IMapFrom<Patient>
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime VisitDate { get; set; }
        public Status VisitStatus { get; set; }
        public Office VisitOffice { get; set; }
       // public int PatientId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public void Mapping(Profile profile)
        {
            //profile.CreateMap<Patient, VisitForDateVM>().ForMember(d=>d.Id,opt => opt.Ignore()).ForMember(d=>d.PatientId, opt=>opt.MapFrom(s=>s.Id)).ReverseMap();
            profile.CreateMap<Visit, VisitForDateVM>().ForMember(d => d.LastName, opt => opt.MapFrom(s => s.Patient.LastName)).ForMember(d=>d.Name, opt=>opt.MapFrom(s=>s.Patient.Name)).ReverseMap();
        }
    }
}
