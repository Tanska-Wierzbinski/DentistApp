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
    public class PatientRepository : IPatientRepository
    {

        private readonly Context _context;
        public PatientRepository(Context context)
        {
            _context = context;
        }

        public async Task Add(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int patientId)
        {
            var result = await _context.Patients.SingleOrDefaultAsync(v => v.Id == patientId);
            _context.Patients.Remove(result);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Patient> GetAll()
        {
            return _context.Patients.AsNoTracking();
        }

        public Patient GetById(int patientId)
        {
            return _context.Patients.SingleOrDefault(v => v.Id == patientId);
        }
        //public Patient GetByIdWithAddress(int patientId)
        //{
        //    return _context.Patients.Include(p => p.Address).SingleOrDefault(v => v.Id == patientId);
        //}

        public async Task Update(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }
    }
}
