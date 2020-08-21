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
    public class DentistRepository : IDentistRepository
    {
        private readonly Context _context;
        public DentistRepository(Context context)
        {
            _context = context;
        }

        public async Task Add(Dentist dentist)
        {
            await _context.Dentists.AddAsync(dentist);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int dentistId)
        {
            var result = await _context.Dentists.SingleOrDefaultAsync(d => d.Id == dentistId);
            _context.Dentists.Remove(result);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Dentist> GetAll()
        {
            return _context.Dentists;
        }

        public async Task<Dentist> GetById(int dentistId)
        {
            return await _context.Dentists.SingleOrDefaultAsync(d => d.Id == dentistId);
        }

        public async Task Update(Dentist dentist)
        {
            _context.Dentists.Update(dentist);
            await _context.SaveChangesAsync();
        }
    }
}
