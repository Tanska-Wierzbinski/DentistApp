
using DentistApp.Application.ViewModels;
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
        Task AddVisit();
        Task EditVisit();
        Task EditVisit(int idVisit);
        PatientListVM GetAllPatient();
        PatientCardVM GetPatientCard(int patientId);


    }
}
