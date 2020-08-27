using AutoMapper;
using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DentistApp.Application.Mapping.IMapFrom;

namespace DentistApp.Application.ViewModels
{
    public class PatientInfoForIndexVM : IMapFrom<Patient>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return Name + " " + LastName; }
        }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? NextVisit { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Patient, PatientInfoForIndexVM>().ReverseMap();

        }
    }
}
