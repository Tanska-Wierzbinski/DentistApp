﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentistApp.Application.Interfaces;
using DentistApp.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DentistApp.Controllers
{
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
        public ActionResult Index()
        {

            return View(_service.GetVisitsDetails());
        }

        // GET: VisitController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VisitController/Create
        public ActionResult Create(DateTime? date, int? dentistId,string message)
        {
            ViewBag.Message = message;
            return View(_service.AddVisit_Get(date, dentistId));
        }

        // POST: VisitController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, TempVisitVM tempVisit)
        {
            var return_value = await _service.AddVisit_Post(tempVisit);
            if(return_value == 1)
            {
                return RedirectToAction(nameof(Create), new { message = "Ten pacjent ma już umówioną wizytę na tę godzinę", date = tempVisit.VisitDate.Date, dentistId = tempVisit.DentistId});
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: VisitController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VisitController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: VisitController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VisitController/Delete/5
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
