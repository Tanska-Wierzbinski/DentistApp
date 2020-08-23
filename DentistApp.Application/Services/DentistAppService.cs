using AutoMapper;
using AutoMapper.QueryableExtensions;
using DentistApp.Application.Interfaces;
using DentistApp.Application.ViewModels;
using DentistApp.Domain.Interfaces;
using DentistApp.Domain.Models;
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
                Visits = visits.ToList()
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

        public TempVisitVM AddVisit_Get(DateTime date, int? dentistId)
        {
            var dentists = _dentistRepository.GetAll().ProjectTo<DentistBasicInfo>(_mapper.ConfigurationProvider);
            var patients = _patientRepository.GetAll().ProjectTo<PatientBasicInfo>(_mapper.ConfigurationProvider);
            var bookedVisits = new List<DateTime>();

            if (dentistId == null)
            {
               bookedVisits = _visitRepository.GetForDate(date)
                                                .GroupBy(v => v.VisitDate)
                                                .Where(v => v.Count() == dentists.Count())
                                                .Select(v => v.Key).ToList();
            }
            else
            {
                bookedVisits = _visitRepository.GetForDate(date)
                                                .Where(v => v.DentistId == dentistId)
                                                .Select(v => v.VisitDate).ToList();
            }
            

            return new TempVisitVM()
            {
                Dentists = dentists.ToList(),
                Patients = patients.ToList(),
                BookedVisits = bookedVisits,
                VisitDate = date
            };
        }

        public async Task AddVisit_Post(TempVisitVM tempVisit)
        {
            var visit = _mapper.Map<Visit>(tempVisit);
            await _visitRepository.Add(visit);
        }
    }
}
