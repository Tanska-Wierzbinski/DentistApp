
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
        Task<VisitInfoForIndexListVM> GetAllVisits(string sortOrder,int? pageNumber, DateTime? dateMin, DateTime? dateMax, int? dentistId, bool? inFuture);
        VisitInfoForDetailsVM GetVisitDetails(int visitId);
        Task AddOrEditDiagnosisAndProcedure(VisitInfoForDetailsVM visit);
        VisitInfoForCancelVM Cancel_Get(int visitId);
        Task Cancel_Post(int visitId);

        TemporaryVisitVM AddVisit_Get(DateTime? date, int? dentistId);
        Task<int> AddVisit_Post(TemporaryVisitVM tempVisit);
        TemporaryVisitVM EditVisit_Get(DateTime? date, int? dentistId, int visitId);
        Task<int> EditVisit_Post(TemporaryVisitVM tempVisit);
        PatientForEditVM EditPatient_Get(int patientId);
        Task EditPatient_Post(PatientForEditVM patient);

        PatientInfoForIndexListVM GetAllPatient();
        PatientCardVM GetPatientCard(int patientId);


    }
}
