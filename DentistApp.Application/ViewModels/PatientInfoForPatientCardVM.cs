using AutoMapper;
using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DentistApp.Application.Mapping.IMapFrom;

namespace DentistApp.Application.ViewModels
{
    public class PatientInfoForPatientCardVM : IMapFrom<Patient>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        public string PESEL { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birth date")]
        public DateTime BirthDate { get; set; }
        public AddressVM Address { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Patient, PatientInfoForPatientCardVM>().ReverseMap();

        }

    }
}
