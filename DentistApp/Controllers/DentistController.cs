using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentistApp.Application.Interfaces;
using DentistApp.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DentistApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class DentistController : Controller
    {
        private readonly ILogger<DentistController> _logger;
        private readonly IDentistAppService _service;
        private readonly UserManager<IdentityUser> _userManager;
        public DentistController(ILogger<DentistController> logger, IDentistAppService service, UserManager<IdentityUser> userManager)
        {
            _service = service;
            _logger = logger;
            _userManager = userManager;
        }

        [AllowAnonymous]
        // GET: DentistController
        public ActionResult Index()
        {
            return View(_service.GetAllDentists());
        }

        [AllowAnonymous]
        // GET: DentistController/Details/5
        public ActionResult Details(int id)
        {
            return View(_service.GetDentistDetails(id));
        }

        // GET: DentistController/Create
        public ActionResult Create()
        {
            List<string> registeredEmails = new List<string>();
            foreach(var user in _userManager.Users)
            {
                registeredEmails.Add(user.Email);
            }
            return View(_service.AddDentist_Get(registeredEmails));
        }

        // POST: DentistController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, DentistForCreateVM dentist)
        {
            await _service.AddDentist_Post(dentist);
                return RedirectToAction(nameof(Index));
        }

        // GET: DentistController/Edit/5
        public ActionResult Edit(int id)
        {
            List<string> registeredEmails = new List<string>();
            foreach (var user in _userManager.Users)
            {
                registeredEmails.Add(user.Email);
            }
            return View(_service.EditDentist_Get(id, registeredEmails));
        }

        // POST: DentistController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IFormCollection collection, DentistForCreateVM editedDentist)
        {
            _service.EditDentist_Post(editedDentist);
                return RedirectToAction(nameof(Index));
        }

        // GET: DentistController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DentistController/Delete/5
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
