using AutoMapper;
using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Text;
using static DentistApp.Application.Mapping.IMapFrom;

namespace DentistApp.Application.ViewModels
{
    public class TempVisitVM : IMapFrom<Visit>
    {
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime VisitDate { get; set; }
        public int DentistId { get; set; }
        public int PatientId { get; set; }
        public List<PatientBasicInfo> Patients { get; set; }
        public List<DentistBasicInfo> Dentists { get; set; }
        public List<DateTime> BookedVisits { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Visit, TempVisitVM>().ForMember(v=>v.BookedVisits, o=>o.Ignore()).ForMember(v=>v.Patients, o=>o.Ignore()).ForMember(v=>v.Dentists,o=>o.Ignore()).ReverseMap();
            //profile.CreateMap<TempVisitVM, Visit>().ForMember(v => v.Id, o => o.Ignore());
        }
    }
}
