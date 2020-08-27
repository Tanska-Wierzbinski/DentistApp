using AutoMapper;
using AutoMapper.QueryableExtensions;
using DentistApp.Application.Interfaces;
using DentistApp.Application.ViewModels;
using DentistApp.Domain.Interfaces;
using DentistApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DentistApp.Application.Services
{
    public class DentistAppService : IDentistAppService
    {
        private readonly IVisitRepository _visitRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDentistRepository _dentistRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        public DentistAppService(IVisitRepository visitRepository, IPatientRepository patientRepository, IDentistRepository dentistRepository, IAddressRepository addressRepository, IMapper mapper)
        {
            _visitRepository = visitRepository;
            _patientRepository = patientRepository;
            _dentistRepository = dentistRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public VisitInfoForCancelVM Cancel_Get(int visitId)
        {
            var visit = _mapper.Map<VisitInfoForCancelVM>(_visitRepository.GetByIdForCancel(visitId));
            return visit;
        }
        public async Task Cancel_Post(int visitId)
        {
            var visit = _visitRepository.GetById(visitId);
            visit.VisitStatus = Status.Canceled;
            await _visitRepository.Update(visit);

        }
        public PatientInfoForIndexListVM GetAllPatient()
        {
            var patients = _patientRepository.GetAll().ProjectTo<PatientInfoForIndexVM>(_mapper.ConfigurationProvider).ToList();
            foreach (var patient in patients)
            {
                var visit = _visitRepository.GetForPatient(patient.Id).Where(p => p.VisitDate.CompareTo(DateTime.Now) > 0).Select(p => p.VisitDate);//.DefaultIfEmpty(default).Min();//.Min();

                if (visit.Any())
                    patient.NextVisit = visit.Min();
            }
            return new PatientInfoForIndexListVM()
            {
                Patients = patients
            };
        }

        public VisitForDateListVM GetVisitsForDate(DateTime date)
        {
            var visits = _visitRepository.GetForDate(date).ProjectTo<VisitForDateVM>(_mapper.ConfigurationProvider).ToList();
            foreach(var visit in visits)
            {
                if(date >= visit.VisitDate && date < visit.VisitDate.AddMinutes(30) && visit.VisitStatus!=Status.Canceled)
                {
                    visit.VisitStatus = Status.InProgress;
                    var v = _mapper.Map<Visit>(visit);
                     _visitRepository.Update(v);
                }
                else if(date >= visit.VisitDate.AddMinutes(30) && visit.VisitStatus != Status.Canceled)
                {
                    visit.VisitStatus = Status.Done;
                    var v = _mapper.Map<Visit>(visit);
                     _visitRepository.Update(v);
                }
            }
            return new VisitForDateListVM()
            {
                Visits = visits,
                Count = visits.Count(),
                DoneVisits = visits.Count(v => v.VisitStatus == Domain.Models.Status.Done),
                CurrentDate = DateTime.Now.Date
            };
        }



        public VisitInfoForIndexListVM GetAllVisits()
        {
            var visits = _visitRepository.GetAll().ProjectTo<VisitInfoForIndexVM>(_mapper.ConfigurationProvider)
                                                    .OrderBy(v => v.VisitDate.Date)
                                                    .ThenBy(v => v.VisitDate.TimeOfDay).ToList();
            foreach (var visit in visits)
            {
                if (DateTime.Now >= visit.VisitDate && DateTime.Now < visit.VisitDate.AddMinutes(30) && visit.VisitStatus != Status.Canceled)
                {
                    visit.VisitStatus = Status.InProgress;
                    var v = _mapper.Map<Visit>(visit);
                    _visitRepository.Update(v);
                }
                else if (DateTime.Now >= visit.VisitDate.AddMinutes(30) && visit.VisitStatus != Status.Canceled)
                {
                    visit.VisitStatus = Status.Done;
                    var v = _mapper.Map<Visit>(visit);
                    _visitRepository.Update(v);
                }
            }
            var dates = visits.GroupBy(d => d.VisitDate.Date).Select(d => d.Key);

            return new VisitInfoForIndexListVM()
            {
                Visits = visits,
                Dates = dates.ToList()
            };
        }

        public PatientCardVM GetPatientCard(int patientId)
        {
            var patient = _mapper.Map<PatientInfoForPatientCardVM>(_patientRepository.GetByIdWithAddress(patientId));
            var visits = _visitRepository.GetForPatient(patientId).OrderBy(v=>v.VisitDate.Date).ThenBy(v=>v.VisitDate.TimeOfDay).ProjectTo<VisitInfoForPatientCardVM>(_mapper.ConfigurationProvider);
            return new PatientCardVM()
            {
                Patient = patient,
                Visits = visits.ToList()
            };
        }

        public TemporaryVisitVM AddVisit_Get(DateTime? date, int? dentistId)
        {
            if (date == null)
            {
                date = DateTime.Today;
            }
            var dentists = _dentistRepository.GetAll().ProjectTo<DentistBasicInfoVM>(_mapper.ConfigurationProvider);
            var patients = _patientRepository.GetAll().ProjectTo<PatientBasicInfoVM>(_mapper.ConfigurationProvider);
            var bookedVisits = new List<DateTime>();
            var availableVisits = new List<TimeSpan>();

            CheckForAvailableVisits(dentists, patients, bookedVisits, availableVisits, date.Value, dentistId);

            var dent = dentists.Select(s => new SelectListItem { Text = s.Name + " " + s.LastName, Value = s.Id.ToString() }).ToList();
            dent.Insert(0, new SelectListItem { Text = "Any", Value = "0" });

            return new TemporaryVisitVM()
            {
                Dentists = dent,
                Patients = patients.Select(s => new SelectListItem { Text = s.Name + " " + s.LastName, Value = s.Id.ToString() }).ToList(),
                AvailableVisits = availableVisits,
                VisitDate = date.Value,
                DentistId = dentistId.GetValueOrDefault(0)
            };
        }

        public async Task<int> AddVisit_Post(TemporaryVisitVM tempVisit)
        {
            tempVisit.VisitDate = tempVisit.VisitDate + tempVisit.TimeOfVisit;
            var visits = _visitRepository.GetForDateTime(tempVisit.VisitDate);
            if (tempVisit.DentistId == 0)
            {
                if (visits.Any())
                {
                    if (visits.Select(v => v.PatientId).Contains(tempVisit.PatientId))
                    {
                        return 1;
                    }
                    var dentists = _dentistRepository.GetAll().Select(d => d.Id).ToList();
                    foreach (var v in visits)
                    {
                        if (dentists.Contains(v.DentistId))
                        {
                            dentists.Remove(v.DentistId);
                        }
                    }
                    if (dentists.Any())
                    {
                        tempVisit.DentistId = dentists.First();
                    }
                }
                else
                {
                    tempVisit.DentistId = _dentistRepository.GetAll().Select(d => d.Id).First();
                }
            }

            var visit = _mapper.Map<Visit>(tempVisit);
            await _visitRepository.Add(visit);
            return 0;
        }

        public TemporaryVisitVM EditVisit_Get(DateTime? date, int? dentistId, int visitId)
        {
            var result = _visitRepository.GetById(visitId);
            if (!date.HasValue)
            {
                date = result.VisitDate;
            }
            if (!dentistId.HasValue)
            {
                dentistId = result.DentistId;
            }

            var dentists = _dentistRepository.GetAll().ProjectTo<DentistBasicInfoVM>(_mapper.ConfigurationProvider);
            var patients = _patientRepository.GetAll().ProjectTo<PatientBasicInfoVM>(_mapper.ConfigurationProvider);
            var bookedVisits = new List<DateTime>();
            var availableVisits = new List<TimeSpan>();

            CheckForAvailableVisits(dentists, patients, bookedVisits, availableVisits, date.Value, dentistId);

            var dent = dentists.Select(s => new SelectListItem { Text = s.Name + " " + s.LastName, Value = s.Id.ToString() }).ToList();
            dent.Insert(0, new SelectListItem { Text = "Any", Value = "0" });
            return new TemporaryVisitVM()
            {
                Id = result.Id,
                Dentists = dent,
                Patients = patients.Select(s => new SelectListItem { Text = s.Name + " " + s.LastName, Value = s.Id.ToString() }).ToList(),//patients.ToList(),
                //BookedVisits = bookedVisits,
                AvailableVisits = availableVisits,
                VisitDate = date.Value,
                DentistId = dentistId.GetValueOrDefault(0),
                PatientId = result.PatientId
            };
        }

        public async Task<int> EditVisit_Post(TemporaryVisitVM tempVisit)
        {
            tempVisit.VisitDate = tempVisit.VisitDate + tempVisit.TimeOfVisit;
            var visits = _visitRepository.GetForDateTime(tempVisit.VisitDate);
            if (tempVisit.DentistId == 0)
            {
                if (visits.Any())
                {
                    if (visits.Select(v => v.PatientId).Contains(tempVisit.PatientId))
                    {
                        return 1;
                    }
                    var dentists = _dentistRepository.GetAll().Select(d => d.Id).ToList();
                    foreach (var v in visits)
                    {
                        if (dentists.Contains(v.DentistId))
                        {
                            dentists.Remove(v.DentistId);
                        }
                    }
                    if (dentists.Any())
                    {
                        tempVisit.DentistId = dentists.First();
                    }
                }
                else
                {
                    tempVisit.DentistId = _dentistRepository.GetAll().Select(d => d.Id).First();
                }
            }

            var visit = _mapper.Map<Visit>(tempVisit);
            await _visitRepository.Update(visit);
            return 0;
        }

        public PatientForEditVM EditPatient_Get(int patientId)
        {
            var patient = _mapper.Map<PatientForEditVM>(_patientRepository.GetByIdWithAddress(patientId));
            //var address = _mapper.Map<AddressVM>(_addressRepository.GetById(patientId));

            //patient.Address = _mapper.Map<AddressVM>(_addressRepository.GetById(patientId)); ;
            return patient;
        }

        public async Task EditPatient_Post(PatientForEditVM editedPatient)
        {
            var patient = _mapper.Map<Patient>(editedPatient);
            await _patientRepository.Update(patient);

            //var address = _mapper.Map<Address>(editedPatient.Address);
            //await _addressRepository.Update(address);
        }



        private void CheckForAvailableVisits(IQueryable<DentistBasicInfoVM> dentists, IQueryable<PatientBasicInfoVM> patients, List<DateTime> bookedVisits, List<TimeSpan> availableVisits, DateTime date, int? dentistId)
        {
            for (TimeSpan i = new TimeSpan(8, 0, 0); i <= new TimeSpan(15, 30, 0); i += new TimeSpan(0, 30, 0))
            {
                if ((date.Date < DateTime.Now.Date) || ((date.Date == DateTime.Now.Date) && (i < DateTime.Now.TimeOfDay)))
                {
                    continue;
                }
                availableVisits.Add(i);
            }

            if (!dentistId.HasValue || dentistId.Value == 0)
            {
                bookedVisits = _visitRepository.GetForDate(date.Date)
                                                 .Where(v=>v.VisitStatus == Status.Booked )
                                                 .GroupBy(v => v.VisitDate)
                                                 .Where(v => v.Count() == dentists.Count())
                                                 .Select(v => v.Key).ToList();
            }
            else
            {
                bookedVisits = _visitRepository.GetForDate(date.Date)
                                                .Where(v => v.DentistId == dentistId.Value && v.VisitStatus == Status.Booked)
                                                .Select(v => v.VisitDate).ToList();
            }

            foreach (var visit in bookedVisits)
            {
                if (availableVisits.Contains(visit.TimeOfDay))
                {
                    availableVisits.Remove(visit.TimeOfDay);
                }
            }
        }

        public VisitInfoForDetailsVM GetVisitDetails(int visitId)
        {
            var visit = _mapper.Map<VisitInfoForDetailsVM>(_visitRepository.GetByIdWithDentistAndPatient(visitId));
            
            return visit;
        }

        public async Task AddOrEditDiagnosisAndProcedure(VisitInfoForDetailsVM visit)
        {
            var result = _mapper.Map<Visit>(visit);
            await _visitRepository.Update(result);
        }

        public async Task AddPatient_Post(PatientForEditVM new_patient)
        {
            var patient = _mapper.Map<Patient>(new_patient);
            await _patientRepository.Add(patient);
        }
    }
}
