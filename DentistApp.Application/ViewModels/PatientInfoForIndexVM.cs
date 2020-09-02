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

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Full name")]
        public string FullName
        {
            get { return Name + " " + LastName; }
        }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Next visit")]
        public DateTime? NextVisit { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Patient, PatientInfoForIndexVM>().ReverseMap();

        }
    }
}
