using DentistApp.Domain.Interfaces;
using DentistApp.Domain.Models;
using DentistApp.Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistApp.Infrastructure.Repositories
{
    public class VisitRepository : IVisitRepository
    {
        private readonly Context _context;
        public VisitRepository(Context context)
        {
            _context = context;
        }
        public async Task Add(Visit visit)
        {
            await _context.Visits.AddAsync(visit);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int visitId)
        {
            var result = await _context.Visits.SingleOrDefaultAsync(v=>v.Id == visitId);
            _context.Visits.Remove(result);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Visit> GetAll()
        {
            return _context.Visits.AsNoTracking();
        }

        public Visit GetById(int visitId)
        {
            return _context.Visits.SingleOrDefault(v => v.Id == visitId);
        }


        public IQueryable<Visit> GetForDate(DateTime date)
        {
            return _context.Visits.Where(v => v.VisitDate.Date.Equals(date.Date));
        }
        public IQueryable<Visit> GetForDateTime(DateTime date)
        {
            return _context.Visits.Where(v => v.VisitDate.Equals(date));
        }

        public IQueryable<Visit> GetForDentist(int dentistId)
        {
            return _context.Visits.Where(v => v.DentistId == dentistId); 
        }

        public IQueryable<Visit> GetForPatient(int patientId)
        {
            return _context.Visits.Where(v => v.PatientId == patientId);
        }

        public async Task Update(Visit visit)
        {
            
            _context.Visits.Update(visit);
            await _context.SaveChangesAsync();
            
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
