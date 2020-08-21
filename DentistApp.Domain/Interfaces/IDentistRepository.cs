﻿using DentistApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistApp.Domain.Interfaces
{
    public interface IDentistRepository
    {
        IQueryable<Dentist> GetAll();
        Task<Dentist> GetById(int dentistId);
        Task Add(Dentist dentist);
        Task Update(Dentist dentist);
        Task Delete(int dentistId);
    }
}
