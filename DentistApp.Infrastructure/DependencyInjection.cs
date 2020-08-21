using DentistApp.Domain.Interfaces;
using DentistApp.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DentistApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IDentistRepository, DentistRepository>();
            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<IVisitRepository, VisitRepository>();
            return services;
        }
    }
}
