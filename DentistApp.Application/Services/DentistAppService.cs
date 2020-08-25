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


        public Task EditVisit()
        {
            throw new NotImplementedException();
        }

        public Task EditVisit(int idVisit)
        {
            throw new NotImplementedException();
        }

        public PatientListVM GetAllPatient()
        {
            var patients = _patientRepository.GetAll().ProjectTo<PatientVM>(_mapper.ConfigurationProvider).ToList();
            foreach (var patient in patients)
            {
                var visit = _visitRepository.GetForPatient(patient.Id).Where(p => p.VisitDate.CompareTo(DateTime.Now) > 0).Select(p => p.VisitDate);//.DefaultIfEmpty(default).Min();//.Min();
                //var next = _visitRepository.GetForPatient(patient.Id).Where(p => p.VisitDate.CompareTo(DateTime.Now) < 0);//.Min(p => p.VisitDate);
                //if (next.Any())
                //    patient.NextVisit = next.Min(p => p.VisitDate);
                if(visit.Any())
                    patient.NextVisit = visit.Min();
            }
            return new PatientListVM()
            {
                Patients = patients
            };
        }

        public VisitForDateListVM GetVisitsForDate(DateTime date)
        {
            var visits = _visitRepository.GetForDate(date).ProjectTo<VisitForDateVM>(_mapper.ConfigurationProvider);

            return new VisitForDateListVM()
            {
                Visits = visits.ToList(),
                Count = visits.Count(),
                DoneVisits = visits.Count(v => v.VisitStatus == Domain.Models.Status.Done),
                CurrentDate = DateTime.Now.Date
            };
        }

        public VisitDetailsListVM GetVisitsDetails()
        {
            var visits = _visitRepository.GetAll().ProjectTo<VisitDetailsVM>(_mapper.ConfigurationProvider);

            return new VisitDetailsListVM()
            {
                Visits = visits.OrderBy(v => v.VisitDate.Date).ThenBy(v=>v.VisitDate.TimeOfDay).ToList()
            };
        }

        public PatientCardVM GetPatientCard(int patientId)
        {
            var patient = _mapper.Map<PatientDetailsVM>(_patientRepository.GetById(patientId));
            var address = _mapper.Map<AddressVM>(_addressRepository.GetById(patientId));
            var visits = _visitRepository.GetForPatient(patientId).ProjectTo<VisitBasicInfoVM>(_mapper.ConfigurationProvider);
            return new PatientCardVM()
            {
                Patient = patient,
                Address = address,
                Visits = visits.ToList()
            };
        }

        public TempVisitVM AddVisit_Get(DateTime? date, int? dentistId)
        {
            if(date == null)
            {
                date = DateTime.Today;
            }
            var dentists = _dentistRepository.GetAll().ProjectTo<DentistBasicInfo>(_mapper.ConfigurationProvider);
            var patients = _patientRepository.GetAll().ProjectTo<PatientBasicInfo>(_mapper.ConfigurationProvider);
            var bookedVisits = new List<DateTime>();
            var availableVisits = new List<TimeSpan>();
            for(TimeSpan i = new TimeSpan(8,0,0);i<=new TimeSpan(15,30,0);i+=new TimeSpan(0,30,0))
            {
                availableVisits.Add(i);
            }

            if (!dentistId.HasValue || dentistId == 0)
            {
               bookedVisits = _visitRepository.GetForDate(date.Value.Date)
                                                .GroupBy(v => v.VisitDate)
                                                .Where(v => v.Count() == dentists.Count())
                                                .Select(v => v.Key).ToList();
            }
            else
            {
                bookedVisits = _visitRepository.GetForDate(date.Value.Date)
                                                .Where(v => v.DentistId == dentistId)
                                                .Select(v => v.VisitDate).ToList();
            }

            foreach(var visit in bookedVisits)
            {
                if(availableVisits.Contains(visit.TimeOfDay))
                {
                    availableVisits.Remove(visit.TimeOfDay);
                }
            }
            var dent = dentists.Select(s => new SelectListItem { Text = s.Name +" "+ s.LastName, Value = s.Id.ToString() }).ToList();
            dent.Insert(0, new SelectListItem { Text = "Dowolny", Value = "0" });
            return new TempVisitVM()
            {
                Dentists = dent,
                Patients = patients.Select(s => new SelectListItem { Text = s.Name +" "+ s.LastName, Value = s.Id.ToString() }).ToList(),//patients.ToList(),
                //BookedVisits = bookedVisits,
                AvailableVisits = availableVisits,
                VisitDate = date.Value,
                DentistId = dentistId.GetValueOrDefault(0)
            };
        }

        public async Task<int> AddVisit_Post(TempVisitVM tempVisit)
        {
            tempVisit.VisitDate = tempVisit.VisitDate + tempVisit.TimeOfVisit;
            var visits = _visitRepository.GetForDateTime(tempVisit.VisitDate);
            if (tempVisit.DentistId == 0)
            {
                if(visits.Any())
                {
                    if(visits.Select(v=>v.PatientId).Contains(tempVisit.PatientId))
                    {
                        ///////////////////////////////////////////////////// 
                        return 1;
                    }
                    var dentists = _dentistRepository.GetAll().Select(d=>d.Id).ToList();
                    foreach(var v in visits)
                    {
                        if(dentists.Contains(v.DentistId))
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
    }
}
