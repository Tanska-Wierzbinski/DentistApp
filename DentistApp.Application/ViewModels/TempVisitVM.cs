using AutoMapper;
using DentistApp.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Text;
using static DentistApp.Application.Mapping.IMapFrom;

namespace DentistApp.Application.ViewModels
{
    public class TempVisitVM : IMapFrom<Visit>
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Data wizyty")]
        public DateTime VisitDate { get; set; }
        [DisplayFormat(DataFormatString ="{0:HH:mm}")]
        public TimeSpan TimeOfVisit { get; set; }
        [DisplayName("Dentysta")]
        public int DentistId { get; set; }
        [DisplayName("Pacjent")]
        public int PatientId { get; set; }
        public List<SelectListItem> Patients { get; set; }
        public List<SelectListItem> Dentists { get; set; }
        public List<TimeSpan> AvailableVisits { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Visit, TempVisitVM>().ForMember(v=>v.AvailableVisits, o=>o.Ignore()).ForMember(v=>v.Patients, o=>o.Ignore()).ForMember(v=>v.Dentists,o=>o.Ignore()).ReverseMap();
        }
        public TempVisitVM()
        {
            VisitDate =  DateTime.Today;
        }
    }
}
