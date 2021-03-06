﻿using AutoMapper;
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
            var visit = _mapper.Map<VisitInfoForCancelVM>(_visitRepository.GetById(visitId));
            visit.Patient = _mapper.Map<PatientBasicInfoVM>(_patientRepository.GetById(visit.PatientId));
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

        public async Task<VisitForDateListVM> GetVisitsForDate(DateTime date)
        {
            var visits = _visitRepository.GetForDate(date).OrderBy(v => v.VisitDate.TimeOfDay)
                                            .ProjectTo<VisitForDateVM>(_mapper.ConfigurationProvider)
                                            .ToList();

            foreach (var visit in visits)
            {
                visit.Dentist = _mapper.Map<DentistBasicInfoVM>(_dentistRepository.GetById(visit.DentistId));
                visit.Patient = _mapper.Map<PatientBasicInfoVM>(_patientRepository.GetById(visit.PatientId));
                if (date >= visit.VisitDate && date < visit.VisitDate.AddMinutes(30) && visit.VisitStatus != Status.Canceled)
                {
                    visit.VisitStatus = Status.InProgress;
                    var v = _mapper.Map<Visit>(visit);
                    await _visitRepository.Update(v);
                }
                else if (date >= visit.VisitDate.AddMinutes(30) && visit.VisitStatus != Status.Canceled)
                {
                    visit.VisitStatus = Status.Done;
                    var v = _mapper.Map<Visit>(visit);
                    await _visitRepository.Update(v);
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

        public async Task<VisitInfoForIndexListVM> GetAllVisits(string sortOrder, int? pageNumber, DateTime? dateMin, DateTime? dateMax, int? dentistId, bool? inFuture)
        {
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "" : sortOrder;
            var temporaryVisits = _visitRepository.GetAll().ProjectTo<VisitInfoForIndexVM>(_mapper.ConfigurationProvider);
            switch (sortOrder)
            {
                case "":
                    temporaryVisits = temporaryVisits.OrderBy(v => v.VisitDate.Date)
                                                     .ThenBy(v => v.VisitDate.TimeOfDay);
                    break;
                case "descending":
                    temporaryVisits = temporaryVisits.OrderByDescending(v => v.VisitDate.Date)
                                                     .ThenBy(v => v.VisitDate.TimeOfDay);
                    break;
            }

            if (inFuture.HasValue && inFuture.Value == true)
            {
                temporaryVisits = temporaryVisits.Where(v => v.VisitDate > DateTime.Now);
            }
            if (dateMax.HasValue && dateMin.HasValue)
            {
                temporaryVisits = dateMin < dateMax ? temporaryVisits.Where(v => v.VisitDate >= dateMin && v.VisitDate <= dateMax.Value.AddDays(1)) :
                                             temporaryVisits.Where(v => v.VisitDate >= dateMax && v.VisitDate <= dateMin.Value.AddDays(1));

            }
            else if (dateMax.HasValue) temporaryVisits = temporaryVisits.Where(v => v.VisitDate <= dateMax.Value.AddDays(1));
            else if (dateMin.HasValue) temporaryVisits = temporaryVisits.Where(v => v.VisitDate >= dateMin);

            if (dentistId.HasValue && dentistId.Value != 0)
            {
                temporaryVisits = temporaryVisits.Where(v => v.DentistId == dentistId);
            }

            var visits = temporaryVisits.ToList();

            foreach (var visit in visits)
            {
                visit.Dentist = _mapper.Map<DentistBasicInfoVM>(_dentistRepository.GetById(visit.DentistId));
                visit.Patient = _mapper.Map<PatientBasicInfoVM>(_patientRepository.GetById(visit.PatientId));
                if (DateTime.Now >= visit.VisitDate && DateTime.Now < visit.VisitDate.AddMinutes(30) && visit.VisitStatus != Status.Canceled && visit.VisitStatus != Status.InProgress)
                {
                    visit.VisitStatus = Status.InProgress;
                    var v = _mapper.Map<Visit>(visit);
                    await _visitRepository.Update(v);
                }
                else if (DateTime.Now >= visit.VisitDate.AddMinutes(30) && visit.VisitStatus != Status.Canceled && visit.VisitStatus != Status.Done)
                {
                    visit.VisitStatus = Status.Done;
                    var v = _mapper.Map<Visit>(visit);
                    await _visitRepository.Update(v);
                }
            }
            var dentists = _dentistRepository.GetAll().ProjectTo<DentistBasicInfoVM>(_mapper.ConfigurationProvider);
            var dent = dentists.Select(s => new SelectListItem { Text = s.Name + " " + s.LastName, Value = s.Id.ToString() }).ToList();
            dent.Insert(0, new SelectListItem { Text = "Any", Value = "0" });
            var pageSize = 2;
            var dates = PaginatedList<DateTime>.Create(visits.GroupBy(d => d.VisitDate.Date).Select(d => d.Key).AsQueryable(), pageNumber ?? 1, pageSize);

            var min_date = string.IsNullOrEmpty(sortOrder) ? visits.Select(v => v.VisitDate).FirstOrDefault() : visits.Select(v => v.VisitDate).LastOrDefault();
            var max_date = string.IsNullOrEmpty(sortOrder) ? visits.Select(v => v.VisitDate).LastOrDefault() : visits.Select(v => v.VisitDate).FirstOrDefault();

            return new VisitInfoForIndexListVM()
            {
                Visits = visits,
                PaginatedDates = dates,
                Dentists = dent,
                DentistId = dentistId.GetValueOrDefault(0),
                DateMax = dateMax.GetValueOrDefault(max_date).Date,
                DateMin = dateMin.GetValueOrDefault(min_date).Date,
                InFuture = inFuture.GetValueOrDefault(false),
                SortOrder = sortOrder

            };
        }

        public PatientCardVM GetPatientCard(int patientId)
        {
            var patient = _mapper.Map<PatientInfoForPatientCardVM>(_patientRepository.GetById(patientId));
            patient.Address = _mapper.Map<AddressVM>(_addressRepository.GetById(patientId));
            var visits = _visitRepository.GetForPatient(patientId).OrderBy(v => v.VisitDate.Date).ThenBy(v => v.VisitDate.TimeOfDay).ProjectTo<VisitInfoForPatientCardVM>(_mapper.ConfigurationProvider).ToList();
            foreach (var visit in visits)
            {
                visit.Dentist = _mapper.Map<DentistBasicInfoVM>(_dentistRepository.GetById(visit.DentistId));
            }
            return new PatientCardVM()
            {
                Patient = patient,
                Visits = visits
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

        public async Task<int> AddVisit_Post(TemporaryVisitVM newVisit)
        {
            newVisit.VisitDate = newVisit.VisitDate + newVisit.TimeOfVisit;
            var visits = _visitRepository.GetForDateTime(newVisit.VisitDate);
            if (newVisit.DentistId == 0)
            {
                if (visits.Any())
                {
                    if (visits.Select(v => v.PatientId).Contains(newVisit.PatientId))
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
                        newVisit.DentistId = dentists.First();
                    }
                }
                else
                {
                    newVisit.DentistId = _dentistRepository.GetAll().Select(d => d.Id).First();
                }
            }

            var visit = _mapper.Map<Visit>(newVisit);
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
                Patients = patients.Select(s => new SelectListItem { Text = s.Name + " " + s.LastName, Value = s.Id.ToString() }).ToList(),
                AvailableVisits = availableVisits,
                VisitDate = date.Value,
                DentistId = dentistId.GetValueOrDefault(0),
                PatientId = result.PatientId
            };
        }

        public async Task<int> EditVisit_Post(TemporaryVisitVM editedVisit)
        {
            editedVisit.VisitDate = editedVisit.VisitDate + editedVisit.TimeOfVisit;
            var visits = _visitRepository.GetForDateTime(editedVisit.VisitDate);
            if (editedVisit.DentistId == 0)
            {
                if (visits.Any())
                {
                    if (visits.Select(v => v.PatientId).Contains(editedVisit.PatientId))
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
                        editedVisit.DentistId = dentists.First();
                    }
                }
                else
                {
                    editedVisit.DentistId = _dentistRepository.GetAll().Select(d => d.Id).First();
                }
            }

            var visit = _mapper.Map<Visit>(editedVisit);
            await _visitRepository.Update(visit);
            return 0;
        }

        public PatientForEditVM EditPatient_Get(int patientId)
        {
            var patient = _mapper.Map<PatientForEditVM>(_patientRepository.GetById(patientId));
            patient.Address = _mapper.Map<AddressVM>(_addressRepository.GetById(patientId));

            return patient;
        }

        public async Task EditPatient_Post(PatientForEditVM editedPatient)
        {
            var patient = _mapper.Map<Patient>(editedPatient);
            var address = _mapper.Map<Address>(editedPatient.Address);
            address.PatientId = patient.Id;
            await _addressRepository.Update(address);
            await _patientRepository.Update(patient);
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
                                                 .Where(v => v.VisitStatus == Status.Booked)
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
            var visit = _mapper.Map<VisitInfoForDetailsVM>(_visitRepository.GetById(visitId));
            visit.Dentist = _mapper.Map<DentistBasicInfoVM>(_dentistRepository.GetById(visit.DentistId));
            visit.Patient = _mapper.Map<PatientBasicInfoVM>(_patientRepository.GetById(visit.PatientId));

            return visit;
        }

        public async Task AddOrEditDiagnosisAndProcedure(VisitInfoForDetailsVM visit)
        {
            var result = _mapper.Map<Visit>(visit);
            await _visitRepository.Update(result);
        }

        public FirstVisitVM FirstVisit_Get(PatientForEditVM newPatient, DateTime? date, int? dentistId)
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

            var visit = new TemporaryVisitVM()
            {
                Dentists = dent,
                PatientId = newPatient.Id,
                AvailableVisits = availableVisits,
                VisitDate = date.Value,
                DentistId = dentistId.GetValueOrDefault(0)
            };

            return new FirstVisitVM()
            {
                Patient = newPatient,
                Visit = visit
            };
        }

        public async Task<int> FirstVisit_Post(FirstVisitVM firstVisit)
        {
            firstVisit.Visit.VisitDate = firstVisit.Visit.VisitDate + firstVisit.Visit.TimeOfVisit;
            var visits = _visitRepository.GetForDateTime(firstVisit.Visit.VisitDate);
            if (firstVisit.Visit.DentistId == 0)
            {
                if (visits.Any())
                {
                    if (visits.Select(v => v.PatientId).Contains(firstVisit.Visit.PatientId))
                    {
                        // return 1;
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
                        firstVisit.Visit.DentistId = dentists.First();
                    }
                }
                else
                {
                    firstVisit.Visit.DentistId = _dentistRepository.GetAll().Select(d => d.Id).First();
                }
            }
            var patient = _mapper.Map<Patient>(firstVisit.Patient);
            await _patientRepository.Add(patient);
            var address = _mapper.Map<Address>(firstVisit.Patient.Address);
            address.PatientId = patient.Id;
            await _addressRepository.Add(address);
            var visit = _mapper.Map<Visit>(firstVisit.Visit);
            visit.PatientId = patient.Id;
            await _visitRepository.Add(visit);
            return 0;
        }

        public async Task AddPatient_Post(PatientForEditVM newPatient)
        {
            var patient = _mapper.Map<Patient>(newPatient);
            var address = _mapper.Map<Address>(newPatient.Address);

            await _patientRepository.Add(patient);
            address.PatientId = patient.Id;
            await _addressRepository.Add(address);
        }

        public DentistListVM GetAllDentists()
        {
            var dentists = _dentistRepository.GetAll().ProjectTo<DentistVM>(_mapper.ConfigurationProvider).ToList();
            return new DentistListVM
            {
                Dentists = dentists
            };
        }


        public async Task AddDentist_Post(DentistVM newDentist)
        {
            var dentist = _mapper.Map<Dentist>(newDentist);
            await _dentistRepository.Add(dentist);
        }

        public DentistForCreateVM EditDentist_Get(int dentistId, List<string> registeredEmails)
        {
            var dentist = _mapper.Map<DentistForCreateVM>(_dentistRepository.GetById(dentistId));
            var dentistsEmails = _dentistRepository.GetAll().Select(d => d.Email);
            List<string> availableEmails = new List<string>();
            foreach (var email in registeredEmails)
            {
                if (!dentistsEmails.Contains(email) || dentist.Email == email)
                {
                    availableEmails.Add(email);
                }
            }
            dentist.AvailableEmails = availableEmails.Select(s => new SelectListItem { Text = s, Value = s }).ToList(); 
            return dentist;
        }

        public async Task EditDentist_Post(DentistForCreateVM editedDentist)
        {
            var dentist = _mapper.Map<Dentist>(editedDentist);
            await _dentistRepository.Update(dentist);
        }


        public DentistInfoForDetailsVM GetDentistDetails(int dentistId)
        {
            var dentist = _mapper.Map<DentistVM>(_dentistRepository.GetById(dentistId));
            var visits = _visitRepository.GetForDentist(dentistId).OrderBy(v=>v.VisitDate.Date).ThenBy(v=>v.VisitDate.TimeOfDay).ProjectTo<VisitInfoForDentistDetailsVM>(_mapper.ConfigurationProvider).ToList();

            foreach (var visit in visits)
            {
                visit.Patient = _mapper.Map<PatientBasicInfoVM>(_patientRepository.GetById(visit.PatientId));
            }

            return new DentistInfoForDetailsVM
            {
                Dentist = dentist,
                Visits = visits
            };
        }

        public DentistForCreateVM AddDentist_Get(List<string> registeredEmails)
        {
            var dentistsEmails = _dentistRepository.GetAll().Select(d=>d.Email);
            List<string> availableEmails = new List<string>();
            foreach(var email in registeredEmails)
            {
                if(!dentistsEmails.Contains(email))
                {
                    availableEmails.Add(email);
                }
            }
            return new DentistForCreateVM
            {
                AvailableEmails = availableEmails.Select(s => new SelectListItem { Text = s, Value=s }).ToList()
            };
        }

        public void Dispose()
        {
            _addressRepository?.Dispose();
            _dentistRepository?.Dispose();
            _patientRepository?.Dispose();
            _visitRepository?.Dispose();
        }
    }
}
