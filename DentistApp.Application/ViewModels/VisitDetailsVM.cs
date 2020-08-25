using AutoMapper;
using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DentistApp.Application.Mapping.IMapFrom;

namespace DentistApp.Application.ViewModels
{
    public class VisitDetailsVM : IMapFrom<Visit>
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime VisitDate { get; set; }
        public Status VisitStatus { get; set; }
        public Office VisitOffice { get; set; }
        public string PatientName { get; set; }
        public string PatientLastName { get; set; }
        public string PESEL { get; set; }
        public string PhoneNumber { get; set; }
        public string DentistName { get; set; }
        public string DentistLastName { get; set; }


        public void Mapping(Profile profile)
        {
             profile.CreateMap<Visit, VisitDetailsVM>().ForMember(d => d.PatientLastName, opt => opt.MapFrom(s => s.Patient.LastName))
                                                    .ForMember(d => d.PatientName, opt => opt.MapFrom(s => s.Patient.Name))
                                                    .ForMember(d => d.PESEL, opt => opt.MapFrom(s => s.Patient.PESEL))
                                                    .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.Patient.PhoneNumber))
                                                    .ForMember(d => d.DentistName, opt => opt.MapFrom(s => s.Dentist.Name))
                                                    .ForMember(d => d.DentistLastName, opt => opt.MapFrom(s => s.Dentist.LastName))
                                                    .ReverseMap();

        }
    }
}
