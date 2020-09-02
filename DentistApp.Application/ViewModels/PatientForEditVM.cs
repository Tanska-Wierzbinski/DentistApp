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
    public class PatientForEditVM : IMapFrom<Patient>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[^\W\d]{1,}$", ErrorMessage = "Invalid name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [RegularExpression(@"^[^\W\d]{1,}$", ErrorMessage = "Invalid last name.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"(^\d{9}$)|(^\+\d{2}\s\d{9}$)|(^\+\d{11}$)", ErrorMessage = "Invalid phone number.")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "PESEL is required.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Invalid PESEL.")]
        public string PESEL { get; set; }

        [Display(Name = "Birth date")]
        public DateTime BirthDate { get; set; }
        public AddressVM Address { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Patient, PatientForEditVM>().ReverseMap();
        }
    }
}
