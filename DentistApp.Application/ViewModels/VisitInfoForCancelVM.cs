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
        public DateTime VisitDate { get; set; }
        public Status VisitStatus { get; set; }
        public string PatientName { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFullName
        {
            get { return PatientName + " " + PatientLastName; }
        }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Visit, VisitInfoForCancelVM>().ReverseMap();
        }
    }
}
