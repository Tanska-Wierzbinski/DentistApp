using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentistApp.Application.Interfaces;
using DentistApp.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DentistApp.Controllers
{
    [Authorize(Roles = "Admin,Secretary")]
    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IDentistAppService _service;
        public PatientController(ILogger<PatientController> logger, IDentistAppService service)
        {
            _service = service;
            _logger = logger;
        }
        // GET: PatientController
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(_service.GetAllPatient());
        }
        [AllowAnonymous]
        public ActionResult PatientCard(int patientId)
        {
            
            return View(_service.GetPatientCard(patientId));
        }
        // GET: PatientController/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            return View();
        }


        // GET: PatientController/Create
        public ActionResult Create()
        {
            return View(new PatientForEditVM() { BirthDate = DateTime.Today });
        }

        // POST: PatientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, PatientForEditVM newPatient)
        {
            await _service.AddPatient_Post(newPatient);
            return RedirectToAction(nameof(Index));
        }

        // GET: PatientController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_service.EditPatient_Get(id));
        }

        // POST: PatientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IFormCollection collection, PatientForEditVM editedPatient)
        {
            await _service.EditPatient_Post(editedPatient);
            return RedirectToAction(nameof(Index));

        }

        // GET: PatientController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PatientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
