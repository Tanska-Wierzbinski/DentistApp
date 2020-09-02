
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
        Task<VisitForDateListVM> GetVisitsForDate(DateTime date);
        Task<VisitInfoForIndexListVM> GetAllVisits(string sortOrder,int? pageNumber, DateTime? dateMin, DateTime? dateMax, int? dentistId, bool? inFuture);

        DentistListVM GetAllDentists();

        VisitInfoForDetailsVM GetVisitDetails(int visitId);
        DentistInfoForDetailsVM GetDentistDetails(int dentistId);
        Task AddOrEditDiagnosisAndProcedure(VisitInfoForDetailsVM visit);
        VisitInfoForCancelVM Cancel_Get(int visitId);
        Task Cancel_Post(int visitId);

        TemporaryVisitVM AddVisit_Get(DateTime? date, int? dentistId);
        Task<int> AddVisit_Post(TemporaryVisitVM newVisit);
        TemporaryVisitVM EditVisit_Get(DateTime? date, int? dentistId, int visitId);
        Task<int> EditVisit_Post(TemporaryVisitVM editedVisit);
        Task AddPatient_Post(PatientForEditVM newPatient);
        PatientForEditVM EditPatient_Get(int patientId);
        Task EditPatient_Post(PatientForEditVM editedPatient);

        DentistForCreateVM AddDentist_Get(List<string> registeredEmails);
        Task AddDentist_Post(DentistVM newDentist);
        DentistForCreateVM EditDentist_Get(int dentistId, List<string> registeredEmails);
        Task EditDentist_Post(DentistForCreateVM editedDentist);

        FirstVisitVM FirstVisit_Get(PatientForEditVM newPatient, DateTime? date, int? dentistId);
        Task<int> FirstVisit_Post(FirstVisitVM firstVisit);

        PatientInfoForIndexListVM GetAllPatient();
        PatientCardVM GetPatientCard(int patientId);


    }
}
