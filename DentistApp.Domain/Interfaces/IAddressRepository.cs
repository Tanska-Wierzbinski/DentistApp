using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DentistApp.Domain.Interfaces
{
    public interface IAddressRepository
    {
        Address GetById(int addressId);
        Task Add(Address address);
        Task Update(Address address);
        Task Delete(int addressId);
    }
}
