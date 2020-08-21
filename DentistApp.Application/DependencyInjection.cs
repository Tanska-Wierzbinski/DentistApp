using AutoMapper;
using DentistApp.Application.Interfaces;
using DentistApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;//.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DentistApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IDentistAppService, DentistAppService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
