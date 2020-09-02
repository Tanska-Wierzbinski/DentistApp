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
    public class VisitController : Controller
    {
        private readonly ILogger<VisitController> _logger;
        private readonly IDentistAppService _service;
        public VisitController(ILogger<VisitController> logger, IDentistAppService service)
        {
            _service = service;
            _logger = logger;
        }
        // GET: VisitController
        [AllowAnonymous]
        public async Task<ActionResult> Index(string sortOrder, int? pageNumber, DateTime? dateMin, DateTime? dateMax, int? dentistId, bool? inFuture)
        {

            return View(await _service.GetAllVisits(sortOrder, pageNumber, dateMin, dateMax, dentistId, inFuture));
        }

        // GET: VisitController/Details/5
        [AllowAnonymous]
        [Authorize(Policy = "EditDiagnosisAndProcedure")]
        public ActionResult Details(int id)
        {
            if (!string.IsNullOrEmpty(Request.Headers["Referer"]))
            {
                ViewBag.Reffer = Request.Headers["Referer"].ToString();
            }
            return View(_service.GetVisitDetails(id));
        }

        //GET: VisitController
        public ActionResult FirstVisit(PatientForEditVM patient, DateTime? date, int? dentistId)
        {
            return View(_service.FirstVisit_Get(patient,date,dentistId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FirstVisit(IFormCollection collection, FirstVisitVM firstVisit)
        {
            await _service.FirstVisit_Post(firstVisit);
            return RedirectToAction(nameof(Index));
        }

        // GET: VisitController/Create
        public ActionResult Create(DateTime? date, int? dentistId, string message)
        {
            ViewBag.Message = message;
            return View(_service.AddVisit_Get(date, dentistId));
        }

        // POST: VisitController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, TemporaryVisitVM tempVisit)
        {
            var return_value = await _service.AddVisit_Post(tempVisit);
            if (return_value == 1)
            {
                return RedirectToAction(nameof(Create), new { message = "This patient already has visit at this time.", date = tempVisit.VisitDate.Date, dentistId = tempVisit.DentistId });
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: VisitController/Edit/5
        public ActionResult Edit(DateTime? date, int? dentistId, int id, string message)
        {
            return View(_service.EditVisit_Get(date, dentistId, id));
        }

        // POST: VisitController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IFormCollection collection, TemporaryVisitVM tempVisit)
        {
            var return_value = await _service.EditVisit_Post(tempVisit);
            if (return_value == 1)
            {
                return RedirectToAction(nameof(Edit), new { message = "This patient already has visit at this time.", date = tempVisit.VisitDate.Date, dentistId = tempVisit.DentistId });
            }
            return RedirectToAction(nameof(Index));
        }

        //// GET: VisitController/AddOrEditDiagnosisAndProcedure/5
        //public ActionResult AddOrEditDiagnosisAndProcedure(int visitId)
        //{
        //    return View(_service.GetVisitDetails(visitId));
        //}

        // POST: VisitController/Edit/5
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditDiagnosisAndProcedure")]
        public async Task<ActionResult> AddOrEditDiagnosisAndProcedure(IFormCollection collection, VisitInfoForDetailsVM visit)
        {
            await _service.AddOrEditDiagnosisAndProcedure(visit);
            return RedirectToAction(nameof(Details), new { id = visit.Id });
        }

        // GET: VisitController/Delete/5
        public ActionResult Cancel(int id)
        {
            return View(_service.Cancel_Get(id));
        }

        // POST: VisitController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(int id, IFormCollection collection)
        {
            
           await _service.Cancel_Post(id);
           return RedirectToAction(nameof(Index));

        }
    }
}
