using AutoMapper;
using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DentistApp.Application.Mapping.IMapFrom;

namespace DentistApp.Application.ViewModels
{
    public class PatientDetailsVM : IMapFrom<Patient>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public string PESEL { get; set; }
        public DateTime BirthDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Patient, PatientDetailsVM>().ReverseMap();

        }

    }
}
