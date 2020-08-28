using AutoMapper;
using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static DentistApp.Application.Mapping.IMapFrom;

namespace DentistApp.Application.ViewModels
{
    public class AddressVM : IMapFrom<Address>
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int Building { get; set; }
        public int? Apartment { get; set; }
        public string Street { get; set; }
        public string City { get; set; }

        [Required(ErrorMessage = "Zip is Required")]
        //[MinLength(5), MaxLength(6)]
        [RegularExpression(@"^\d{2}(-\d{3})?$", ErrorMessage = "Invalid Zip")]
        //[DataType(DataType.PostalCode)]
        [Display(Name = "Zip code")]
        public string PostalCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Address, AddressVM>().ReverseMap();

        }
    }
}
