
using DentistApp.Application.ViewModels;
using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DentistApp.Application.Interfaces
{
    public interface IDentistAppService
    {
        VisitForDateListVM GetVisitsForDate(DateTime date);
        VisitDetailsListVM GetVisitsDetails();
        TempVisitVM AddVisit_Get(DateTime date, int? dentistId);
        Task AddVisit_Post(TempVisitVM tempVisit);
        Task EditVisit();
        Task EditVisit(int idVisit);
        PatientListVM GetAllPatient();
        PatientCardVM GetPatientCard(int patientId);


    }
}
