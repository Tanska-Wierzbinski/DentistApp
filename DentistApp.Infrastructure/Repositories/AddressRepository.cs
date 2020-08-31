using DentistApp.Domain.Interfaces;
using DentistApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistApp.Infrastructure.Repositories
{
    class AddressRepository : IAddressRepository
    {
        private readonly Context _context;
        public AddressRepository(Context context)
        {
            _context = context;
        }

        public async Task Add(Address address)
        {
            _context.Add(address);
            _context.SaveChanges();
        }

        public async Task Delete(int addressId)
        {
            var result = await _context.Addresses.SingleOrDefaultAsync(a => a.PatientId == addressId);
            _context.Addresses.Remove(result);
            await _context.SaveChangesAsync();
        }

        public Address GetById(int patientId)
        {
            return  _context.Addresses.SingleOrDefault(a => a.PatientId == patientId);
        }

        public async Task Update(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
        }
    }
}
