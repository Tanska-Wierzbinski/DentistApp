using DentistApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Security
{
    public class CanEditOnlyOwnDiagnosisAndProceduresHandler : AuthorizationHandler<ManageDentistNameRequirement>
    {
        private readonly IDentistAppService _service;
        private readonly IHttpContextAccessor _contextAccessor;
        public CanEditOnlyOwnDiagnosisAndProceduresHandler(IDentistAppService service, IHttpContextAccessor contextAccessor)
        {
            _service = service;
            _contextAccessor = contextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageDentistNameRequirement requirement)
        {
            var authFilterContext = context.Resource as Endpoint;
            //if(authFilterContext == null)
            //{
            //    return Task.CompletedTask;
            //}

            string loggedInUserName = context.User.Identity.Name;
            

            string visitIdBeingEdited = _contextAccessor.HttpContext.Request.Path;

            var visit = _service.GetVisitDetails(Int32.Parse(visitIdBeingEdited.Split('/').Last()));
            var dentist = _service.GetDentistDetails(visit.DentistId);
            if (loggedInUserName == dentist.Dentist.Email)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }   
    }
}
