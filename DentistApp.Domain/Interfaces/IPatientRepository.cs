using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistApp.Domain.Interfaces
{
    public interface IPatientRepository
    {
        IQueryable<Patient> GetAll();
        Patient GetById(int patientId);
        Patient GetByIdWithAddress(int patientId);
        Task Add(Patient patient);
        Task Update(Patient patient);
        Task Delete(int patientId);

    }
}
