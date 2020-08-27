
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
        VisitInfoForIndexListVM GetAllVisits();
        VisitInfoForDetailsVM GetVisitDetails(int visitId);
        Task AddOrEditDiagnosisAndProcedure(VisitInfoForDetailsVM visit);
        TemporaryVisitVM AddVisit_Get(DateTime? date, int? dentistId);
        Task<int> AddVisit_Post(TemporaryVisitVM tempVisit);
        TemporaryVisitVM EditVisit_Get(DateTime? date, int? dentistId, int visitId);
        Task<int> EditVisit_Post(TemporaryVisitVM tempVisit);
        Task AddPatient_Post(PatientForEditVM new_patient);
        PatientForEditVM EditPatient_Get(int patientId);
        Task EditPatient_Post(PatientForEditVM patient);

        PatientInfoForIndexListVM GetAllPatient();
        PatientCardVM GetPatientCard(int patientId);


    }
}
