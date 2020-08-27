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
     
        public string PatientName { get; set; }
        public string PatientLastName { get; set; }

        public int PatientId { get; set; }
        public int DentistId { get; set; }
        public string DentistName { get; set; }
        public string DentistLastName { get; set; }
        public string PatientFullName
        {
            get { return PatientName + " " + PatientLastName; }
        }
        public string DentistFullName
        {
            get { return DentistName + " " + DentistLastName; }
        }


        public void Mapping(Profile profile)
        {
             profile.CreateMap<Visit, VisitInfoForIndexVM>().ForMember(d => d.PatientLastName, opt => opt.MapFrom(s => s.Patient.LastName))
                                                    .ForMember(d => d.PatientName, opt => opt.MapFrom(s => s.Patient.Name))
                                                    .ForMember(d => d.DentistName, opt => opt.MapFrom(s => s.Dentist.Name))
                                                    .ForMember(d => d.DentistLastName, opt => opt.MapFrom(s => s.Dentist.LastName))
                                                    .ReverseMap();

        }
    }
}
