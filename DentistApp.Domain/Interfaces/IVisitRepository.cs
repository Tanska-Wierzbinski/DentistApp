using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DentistApp.Domain.Interfaces
{
    public interface IVisitRepository
    {
        IQueryable<Visit> GetAll();
        IQueryable<Visit> GetForDate(DateTime date);
        IQueryable<Visit> GetForDateTime(DateTime date);
        IQueryable<Visit> GetForPatient(int patientId);
        IQueryable<Visit> GetForDentist(int dentistId);
        Visit GetById(int visitId);
        Visit GetByIdWithDentistAndPatient(int visitId);
        Task Add(Visit visit);
        Task Update(Visit visit);
        Task Delete(int visitId);


    }
}
