using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DentistApp.Application.ViewModels
{
    public class VisitForDateListVM
    {
        public List<VisitForDateVM> Visits { get; set; }
        public int Count { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CurrentDate { get; set; }
        public int DoneVisits { get; set; }

    }
}
